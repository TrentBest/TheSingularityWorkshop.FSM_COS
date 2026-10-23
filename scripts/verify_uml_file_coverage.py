#!/usr/bin/env python3
"""Check file-level UML companion coverage for FSM_COS source files.

This is a deterministic file-coverage guard, not a C# declaration parser. It does
not claim nested/multiple declaration coverage or validate diagram semantics.
"""
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
SOURCE_ROOT = ROOT / "src" / "FSM_COS"
COMPANION_ROOT = ROOT / "docs" / "UML" / "src" / "FSM_COS"
DIAGRAM_ROOT = ROOT / "docs" / "UML" / "ClassDiagram"
EXCLUDED_PARTS = {"bin", "obj", ".git"}

sources = sorted(
    path for path in SOURCE_ROOT.rglob("*.cs")
    if not any(part in EXCLUDED_PARTS for part in path.relative_to(ROOT).parts)
)
errors = []
for source in sources:
    name = source.stem
    companion = COMPANION_ROOT / f"{name}.md"
    uml_candidates = (
        COMPANION_ROOT / f"{name}UML.md",
        DIAGRAM_ROOT / f"{name}UML.md",
    )
    if not companion.is_file():
        errors.append(f"{source.relative_to(ROOT).as_posix()}: missing companion {companion.relative_to(ROOT).as_posix()}")
    if not any(candidate.is_file() for candidate in uml_candidates):
        expected = " or ".join(candidate.relative_to(ROOT).as_posix() for candidate in uml_candidates)
        errors.append(f"{source.relative_to(ROOT).as_posix()}: missing UML neighborhood companion ({expected})")

if errors:
    print("FSM_COS UML file-coverage check failed:")
    for error in errors:
        print(f" - {error}")
    print("This check covers source files only; compiler-backed declaration-level coverage remains separate.")
    sys.exit(1)

print(f"FSM_COS UML file coverage passed: {len(sources)} C# source files have companion and UML-neighborhood pages.")
print("Scope limitation: this checks file-level presence only, not declaration identity, source spans, semantic relationships, or freshness.")
