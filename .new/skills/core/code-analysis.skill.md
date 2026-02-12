# Code Analysis Skill

Analyze code structure, quality, patterns, and provide insights.

---

## Skill Definition

```yaml
skill:
  id: code-analysis
  name: "Code Analysis"
  version: "1.0"
  description: "Analyze code structure, quality, and patterns"
```

---

## Context Loading

```yaml
context:
  required:
    - context/project-structure.yaml       # Project conventions (detected)
  optional:
    - knowledge/core/code-quality.md       # Quality standards (if reviewing)
    - knowledge/patterns/${pattern}/*.md   # Pattern knowledge (if checking compliance)
  never:
    - requirements/*                        # Not needed for code analysis
    - changes/*                             # Historical changes not needed
```

**Context Size**: ~2-5KB (minimal)

---

## Capabilities

### 1. Structure Analysis

Understand code organization:
- Module/namespace structure
- Class hierarchies
- Dependency relationships
- File organization

### 2. Pattern Detection

Identify patterns in use:
- Design patterns (Repository, Factory, etc.)
- Architectural patterns (Layered, DDD, etc.)
- Anti-patterns

### 3. Quality Assessment

Evaluate code quality:
- Complexity metrics
- Coupling/cohesion
- Code smells
- Best practice adherence

### 4. Dependency Analysis

Map dependencies:
- Internal dependencies
- External packages
- Circular dependencies
- Unused dependencies

---

## Inputs

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `target` | string | Yes | File path, folder, or code snippet to analyze |
| `analysis_type` | enum | No | `structure`, `quality`, `patterns`, `dependencies`, `all` |
| `depth` | enum | No | `surface`, `moderate`, `deep` |

---

## Outputs

| Name | Type | Description |
|------|------|-------------|
| `structure` | object | Code structure analysis |
| `patterns_found` | array | Detected patterns |
| `quality_score` | number | Quality assessment (0-100) |
| `issues` | array | Identified issues |
| `recommendations` | array | Improvement suggestions |

---

## Usage

### By Developer

```
Invoke skill: code-analysis
Target: src/Domain/Entities/Order.cs
Type: structure
```

Use when:
- Understanding existing code before modification
- Checking implementation against design
- Identifying areas for refactoring

### By Reviewer

```
Invoke skill: code-analysis
Target: [changed files]
Type: quality
Depth: deep
```

Use when:
- Reviewing code changes
- Checking pattern compliance
- Assessing code quality

### By Architect

```
Invoke skill: code-analysis
Target: src/
Type: dependencies
```

Use when:
- Analyzing module dependencies
- Planning refactoring
- Evaluating architecture compliance

---

## Example Output

```yaml
analysis:
  target: "src/Domain/Entities/Order.cs"
  
  structure:
    type: "class"
    name: "Order"
    namespace: "Company.Domain.Entities"
    inherits: "Entity<Guid>"
    implements: ["IAggregateRoot"]
    properties: 5
    methods: 8
    
  patterns_found:
    - name: "Aggregate Root"
      confidence: "high"
      evidence: "Implements IAggregateRoot, has identity"
    - name: "Rich Domain Model"
      confidence: "medium"
      evidence: "Contains business methods"
      
  quality_score: 85
  
  issues:
    - severity: "minor"
      type: "code_smell"
      message: "Method 'ProcessOrder' exceeds 20 lines"
      line: 45
      
  recommendations:
    - "Consider extracting order validation to a specification"
    - "Add XML documentation for public methods"
```

---

## Integration

This skill is used by:
- **Developer**: Before and during implementation
- **Reviewer**: During code review
- **Architect**: For architectural analysis
- **Tester**: To understand code for test design
