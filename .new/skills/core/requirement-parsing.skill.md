# Requirement Parsing Skill

Parse and analyze requirements from PRD documents.

---

## Skill Definition

```yaml
skill:
  id: requirement-parsing
  name: "Requirement Parsing"
  version: "1.0"
  description: "Parse and analyze requirements from PRD"
```

---

## Context Loading

```yaml
context:
  required:
    - requirements/${prd_file}               # The PRD to parse
  optional:
    - context/project-structure.yaml         # Current project context
    - knowledge/patterns/${pattern}/overview.md  # Pattern concepts (for domain extraction)
  never:
    - templates/*                            # Not needed for parsing
    - knowledge/core/code-quality.md         # Not relevant
    - knowledge/core/review-checklist.md     # Not relevant
```

---

## Capabilities

### 1. Functional Requirement Extraction

Extract functional requirements:
- Feature descriptions
- User stories
- Use cases
- Business rules

### 2. Acceptance Criteria Extraction

Identify testable criteria:
- Given/When/Then scenarios
- Success conditions
- Edge cases

### 3. Domain Concept Identification

Extract domain concepts:
- Entities mentioned
- Actions/behaviors
- Relationships
- Business terminology

### 4. Dependency Analysis

Identify dependencies:
- Prerequisites
- Related features
- External integrations

### 5. Scope Definition

Clarify boundaries:
- In scope
- Out of scope
- Assumptions

---

## Inputs

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `prd_path` | string | Yes | Path to PRD document |
| `focus` | enum | No | `all`, `functional`, `domain`, `criteria` |
| `format` | enum | No | Output format: `structured`, `summary` |

---

## Outputs

| Name | Type | Description |
|------|------|-------------|
| `summary` | string | Brief summary of requirements |
| `functional_requirements` | array | List of functional requirements |
| `domain_concepts` | array | Identified domain concepts |
| `acceptance_criteria` | array | Testable criteria |
| `dependencies` | array | Prerequisites and dependencies |
| `questions` | array | Ambiguities needing clarification |

---

## Usage

### Full Parsing

```yaml
invoke: requirement-parsing
inputs:
  prd_path: requirements/user-registration.md
  focus: all
  format: structured
```

### Domain Focus

```yaml
invoke: requirement-parsing
inputs:
  prd_path: requirements/order-management.md
  focus: domain
```

---

## Example Output

Input PRD: `requirements/order-management.md`

```yaml
parsing_result:
  summary: "Order management system allowing customers to place, track, and cancel orders"
  
  functional_requirements:
    - id: FR-1
      title: "Place Order"
      description: "Customer can create a new order with selected items"
      priority: high
      
    - id: FR-2
      title: "Cancel Order"
      description: "Customer can cancel a pending order"
      conditions: ["Order status is 'Pending'"]
      priority: medium
      
  domain_concepts:
    entities:
      - name: "Order"
        description: "Represents a customer order"
        attributes: ["id", "customer", "items", "status", "total"]
        
      - name: "OrderItem"
        description: "Line item within an order"
        attributes: ["product", "quantity", "price"]
        
    value_objects:
      - name: "Money"
        description: "Monetary amount with currency"
        
    enums:
      - name: "OrderStatus"
        values: ["Pending", "Confirmed", "Shipped", "Delivered", "Cancelled"]
        
  acceptance_criteria:
    - requirement: FR-1
      criteria:
        - "Given valid items, when placing order, then order is created with Pending status"
        - "Given empty cart, when placing order, then error is shown"
        
  dependencies:
    - "Product catalog must exist"
    - "Customer authentication required"
    
  questions:
    - "What payment methods are supported?"
    - "Is order modification allowed after confirmation?"
```

---

## Integration

This skill is used by:
- **Analyst**: Primary user for requirement analysis
- **Architect**: To understand requirements for design
