import json
from pathlib import Path
from graphify.build import build_from_json
from graphify.wiki import to_wiki

out = Path("graphify-out")

extraction = json.loads((out / ".graphify_ast.json").read_text())
analysis = json.loads((out / ".graphify_analysis.json").read_text())

labels_path = out / ".graphify_labels.json"
labels_raw = json.loads(labels_path.read_text()) if labels_path.exists() else {}

G = build_from_json(extraction)

communities = {int(k): v for k, v in analysis["communities"].items()}
cohesion = {int(k): v for k, v in analysis.get("cohesion", {}).items()}
labels = {int(k): v for k, v in labels_raw.items()}

n = to_wiki(
    G,
    communities,
    out / "wiki",
    community_labels=labels or None,
    cohesion=cohesion,
    god_nodes_data=analysis.get("gods", []),
)

print(f"Wiki: {n} articles in {out / 'wiki'}")