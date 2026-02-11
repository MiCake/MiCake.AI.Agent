# Multi-Agent Collaboration Framework

A technology-agnostic framework for AI-assisted software development using multiple specialized agents.

## Overview

This framework provides a structured approach to AI-assisted development through:

- **Multiple Specialized Agents**: Each agent has a specific role (analyst, architect, developer, reviewer, etc.)
- **Configurable Adapters**: Support for different languages and frameworks (.NET, Java, Python, etc.)
- **Pattern Packs**: Pre-built knowledge for common patterns (DDD, Clean Architecture, etc.)
- **Workflow Orchestration**: Defined processes for common development scenarios
- **Knowledge Base**: Reusable principles and best practices

## Quick Start

### 1. Configure Your Project

Edit `manifest.yaml` to set your adapter and patterns:

```yaml
adapters:
  active: "dotnet"  # or java, python, etc.
  
patterns:
  active: "ddd"     # or clean-architecture, etc.
```

### 2. Update Project Context

Edit `context/project-structure.yaml`:

```yaml
project:
  name: "Your Project"
  root: "/path/to/project"
```

### 3. Create a Requirement

Create `requirements/your-feature.md` using the template.

### 4. Start the Workflow

Invoke the Conductor agent to process your requirement.

---

## Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                    manifest.yaml (System Root)                   │
├─────────────────────────────────────────────────────────────────┤
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────────────┐  │
│  │   Adapters   │  │   Patterns   │  │      Workflows       │  │
│  │  .NET, Java  │  │  DDD, Clean  │  │  Req→Code, Review   │  │
│  │  Python...   │  │  Arch...     │  │                      │  │
│  └──────────────┘  └──────────────┘  └──────────────────────┘  │
├─────────────────────────────────────────────────────────────────┤
│  ┌──────────────────────────────────────────────────────────┐  │
│  │                       Agents                              │  │
│  │  Conductor | Analyst | Architect | Developer | Reviewer  │  │
│  │            | Tester  | Advisor                           │  │
│  └──────────────────────────────────────────────────────────┘  │
├─────────────────────────────────────────────────────────────────┤
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────────────┐  │
│  │  Knowledge   │  │  Templates   │  │      Context         │  │
│  │  Core/Patt.  │  │  Abstract/   │  │  Project, Arch       │  │
│  │  /Project    │  │  Lang-spec   │  │                      │  │
│  └──────────────┘  └──────────────┘  └──────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

---

## Directory Structure

```
.new/
├── manifest.yaml              # System configuration root
├── README.md                  # This file
│
├── config/                    # Configuration files
│   ├── system.yaml           # System defaults
│   ├── preferences.yaml      # User preferences
│   └── adapters/             # Language/framework adapters
│       ├── dotnet.yaml
│       ├── java.yaml
│       └── python.yaml
│
├── agents/                    # Agent definitions
│   ├── conductor.agent.md    # Orchestration
│   ├── analyst.agent.md      # Requirements analysis
│   ├── architect.agent.md    # Technical design
│   ├── developer.agent.md    # Implementation
│   ├── reviewer.agent.md     # Code review
│   ├── tester.agent.md       # Quality assurance
│   ├── advisor.agent.md      # Technical consultation
│   └── _custom/              # Custom agents
│
├── knowledge/                 # Knowledge base
│   ├── core/                 # Universal principles
│   │   ├── software-principles.md
│   │   ├── code-quality.md
│   │   └── review-checklist.md
│   ├── patterns/             # Pattern-specific knowledge
│   │   ├── ddd/
│   │   └── clean-architecture/
│   └── project/              # Project-specific knowledge
│
├── templates/                 # Code generation templates
│   ├── _engine/              # Template engine config
│   ├── abstract/             # Language-agnostic templates
│   ├── dotnet/               # .NET templates
│   ├── java/                 # Java templates
│   └── python/               # Python templates
│
├── workflows/                 # Workflow definitions
│   ├── core/
│   │   ├── requirement-to-code.workflow.md
│   │   ├── requirement-change.workflow.md
│   │   └── code-review.workflow.md
│   └── _custom/              # Custom workflows
│
├── context/                   # Project context
│   ├── project-structure.yaml
│   └── architecture-model.yaml
│
├── changes/                   # Change tracking
│   ├── CHANGELOG.md
│   └── {change-id}/          # Per-change artifacts
│
└── requirements/              # Requirement documents
    └── _templates/           # PRD templates
```

---

## Agents

| Agent | Role | Responsibilities |
|-------|------|------------------|
| **Conductor** | Orchestration | Coordinate workflows, manage execution |
| **Analyst** | Requirements | Analyze PRDs, extract domain concepts |
| **Architect** | Design | Technical design, pattern application |
| **Developer** | Implementation | Write code following designs |
| **Reviewer** | Quality | Review code for standards compliance |
| **Tester** | Testing | Verify functionality, write tests |
| **Advisor** | Consultation | Provide technical guidance |

---

## Workflows

### Requirement to Code

Complete flow from PRD to working code:

```
PRD → Analysis → Design → Implementation → Review → Testing → Done
```

### Requirement Change

Modify existing code when requirements change:

```
Change Request → Impact Analysis → Design → Implementation → Regression Review → Done
```

### Code Review

Standalone code review:

```
Code → Static Analysis → Design Review → Logic Review → Report
```

---

## Configuration

### Selecting an Adapter

Adapters configure language-specific settings:

```yaml
# manifest.yaml
adapters:
  active: "dotnet"
```

Available adapters: `dotnet`, `java`, `python` (add more in `config/adapters/`)

### Selecting a Pattern

Patterns provide architectural guidance:

```yaml
# manifest.yaml
patterns:
  active: "ddd"
```

Available patterns: `ddd`, `clean-architecture` (add more in `knowledge/patterns/`)

### User Preferences

Configure user-specific settings:

```yaml
# config/preferences.yaml
language: "en"  # en, zh
code_comments: true
explanation_detail: "standard"
```

---

## Extending the Framework

### Adding a New Adapter

1. Create `config/adapters/{adapter-name}.yaml`
2. Define terminology, conventions, types
3. Add language templates in `templates/{adapter-name}/`
4. Register in `manifest.yaml`

### Adding a New Pattern

1. Create `knowledge/patterns/{pattern-name}/`
2. Add `manifest.yaml`, overview, and checklists
3. Register in `manifest.yaml`

### Adding a Custom Agent

1. Create `agents/_custom/{agent-name}.agent.md`
2. Follow agent definition format
3. Register in `manifest.yaml`

### Adding a Custom Workflow

1. Create `workflows/_custom/{workflow-name}.workflow.md`
2. Define phases and transitions
3. Register in `manifest.yaml`

---

## Migration from Original MiCake.AI.Agent

If migrating from the original MiCake-specific implementation:

1. Your agents map to generalized agents:
   - Sage → Analyst
   - Inspector → Reviewer
   - QA → Tester
   - Consultant → Advisor

2. DDD knowledge is now in `knowledge/patterns/ddd/`

3. .NET specifics are now in `config/adapters/dotnet.yaml`

4. Update `manifest.yaml` with your configuration

---

## Best Practices

1. **Keep Context Updated**: Stale context leads to wrong decisions
2. **Use Appropriate Detail Level**: Match complexity to task size
3. **Trust the Workflow**: Let agents coordinate through defined phases
4. **Review Generated Code**: Verify output matches intent
5. **Iterate When Needed**: Use feedback loops in workflows

---

## License

[Specify your license]

---

## Contributing

[Contribution guidelines if open source]
