# Workflows

This directory contains workflow definitions that orchestrate multi-agent collaboration.

## Architecture

```
workflows/
├── README.md              # This file
├── core/                  # Core workflow definitions
│   ├── requirement-to-code.workflow.md
│   ├── requirement-change.workflow.md
│   └── code-review.workflow.md
└── _custom/               # Project-specific workflows
    └── README.md
```

## What is a Workflow?

A workflow defines:

1. **Trigger**: When the workflow starts
2. **Phases**: Sequential stages of work
3. **Agents**: Which agents participate in each phase
4. **Transitions**: How work flows between agents
5. **Artifacts**: Inputs and outputs at each phase
6. **Completion**: When the workflow ends

## Core Workflows

| Workflow | Purpose | Key Agents |
|----------|---------|------------|
| Requirement to Code | Implement new features from PRD | Conductor → Analyst → Architect → Developer → Reviewer → Tester |
| Requirement Change | Modify existing implementation | Conductor → Analyst → Architect → Developer → Reviewer |
| Code Review | Review and improve code quality | Conductor → Reviewer → Developer |

## Workflow Execution Model

```
┌─────────────────────────────────────────────────────────────┐
│                      Conductor                               │
│                   (Orchestration)                            │
└─────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│  Phase 1: Analysis                                           │
│  ┌─────────────┐                                            │
│  │   Analyst   │ → Output: Analyzed requirements            │
│  └─────────────┘                                            │
└─────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│  Phase 2: Design                                             │
│  ┌─────────────┐                                            │
│  │  Architect  │ → Output: Technical design                 │
│  └─────────────┘                                            │
└─────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│  Phase 3: Implementation                                     │
│  ┌─────────────┐                                            │
│  │  Developer  │ → Output: Code implementation              │
│  └─────────────┘                                            │
└─────────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│  Phase 4: Quality                                            │
│  ┌────────────┐    ┌────────────┐                           │
│  │  Reviewer  │    │   Tester   │ → Output: Quality report  │
│  └────────────┘    └────────────┘                           │
└─────────────────────────────────────────────────────────────┘
```

## Workflow Definition Format

```yaml
workflow:
  id: workflow-name
  name: "Display Name"
  description: "What this workflow does"
  
triggers:
  - type: user_request
    pattern: "implement|create|build"
  
phases:
  - id: phase-1
    name: "Phase Name"
    agent: agent-name
    input:
      - artifact-name
    output:
      - artifact-name
    transitions:
      success: phase-2
      failure: handle-failure
      
completion:
  conditions:
    - all_phases_complete
    - quality_check_passed
  artifacts:
    - final-output
```

## Creating Custom Workflows

1. Create file in `workflows/_custom/`
2. Follow the workflow definition format
3. Register in `manifest.yaml` under `workflows.custom`
4. Reference existing agents or define custom ones

## Workflow Variables

Workflows can reference variables from:

- **Adapter**: `${adapter.*}` - Language/framework specifics
- **Pattern**: `${pattern.*}` - Architecture pattern specifics  
- **Project**: `${project.*}` - Project configuration
- **Input**: `${input.*}` - User-provided input

## Best Practices

1. **Single Responsibility**: Each phase has one clear goal
2. **Clear Handoffs**: Explicit artifacts between phases
3. **Quality Gates**: Include review phases
4. **Failure Handling**: Define what happens on errors
5. **Documentation**: Document expected inputs/outputs
