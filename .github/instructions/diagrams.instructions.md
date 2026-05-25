---
description: "Use when creating or editing Mermaid diagrams anywhere in the repository — style, layout, classDef conventions, and diagram type selection."
---
## Mermaid diagram style guidance

When creating or editing Mermaid diagrams, prefer diagrams that are clean, polished, and easy to scan.

Guidelines:

- Use short, meaningful node and state labels.
- Prefer clear layout direction, usually `direction LR` for flows and state machines.
- Avoid overcrowding nodes with long descriptions.
- Move details into `note` blocks when using state diagrams.
- Use consistent naming and casing across nodes.
- Use concise transition labels such as `Job succeeds`, `TTL elapsed`, or `Download requested`.
- Group related concepts when possible.
- Add tasteful `classDef` styling to distinguish states or categories.
- Use soft, muted colors rather than saturated colors.
- Use consistent stroke colors and avoid visual noise.
- Keep diagrams readable in both light and dark themes when possible.
- Prefer semantic Mermaid diagram types, e.g. `stateDiagram-v2` for state machines and `flowchart` for process flows.
- Avoid Mermaid features that are poorly supported across renderers.
- Do not use overly decorative styling at the cost of clarity.

For state diagrams:

- Keep states visually compact.
- Use notes for UI behavior, API details, or implementation notes.
- Use simple state names like `Pending`, `Ready`, `Failed`, and `Expired`.
- Use terminal states intentionally.
- Make error and expiry paths visually distinct but not alarming.

Example style pattern:

```mermaid
stateDiagram-v2
    direction LR

    [*] --> Pending : Create ticket
    Pending --> Ready : Job succeeds
    Pending --> Failed : Job fails
    Ready --> Expired : TTL elapsed
    Failed --> Expired : TTL elapsed

    note right of Pending
        Preparing archive
        Auto-refresh enabled
    end note

    classDef pending fill:#fff7d6,stroke:#d6a300,color:#2b2b2b;
    classDef ready fill:#dff7e8,stroke:#2e9f5b,color:#1d3a29;
    classDef failed fill:#ffe0e0,stroke:#d64545,color:#4a1f1f;
    classDef expired fill:#eeeeee,stroke:#888888,color:#333333;

    class Pending pending;
    class Ready ready;
    class Failed failed;
    class Expired expired;
  