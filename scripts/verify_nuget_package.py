#!/usr/bin/env python3
"""Verify the NuGet package produced by FSM_COS matches its project contract."""

from __future__ import annotations

import argparse
import posixpath
import re
import sys
import zipfile
from pathlib import Path
from xml.etree import ElementTree as ET


def local_name(tag: str) -> str:
    return tag.rsplit("}", 1)[-1]


def child_text(element: ET.Element, name: str) -> str | None:
    child = next((item for item in element if local_name(item.tag) == name), None)
    return child.text.strip() if child is not None and child.text else None


def fail(message: str) -> None:
    raise ValueError(message)


def verify(package_dir: Path, project_path: Path) -> None:
    project = ET.parse(project_path).getroot()
    property_group = next((item for item in project if local_name(item.tag) == "PropertyGroup"), None)
    if property_group is None:
        fail(f"No PropertyGroup found in {project_path}")

    project_values = {
        local_name(item.tag): (item.text or "").strip()
        for item in property_group
    }
    package_id = project_values.get("PackageId")
    assembly_name = project_values.get("AssemblyName") or project_path.stem
    version = project_values.get("Version")
    readme_name = project_values.get("PackageReadmeFile")
    license_name = project_values.get("PackageLicenseFile")
    if not all((package_id, version, readme_name, license_name)):
        fail("Project must declare PackageId, Version, PackageReadmeFile, and PackageLicenseFile.")

    packages = sorted(package_dir.glob("*.nupkg"))
    if len(packages) != 1:
        fail(f"Expected exactly one .nupkg in {package_dir}; found {len(packages)}.")

    package = packages[0]
    with zipfile.ZipFile(package) as archive:
        names = set(archive.namelist())
        svg_names = sorted(name for name in names if name.lower().endswith(".svg"))
        for svg_name in svg_names:
            try:
                svg_root = ET.fromstring(archive.read(svg_name))
            except ET.ParseError as error:
                fail(f"SVG is not well-formed XML: {svg_name}: {error}")

            role = svg_root.attrib.get("role")
            if "/section-dividers/" in svg_name:
                if role != "presentation" or svg_root.attrib.get("aria-hidden") != "true":
                    fail(f"Decorative section divider must be hidden from assistive technology: {svg_name}.")
            elif "/section-markers/" in svg_name:
                if role != "img" or not svg_root.attrib.get("aria-label", "").strip():
                    fail(f"Section marker must expose an accessible image label: {svg_name}.")
            else:
                children = {local_name(item.tag): item for item in svg_root}
                title = children.get("title")
                description = children.get("desc")
                labelled_ids = set(svg_root.attrib.get("aria-labelledby", "").split())
                required_ids = {
                    item.attrib.get("id")
                    for item in (title, description)
                    if item is not None and item.attrib.get("id")
                }
                if (
                    role != "img"
                    or title is None
                    or description is None
                    or not required_ids
                    or not required_ids.issubset(labelled_ids)
                ):
                    fail(
                        "Substantive SVG must have a title, description, role='img', "
                        f"and matching aria-labelledby references: {svg_name}."
                    )
        nuspecs = [name for name in names if name.lower().endswith(".nuspec")]
        if len(nuspecs) != 1:
            fail(f"Expected exactly one .nuspec in {package.name}; found {len(nuspecs)}.")

        nuspec = ET.fromstring(archive.read(nuspecs[0]))
        metadata = next((item for item in nuspec if local_name(item.tag) == "metadata"), None)
        if metadata is None:
            fail("The package .nuspec has no metadata element.")

        actual_id = child_text(metadata, "id")
        actual_version = child_text(metadata, "version")
        if actual_id != package_id:
            fail(f"Package ID mismatch: project={package_id!r}, package={actual_id!r}.")
        if actual_version != version:
            fail(f"Package version mismatch: project={version!r}, package={actual_version!r}.")

        required_files = {
            readme_name,
            license_name,
            "README.md",
            "SECURITY.md",
            "DOCUMENTATION_INDEX.md",
            f"docs/releases/{version}.md",
            "docs/assets/fsm-cos-overview.svg",
            "docs/assets/fsm-cos-preflight.svg",
            "docs/assets/fsm-cos-crane.svg",
        }
        missing = sorted(required_files - names)
        if missing:
            fail("Package is missing required files: " + ", ".join(missing))

        markdown_link = re.compile(r"!?\[[^\]]*\]\(([^)]+)\)|^\[[^\]]+\]:\s*<?([^>\s]+)>?", re.MULTILINE)
        for markdown_name in sorted(name for name in names if name.lower().endswith(".md")):
            markdown = archive.read(markdown_name).decode("utf-8-sig")
            for match in markdown_link.finditer(markdown):
                target = (match.group(1) or match.group(2) or "").strip().split()[0].strip("<>")
                if not target or target.startswith(("#", "http://", "https://", "mailto:", "data:")):
                    continue
                target_path = target.split("#", 1)[0].split("?", 1)[0]
                if not target_path:
                    continue
                resolved = posixpath.normpath(posixpath.join(posixpath.dirname(markdown_name), target_path))
                if resolved not in names and not any(
                    name.startswith(resolved.rstrip("/") + "/") for name in names
                ):
                    fail(
                        f"Markdown link in {markdown_name} points outside the package or to a missing file: "
                        f"{target!r} (resolved to {resolved!r})."
                    )

        assembly_path = f"lib/net8.0/{assembly_name}.dll"
        if assembly_path not in names:
            fail(f"Package is missing target assembly {assembly_path!r}.")
        xml_doc_path = f"lib/net8.0/{assembly_name}.xml"
        if xml_doc_path not in names:
            fail(f"Package is missing XML API documentation {xml_doc_path!r}.")

        project_dependencies = {
            item.attrib["Include"]: item.attrib["Version"]
            for group in project
            if local_name(group.tag) == "ItemGroup"
            for item in group
            if local_name(item.tag) == "PackageReference"
            and item.attrib.get("Include")
            and item.attrib.get("Version")
            and "WorkshopUseRepositorySources' == 'true'" not in item.attrib.get("Condition", "")
        }
        dependencies_node = next(
            (item for item in metadata if local_name(item.tag) == "dependencies"),
            None,
        )
        package_dependencies: dict[str, str] = {}
        if dependencies_node is not None:
            for group in dependencies_node.iter():
                if local_name(group.tag) == "dependency":
                    package_dependencies[group.attrib.get("id", "")] = group.attrib.get("version", "")

        if any(dependency_id.casefold() == "thesingularityworkshop.gui.svg" for dependency_id in package_dependencies):
            fail("GUI.SVG is build-time documentation tooling and must not be a runtime NuGet dependency.")

        if set(package_dependencies) != set(project_dependencies):
            fail(
                "Package dependency IDs differ from the project contract: "
                f"project={sorted(project_dependencies)}, package={sorted(package_dependencies)}."
            )

        def lower_bound(version_range: str) -> str:
            # NuGet may encode a PackageReference as a lower-bounded range, e.g. [1.2.3, ).
            return version_range.strip().lstrip("[(").split(",", 1)[0].rstrip("]) ").strip()

        for dependency_id, dependency_version in project_dependencies.items():
            actual_dependency_version = package_dependencies.get(dependency_id, "")
            if lower_bound(actual_dependency_version) != dependency_version:
                fail(
                    f"Dependency mismatch for {dependency_id}: project={dependency_version!r}, "
                    f"package={actual_dependency_version!r}."
                )

        readme_metadata = child_text(metadata, "readme")
        if readme_metadata != readme_name:
            fail(f"Package readme metadata mismatch: project={readme_name!r}, package={readme_metadata!r}.")

        print(f"Verified {package.name}")
        print(f"  identity: {actual_id} {actual_version}")
        print(f"  dependencies: {', '.join(sorted(project_dependencies))}")
        print(f"  package documentation: {len(required_files)} required files present")
        print("  assembly and XML API documentation: present")


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--package-dir", required=True, type=Path)
    parser.add_argument("--project", required=True, type=Path)
    args = parser.parse_args()
    try:
        verify(args.package_dir, args.project)
    except (OSError, KeyError, ValueError, ET.ParseError, zipfile.BadZipFile) as error:
        print(f"Package verification failed: {error}", file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
