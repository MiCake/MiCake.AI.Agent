# Custom Skills

This directory is for project-specific skills that extend the core skill set.

## Creating a Custom Skill

### 1. Create Skill File

Create a new `.skill.md` file:

```markdown
# My Custom Skill

## Skill Definition

\`\`\`yaml
skill:
  id: my-custom-skill
  name: "My Custom Skill"
  version: "1.0"
  description: "What this skill does"
\`\`\`

## Context Loading

\`\`\`yaml
context:
  required:
    - path/to/required/context
  optional:
    - path/to/optional/context
  never:
    - path/to/excluded/context
\`\`\`

## Capabilities
...

## Inputs
...

## Outputs
...

## Usage
...
```

### 2. Register the Skill

Add to `skills/manifest.yaml`:

```yaml
custom:
  - id: "my-custom-skill"
    path: "skills/_custom/my-custom-skill.skill.md"
    description: "What this skill does"
```

### 3. Assign to Agents

Update agent_skills in manifest:

```yaml
agent_skills:
  developer:
    optional:
      - my-custom-skill
```

## Example: Database Migration Skill

```markdown
# Database Migration Skill

## Skill Definition

\`\`\`yaml
skill:
  id: db-migration
  name: "Database Migration"
  version: "1.0"
  description: "Generate and manage database migrations"
\`\`\`

## Context Loading

\`\`\`yaml
context:
  required:
    - context/project-structure.yaml
  optional: []
  never:
    - requirements/*
\`\`\`

## Capabilities

### 1. Migration Generation
Generate migration files from model changes.

### 2. Migration Script Preview
Preview SQL that will be executed.

## Inputs

| Name | Type | Required | Description |
|------|------|----------|-------------|
| target_entity | string | Yes | Entity to migrate |
| migration_name | string | Yes | Name for migration |

## Outputs

| Name | Type | Description |
|------|------|-------------|
| migration_file | string | Generated migration file |
| sql_preview | string | Preview of SQL commands |
```

## Best Practices

1. **Minimal Context**: Only load what you need
2. **Single Purpose**: One skill, one responsibility
3. **Clear Interface**: Well-defined inputs/outputs
4. **Reusable**: Design for multiple agents
5. **Documented**: Include usage examples
