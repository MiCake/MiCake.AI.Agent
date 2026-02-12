# Skills System

Skills are modular capability units that agents can invoke to perform specific tasks. Each skill loads only the context it needs, reducing overall context size and improving efficiency.

## Architecture

```
skills/
├── README.md              # This file
├── manifest.yaml          # Skill registry
├── core/                  # Core skills (always available)
│   ├── code-analysis.skill.md
│   ├── template-generation.skill.md
│   ├── requirement-parsing.skill.md
│   └── review-execution.skill.md
├── patterns/              # Pattern-specific skills
│   ├── ddd-modeling.skill.md
│   └── clean-arch-design.skill.md
└── _custom/               # Project-specific skills
    └── README.md
```

## What is a Skill?

A skill is a focused capability that:

1. **Does One Thing Well**: Single responsibility
2. **Loads Minimal Context**: Only what's needed for the task
3. **Is Reusable**: Can be used by multiple agents
4. **Is Composable**: Can be combined with other skills

## Skill vs Agent

| Aspect | Agent | Skill |
|--------|-------|-------|
| Scope | Broad role | Specific task |
| Context | Role-specific | Task-specific |
| Invocation | By Conductor | By any Agent |
| State | Workflow context | Stateless |

## How Skills Work

```
┌─────────────────────────────────────────────────────────────┐
│                        Agent                                 │
│   ┌─────────────────────────────────────────────────────┐   │
│   │  Core Context (minimal, role-specific)               │   │
│   └─────────────────────────────────────────────────────┘   │
│                           │                                  │
│                           ▼                                  │
│   ┌──────────┐  ┌──────────┐  ┌──────────┐                 │
│   │  Skill   │  │  Skill   │  │  Skill   │                 │
│   │ +context │  │ +context │  │ +context │                 │
│   └──────────┘  └──────────┘  └──────────┘                 │
└─────────────────────────────────────────────────────────────┘
```

When an agent needs a capability:
1. Agent invokes the skill
2. Skill loads its specific context
3. Skill performs the task
4. Result returned to agent
5. Skill context is released

## Skill Definition Format

```yaml
skill:
  id: skill-name
  name: "Display Name"
  description: "What this skill does"
  
context:
  required:
    - path/to/required/context
  optional:
    - path/to/optional/context
    
inputs:
  - name: input_name
    type: string
    required: true
    description: "What this input is"
    
outputs:
  - name: output_name
    type: string
    description: "What this output is"
    
usage:
  agents:
    - developer
    - architect
  when: "Description of when to use this skill"
```

## Benefits

1. **Reduced Context Size**: Each agent loads minimal base context
2. **Focused Expertise**: Skills are specialized for specific tasks
3. **Better Reusability**: Same skill used by multiple agents
4. **Easier Maintenance**: Update skill once, all agents benefit
5. **Flexible Composition**: Agents choose skills as needed
