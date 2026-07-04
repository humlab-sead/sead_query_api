#!/usr/bin/env python3
import re
import csv
import sys
from pathlib import Path
from typing import Any

def write_dot(nodes, edges, output_path="graph.dot"):
    def esc(value):
        return str(value).replace("\\", "\\\\").replace('"', '\\"')

    with open(output_path, "w", encoding="utf-8") as f:
        f.write("digraph ConceptModel {\n")
        f.write('  graph [rankdir="LR"];\n')
        f.write('  node [shape="box", style="rounded"];\n')
        f.write("\n")

        for node in nodes:
            node_id = esc(node["id"])
            label = esc(node.get("label", node["id"]))
            kind = esc(node.get("kind", ""))

            f.write(f'  "{node_id}" [label="{label}\\n({kind})"];\n')

        f.write("\n")

        for edge in edges:
            source = esc(edge["source"])
            target = esc(edge["target"])
            rel = esc(edge["relationship"])

            f.write(f'  "{source}" -> "{target}" [label="{rel}"];\n')

        f.write("}\n")

def main():
        
    if len(sys.argv) != 2:
        print("Usage: python extract_concept_graph.py model.md")
        sys.exit(1)

    text: str = Path(sys.argv[1]).read_text(encoding="utf-8")

    section_re: re.Pattern[str] = re.compile(
        r"^##\s+(Concept|Process):\s*(.+?)\s*$", re.MULTILINE
    )

    matches: list[re.Match[str]] = list(section_re.finditer(text))

    nodes: list[dict[str, str]] = []
    edges: list[dict[str, str]] = []

    for i, match in enumerate(matches):
        kind: str | Any = match.group(1)
        name: str | Any = match.group(2).strip()

        start: int = match.end()
        end: int = matches[i + 1].start() if i + 1 < len(matches) else len(text)
        body: str = text[start:end]

        nodes.append({"id": name, "label": name, "kind": kind})

        rel_match: re.Match[str] | None = re.search(
            r"^###\s+Relationships\s*$([\s\S]*?)(?=^###\s+|\Z)", body, re.MULTILINE
        )

        if not rel_match:
            continue

        rel_block: str = rel_match.group(1)

        for line in rel_block.splitlines():
            line = line.strip()

            if not line.startswith("|"):
                continue
            if re.match(r"^\|\s*-+", line):
                continue
            if "Relationship" in line and ("Concept" in line or "Target" in line):
                continue

            cells: list[str] = [c.strip() for c in line.strip("|").split("|")]

            if len(cells) < 2:
                continue

            relationship: str = cells[0]
            target: str = cells[1]

            if relationship and target:
                edges.append(
                    {"source": name, "relationship": relationship, "target": target}
                )

    with open("nodes.csv", "w", newline="", encoding="utf-8") as f:
        writer = csv.DictWriter(f, fieldnames=["id", "label", "kind"])
        writer.writeheader()
        writer.writerows(nodes)

    with open("edges.csv", "w", newline="", encoding="utf-8") as f:
        writer = csv.DictWriter(f, fieldnames=["source", "relationship", "target"])
        writer.writeheader()
        writer.writerows(edges)

    print(f"Wrote {len(nodes)} nodes to nodes.csv")
    print(f"Wrote {len(edges)} edges to edges.csv")

    write_dot(nodes, edges, output_path="concept_graph.dot")
    print("Wrote graph visualization to concept_graph.dot")
    
if __name__ == "__main__":
    main()
