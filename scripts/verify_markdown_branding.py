#!/usr/bin/env python3
"""Enforce the Workshop moniker header and static avatar-only footer on every Markdown file."""

from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
MONIKER = "https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg"
AVATAR = "https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png"
FOOTER_MARKER = "<!-- THE SINGULARITY WORKSHOP FOOTER -->"

EXCLUDED_PARTS = {".git", "bin", "obj", "_dependencies", "TestResults", "artifacts"}
markdown_files = sorted(
    path for path in ROOT.rglob("*.md")
    if not any(part in EXCLUDED_PARTS for part in path.relative_to(ROOT).parts)
)

errors = []
for path in markdown_files:
    relative = path.relative_to(ROOT).as_posix()
    text = path.read_text(encoding="utf-8")
    lines = text.splitlines()
    first_lines = "\n".join(lines[:8])
    nonempty = [line for line in lines if line.strip()]
    tail = "\n".join(nonempty[-8:])

    if MONIKER not in first_lines:
        errors.append(f"{relative}: Workshop moniker SVG is missing from the document header.")
    if text.count(FOOTER_MARKER) != 1:
        errors.append(f"{relative}: expected exactly one Workshop footer marker.")
    if AVATAR not in tail:
        errors.append(f"{relative}: static Workshop avatar is missing from the document footer.")
    if ".svg" in tail.lower():
        errors.append(f"{relative}: footer must not use an SVG avatar.")
    if "Because state shouldn't be a mess." in tail or "Resources & Support" in tail:
        errors.append(f"{relative}: footer must be the avatar alone; keep prose/links in the document body.")

if errors:
    print("Markdown branding validation failed:")
    for error in errors:
        print(f" - {error}")
    sys.exit(1)

print(f"Markdown branding validated: {len(markdown_files)} files have the moniker header and static avatar-only footer.")
