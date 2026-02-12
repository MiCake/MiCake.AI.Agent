# Multi-Agent Collaboration Framework

A technology-agnostic framework for AI-assisted software development using multiple specialized agents.

## Overview

This framework provides a structured approach to AI-assisted development through:

- **Multiple Specialized Agents**: Each agent has a specific role (analyst, architect, developer, reviewer, etc.)
- **Skills System**: Modular capabilities loaded on-demand for efficiency
- **Pattern Packs**: Pre-built knowledge for common patterns (DDD, Clean Architecture, etc.)
- **Workflow Orchestration**: Defined processes for common development scenarios
- **Knowledge Base**: Reusable principles and best practices
- **GitHub Copilot Integration**: Custom instructions for seamless Copilot usage

## Quick Start

### Option 1: Auto-Configure (Recommended)

Simply ask the Conductor agent:

```
@agent auto-configure
```

This will:
1. Scan your project structure
2. Detect language, framework, and patterns
3. Generate all configuration files
4. Optionally create project-specific templates

### Option 2: Manual Configuration

Edit `manifest.yaml` to set your patterns:

```yaml
patterns:
  active: "ddd"     # or clean-architecture, etc.
```

Then update project context in `context/project-structure.yaml`.

---

## Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                    manifest.yaml (System Root)                   │
├─────────────────────────────────────────────────────────────────┤
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────────────┐  │
│  │   Skills     │  │   Patterns   │  │      Workflows       │  │
│  │  On-demand   │  │  DDD, Clean  │  │  Req→Code, Review   │  │
│  │  Modules     │  │  Arch...     │  │                      │  │
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
│  │  /Project    │  │  _project/   │  │                      │  │
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
├── .github/                   # GitHub integration
│   └── copilot-instructions.md  # Copilot custom instructions
│
├── config/                    # Configuration files
│   ├── system.yaml           # System behavior settings
│   └── preferences.yaml      # User preferences
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
├── skills/                    # Modular capabilities
│   ├── manifest.yaml         # Skill registry
│   ├── core/                 # Core skills
│   │   ├── code-analysis.skill.md
│   │   ├── template-generation.skill.md
│   │   ├── requirement-parsing.skill.md
│   │   ├── review-execution.skill.md
│   │   ├── test-generation.skill.md
│   │   └── project-detection.skill.md
│   ├── patterns/             # Pattern-specific skills
│   │   └── ddd-modeling.skill.md
│   └── _custom/              # Custom skills
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
│   ├── _project/             # Project-specific templates (auto-generated)
│   ├── _custom/              # Custom templates
│   └── abstract/             # Language-agnostic templates
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

### Auto-Configuration (Recommended)

Let the framework detect your project settings:

```
@agent auto-configure
```

This scans your project and:
- Detects language and framework
- Identifies architecture patterns
- Generates configuration files
- Optionally creates project-specific templates

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

### System Behavior

Configure system-level behavior:

```yaml
# config/system.yaml
interaction:
  confirm_before_generate: true
  confirm_before_handoff: true

output:
  no_emojis: true
  avoid_prose: true
```

---

## Skills System

Skills are modular capabilities that agents invoke on-demand, minimizing context loading overhead.

### Core Skills

| Skill | Purpose | Used By |
|-------|---------|---------|
| `code-analysis` | Analyze code structure | Developer, Reviewer, Architect |
| `template-generation` | Generate code from templates | Developer |
| `requirement-parsing` | Parse requirements documents | Analyst |
| `review-execution` | Execute code review | Reviewer |
| `test-generation` | Generate test code | Tester |
| `project-detection` | Auto-detect project structure | Conductor |

### Pattern Skills

| Skill | Purpose | Used By |
|-------|---------|---------|
| `ddd-modeling` | Domain modeling | Architect, Developer |

### Adding Custom Skills

1. Create `skills/_custom/{skill-name}.skill.md`
2. Define context loading, capabilities, inputs/outputs
3. Register in `skills/manifest.yaml`

---

## GitHub Copilot Integration

The framework integrates with GitHub Copilot through custom instructions.

### Quick Commands

| Command | Description |
|---------|-------------|
| `@agent start` | Get started |
| `@agent auto-configure` | Auto-detect and configure |
| `@agent dispatch` | Route task to agents |
| `@agent analyze` | Analyze requirements |
| `@agent design` | Create architecture |
| `@agent implement` | Build features |
| `@agent review` | Review code |
| `@agent test` | Run tests |
| `@agent consult` | Get advice |

See `.github/copilot-instructions.md` for full documentation.

---

## Extending the Framework

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

### Adding Project Templates

Use `auto-configure` to generate templates automatically, or:

1. Create templates in `templates/_project/`
2. Use template variables (see `templates/_project/README.md`)
3. Templates will be used by `template-generation` skill

---

## Migration from Original MiCake.AI.Agent

If migrating from the original MiCake-specific implementation:

1. Your agents map to generalized agents:
   - Sage → Analyst
   - Inspector → Reviewer
   - QA → Tester
   - Consultant → Advisor

2. DDD knowledge is now in `knowledge/patterns/ddd/`

3. Language-specific settings are auto-detected via `auto-configure`

4. Use `@agent auto-configure` to set up the framework for your project

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
