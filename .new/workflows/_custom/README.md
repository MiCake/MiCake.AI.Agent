# Custom Workflows

This directory is for project-specific workflows that extend or modify the core workflows.

## Creating a Custom Workflow

### 1. Copy a Core Workflow as Template

```powershell
Copy-Item workflows/core/requirement-to-code.workflow.md workflows/_custom/my-workflow.workflow.md
```

### 2. Modify for Your Needs

- Change phases
- Add/remove agents
- Modify transitions
- Adjust quality gates

### 3. Register in Manifest

In `manifest.yaml`:

```yaml
workflows:
  custom:
    - id: "my-workflow"
      path: "workflows/_custom/my-workflow.workflow.md"
```

## Example: Hotfix Workflow

```yaml
workflow:
  id: hotfix
  name: "Hotfix"
  description: "Emergency fix workflow with expedited review"

phases:
  - id: analysis
    agent: conductor
    skip_on: trivial_change
    
  - id: implementation
    agent: developer
    
  - id: quick-review
    agent: reviewer
    config:
      review_type: quick
      focus: [security, correctness]
      
  - id: testing
    agent: tester
    config:
      require_unit_tests: true
      require_integration_tests: false
```

## Example: Documentation-Only Workflow

```yaml
workflow:
  id: docs-update
  name: "Documentation Update"
  description: "Workflow for documentation-only changes"

phases:
  - id: content-review
    agent: reviewer
    config:
      focus: [accuracy, clarity, completeness]
      
  - id: approval
    agent: conductor
```

## Workflow Composition

You can compose workflows by referencing phases from core workflows:

```yaml
workflow:
  id: simplified-implementation
  
phases:
  - import: "core/requirement-to-code#analysis"
  - import: "core/requirement-to-code#implementation"
  - id: custom-review
    agent: reviewer
    config:
      # custom configuration
```

## Best Practices

1. **Start from Core**: Base custom workflows on core ones
2. **Document Changes**: Explain why customization needed
3. **Test Thoroughly**: Verify workflow works as expected
4. **Version Control**: Track workflow changes
5. **Keep It Simple**: Only customize what's necessary
