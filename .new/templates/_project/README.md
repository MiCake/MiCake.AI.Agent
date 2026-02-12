# Project-Specific Templates

This directory contains templates that are auto-generated based on your project's
existing code patterns by the `auto-configure` command.

## How Templates Are Generated

When you run `auto-configure` in Conductor, the system:

1. **Analyzes** your existing codebase for patterns
2. **Extracts** common structures (entities, repositories, services, etc.)
3. **Generates** templates that match your coding conventions

## Template Variables

All templates support these standard variables:

| Variable | Description | Example |
|----------|-------------|---------|
| `${name}` | Class/entity name | `Order` |
| `${name_lower}` | Lowercase name | `order` |
| `${name_plural}` | Pluralized name | `Orders` |
| `${namespace}` | Project namespace | `Company.Domain` |
| `${properties}` | Property definitions | (generated) |
| `${methods}` | Method definitions | (generated) |

## Creating Custom Templates

1. Create a new template file with `.template.{ext}` naming
2. Use the variable syntax above
3. Add template metadata at the top:

```
/**
 * Template: Entity
 * Source: Based on existing Order entity
 * Variables: name, namespace, properties
 */
```

## Template Precedence

1. `_project/` templates (this directory) - highest priority
2. `abstract/` templates - fallback definitions
