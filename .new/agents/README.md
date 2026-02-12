# AI Agent System

Multi-agent collaboration framework for software development.

## Overview

This AI Agent System provides an intelligent development assistant that helps developers build high-quality software through specialized AI agents working together.

## Features

- **Smart Guidance** - Complete development chain from requirements to code
- **Architecture Design** - System modeling and module planning
- **Code Generation** - Automatic generation of framework-compliant code
- **Code Review** - Automated review based on best practices
- **Extensible Knowledge Base** - Built-in patterns with custom extensions

## Quick Start

1. Ensure `.ai-agents/` folder exists in your project
2. Configure `manifest.yaml` for your technology stack
3. Select **Conductor** agent in your AI assistant to get started
4. Run `setup` to configure your preferences
5. Use `services` to see all available agents

## Commands

| Command | Description | Agent |
|---------|-------------|-------|
| `start` | Welcome and introduction | Conductor |
| `setup` | Configure preferences | Conductor |
| `init-project` | Initialize project structure | Conductor |
| `services` | List all available agents | Conductor |
| `change-request` | Start requirement change workflow | Conductor |
| `change-status` | View current change status | Conductor |
| `sync-context` | Refresh project context | Conductor |
| `analyze-requirements` | Analyze PRD/User Stories | Analyst |
| `system-design` | Design system architecture | Architect |
| `implement` | Implement feature from design | Developer |
| `review` | Code review | Reviewer |
| `validate` | Validate against requirements | Tester |
| `consult` | Get expert advice | Advisor |

## Agents

| Agent | Role |
|-------|------|
| [Conductor](./conductor.agent.md) | Main coordinator, setup guide, agent routing |
| [Analyst](./analyst.agent.md) | Requirements analysis, concept extraction |
| [Architect](./architect.agent.md) | Architecture design, system modeling |
| [Developer](./developer.agent.md) | Code implementation, bug fixing |
| [Reviewer](./reviewer.agent.md) | Code review, quality assurance |
| [Tester](./tester.agent.md) | Requirements validation, testing |
| [Advisor](./advisor.agent.md) | Framework guidance, best practices |

## Directory Structure

```
.ai-agents/
├── manifest.yaml                    # System manifest
├── README.md                        # This file
│
├── agents/                          # Agent definitions
│   ├── conductor.agent.md
│   ├── analyst.agent.md
│   ├── architect.agent.md
│   ├── developer.agent.md
│   ├── reviewer.agent.md
│   ├── tester.agent.md
│   ├── advisor.agent.md
│   └── _custom/                     # Custom agents
│
├── config/                          # Configuration
│   ├── system.yaml                  # System behavior settings
│   └── preferences.yaml             # User preferences
│
├── skills/                          # Modular capabilities
│   ├── manifest.yaml                # Skill registry
│   ├── core/                        # Core skills
│   ├── patterns/                    # Pattern-specific skills
│   └── _custom/                     # Custom skills
│
├── knowledge/                       # Knowledge base
│   ├── core/                        # Core knowledge
│   ├── patterns/                    # Architecture patterns
│   │   ├── ddd/
│   │   ├── clean-architecture/
│   │   └── ...
│   └── project/                     # Project-specific
│
├── templates/                       # Code templates
│   ├── abstract/                    # Abstract definitions
│   ├── _project/                    # Project-specific (auto-generated)
│   └── _custom/                     # Custom templates
│
├── workflows/                       # Workflow definitions
│   ├── core/
│   │   ├── requirement-to-code.workflow.md
│   │   ├── requirement-change.workflow.md
│   │   └── code-review.workflow.md
│   └── _custom/
│
├── context/                         # Project context
│   ├── project-structure.yaml
│   └── architecture-model.yaml
│
├── changes/                         # Change management
│   ├── pending/
│   ├── in-progress/
│   └── completed/
│
└── requirements/                    # Requirements documents
```

## Configuration

### Auto-Configuration (Recommended)

Use the Conductor's auto-configure command to automatically set up your project:

```
@agent auto-configure
```

### Selecting Architecture Pattern

Edit `manifest.yaml`:

```yaml
patterns:
  active: "ddd"  # or "clean-architecture", "hexagonal", etc.
```

## Extending the System

### Adding Custom Agents

Create a new file in `agents/_custom/`:

```markdown
# My Custom Agent

## Metadata
- **ID**: custom-myagent
- **Name**: My Agent
...
```

### Adding Custom Knowledge

Add files to `knowledge/project/`:
- Team conventions
- Project-specific patterns
- Domain terminology

### Adding Custom Workflows

Create files in `workflows/extensions/` following the workflow template.

## License

MIT License
