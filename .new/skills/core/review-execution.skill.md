# Review Execution Skill

Execute code review against standards and checklists.

---

## Skill Definition

```yaml
skill:
  id: review-execution
  name: "Review Execution"
  version: "1.0"
  description: "Execute code review against standards"
```

---

## Context Loading

```yaml
context:
  required:
    - context/project-structure.yaml               # Project conventions
    - knowledge/core/review-checklist.md           # Universal review checklist
  optional:
    - knowledge/patterns/${pattern}/review-checklist.md  # Pattern-specific checklist
    - config/preferences.yaml                       # User preferences
  never:
    - requirements/*                                # Not needed for review
    - templates/*                                   # Not needed for review
    - workflows/*                                   # Workflow definitions not needed
```

**Context Size**: ~3-6KB (review-focused)

---

## Capabilities

### 1. Checklist-Based Review

Apply systematic checks:
- Formatting compliance
- Naming conventions
- Code quality standards
- Pattern compliance

### 2. Issue Categorization

Categorize findings:
- Critical / Major / Minor / Suggestion
- Security / Performance / Maintainability
- Auto-fixable / Manual fix required

### 3. Report Generation

Generate review reports:
- Summary statistics
- Detailed findings
- Remediation guidance

### 4. Comparative Review

Compare against:
- Original requirements
- Design documents
- Previous versions

---

## Inputs

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `target` | string/array | Yes | File(s) to review |
| `review_type` | enum | No | `quick`, `standard`, `deep`, `security` |
| `design_doc` | string | No | Path to design document for comparison |
| `focus_areas` | array | No | Specific areas to focus on |

---

## Outputs

| Name | Type | Description |
|------|------|-------------|
| `status` | enum | `approved`, `approved_with_comments`, `changes_requested`, `rejected` |
| `summary` | object | Issue counts by severity |
| `issues` | array | Detailed issue list |
| `positives` | array | Good practices observed |
| `report` | string | Formatted review report |

---

## Usage

### Standard Review

```yaml
invoke: review-execution
inputs:
  target: 
    - src/Domain/Entities/Order.cs
    - src/Domain/Services/OrderService.cs
  review_type: standard
```

### Security-Focused Review

```yaml
invoke: review-execution
inputs:
  target: src/Infrastructure/
  review_type: security
  focus_areas:
    - input_validation
    - authentication
    - data_protection
```

### Design Comparison

```yaml
invoke: review-execution
inputs:
  target: src/Domain/Entities/Order.cs
  review_type: deep
  design_doc: changes/20240115-001/design.md
```

---

## Review Categories

```yaml
categories:
  formatting:
    weight: 1
    checks:
      - consistent_formatting
      - naming_conventions
      - file_organization
      
  code_quality:
    weight: 2
    checks:
      - no_duplication
      - single_responsibility
      - appropriate_abstraction
      - clean_code
      
  design:
    weight: 3
    checks:
      - pattern_compliance
      - layer_separation
      - dependency_direction
      - interface_usage
      
  logic:
    weight: 3
    checks:
      - correctness
      - edge_cases
      - null_safety
      - error_handling
      
  security:
    weight: 4
    checks:
      - input_validation
      - sql_injection
      - xss_prevention
      - authorization
      
  performance:
    weight: 2
    checks:
      - no_obvious_bottlenecks
      - appropriate_caching
      - resource_management
```

---

## Example Output

```yaml
review_result:
  status: changes_requested
  
  summary:
    critical: 0
    major: 2
    minor: 3
    suggestions: 2
    
  issues:
    - severity: major
      category: design
      file: src/Domain/Entities/Order.cs
      line: 45
      check: pattern_compliance
      message: "Entity directly accesses repository - violates DDD"
      suggestion: "Move data access to application service"
      
    - severity: major
      category: logic
      file: src/Domain/Services/OrderService.cs
      line: 78
      check: null_safety
      message: "Potential null reference - customer not checked"
      suggestion: "Add null check or use null-conditional operator"
      
    - severity: minor
      category: formatting
      file: src/Domain/Entities/Order.cs
      line: 12
      check: naming_conventions
      message: "Private field should start with underscore"
      suggestion: "Rename 'items' to '_items'"
      
  positives:
    - "Good use of value objects for Money"
    - "Clear method naming following domain language"
    - "Appropriate encapsulation of internal state"
    
  report: |
    # Code Review Report
    
    ## Summary
    **Status**: Changes Requested
    
    | Severity | Count |
    |----------|-------|
    | Critical | 0 |
    | Major | 2 |
    | Minor | 3 |
    | Suggestions | 2 |
    
    ## Major Issues (Must Fix)
    1. **[Order.cs:45]** Entity violates DDD by accessing repository
    2. **[OrderService.cs:78]** Potential null reference
    
    ## Minor Issues
    ...
```

---

## Integration

This skill is used by:
- **Reviewer**: Primary user for code review
- **Developer**: Self-review before submission
