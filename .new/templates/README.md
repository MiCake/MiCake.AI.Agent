# Template System

This directory contains code generation templates that agents use to create consistent code.

## Architecture

```
templates/
├── _engine/              # Template engine configuration
│   └── variables.yaml    # Variable definitions and placeholders
├── abstract/             # Language-agnostic template definitions
│   ├── entity.template.md
│   ├── value-object.template.md
│   ├── repository.template.md
│   └── service.template.md
├── dotnet/               # .NET/C# specific templates
├── java/                 # Java specific templates
├── python/               # Python specific templates
└── _custom/              # Custom project templates
```

## How Templates Work

### 1. Abstract Templates

Define the **structure and intent** without language specifics:

```markdown
# Entity Template

## Purpose
Defines a domain entity with identity and behavior.

## Structure
- Identity field (configured by adapter)
- Properties with encapsulation
- Business methods
- Invariant validation

## Required Elements
- Constructor with validation
- Business behavior methods
- Domain events (if needed)
```

### 2. Language Templates

Implement abstract templates in specific languages:

```csharp
// dotnet/entity.template.cs
public class ${EntityName} : Entity<${IdType}>
{
    public ${EntityName}(${ConstructorParams})
    {
        // Initialize and validate
    }
    
    ${Properties}
    
    ${Methods}
}
```

### 3. Variable Substitution

Variables from adapter and pattern configurations:

| Variable | Source | Example |
|----------|--------|---------|
| `${EntityName}` | Input | `Order` |
| `${IdType}` | Adapter | `Guid` (.NET), `UUID` (Java) |
| `${namespace}` | Adapter | `Company.Product.Domain` |
| `${base_class}` | Pattern | `Entity<TId>` |

## Adding Custom Templates

1. Create language-specific file in appropriate directory
2. Use standard placeholders
3. Register in adapter's `templates` section
4. Optionally create abstract definition first

## Template Selection

Templates are selected based on:

1. **Pattern** (DDD, Clean Architecture, etc.)
2. **Language** (from adapter)
3. **Component type** (entity, repository, etc.)

Selection flow:

```
Pattern requested (e.g., "entity")
    ↓
Check language-specific: templates/dotnet/entity.template.cs
    ↓
If not found, use abstract: templates/abstract/entity.template.md
    ↓
Apply adapter variables
    ↓
Generate code
```
