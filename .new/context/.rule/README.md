# Context Directory Rules

This document defines rules for maintaining the context directory.

## Rule 1: Keep Context Current

**What**: Context files must reflect the current state of the project.

**Why**: Stale context leads to incorrect decisions.

**How**: Update context files after any significant change.

## Rule 2: Use Standard Formats

**What**: All context files use YAML format with defined schema.

**Why**: Consistency enables automated processing.

**How**: Follow the templates provided.

## Rule 3: Document Changes

**What**: Major context updates should include a reason.

**Why**: Helps understand why context evolved.

**How**: Add comments in YAML files.

## Rule 4: Validate Before Commit

**What**: Context files should be valid YAML.

**Why**: Invalid context breaks agent workflows.

**How**: Use YAML linter before committing.

## Rule 5: Minimal and Relevant

**What**: Only include information agents need.

**Why**: Less noise means better decisions.

**How**: Remove outdated or irrelevant entries.

---

## File Schemas

### project-structure.yaml

```yaml
# Required
project:
  name: string
  root: string
  
# Required
modules:
  - name: string
    path: string
    type: enum[domain, application, infrastructure, presentation]
    
# Optional
dependencies:
  internal: [module references]
  external: [package names]
```

### architecture-model.yaml

```yaml
# Required
architecture:
  style: enum[layered, clean, hexagonal, microservices, monolith]
  pattern: enum[ddd, cqrs, crud, etc]
  
# Required
layers:
  - name: string
    responsibilities: [string]
    
# Optional
decisions:
  - id: string
    date: date
    decision: string
    rationale: string
```

### tech-stack.yaml

```yaml
# Required
language:
  name: string
  version: string
  
# Required
framework:
  name: string
  version: string
  
# Optional
dependencies:
  - name: string
    version: string
    purpose: string
    
tools:
  - name: string
    purpose: string
```

### conventions.yaml

```yaml
# Optional - extends adapter defaults
naming:
  entities: pattern
  repositories: pattern
  services: pattern
  
formatting:
  line_length: number
  indentation: number
  
documentation:
  required_for: [public, all]
  style: enum[xml-doc, jsdoc, docstring]
```
