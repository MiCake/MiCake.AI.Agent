# Template Generation Skill

Generate code from templates based on adapter and pattern configuration.

---

## Skill Definition

```yaml
skill:
  id: template-generation
  name: "Template Generation"
  version: "1.0"
  description: "Generate code from templates"
```

---

## Context Loading

```yaml
context:
  required:
    - templates/_engine/variables.yaml          # Template variables
    - templates/_project/*                       # Project-specific templates
    - context/project-structure.yaml             # Project conventions
  optional:
    - templates/abstract/${template_type}*      # Abstract template (fallback)
    - knowledge/patterns/${pattern}/manifest.yaml  # Pattern terminology
  never:
    - knowledge/core/*                          # General knowledge not needed
    - requirements/*                            # Not needed for generation
    - workflows/*                               # Workflow definitions not needed
```

**Context Size**: ~3-8KB (template-specific)

---

## Capabilities

### 1. Entity Generation

Generate entity/model classes with:
- Properties
- Constructors
- Business methods
- Validation

### 2. Repository Generation

Generate repository interfaces and implementations:
- Interface definition
- Basic CRUD methods
- Custom queries

### 3. Service Generation

Generate service classes:
- Domain services
- Application services
- Use cases

### 4. Value Object Generation

Generate immutable value objects:
- Properties
- Equality
- Validation

### 5. DTO/Model Generation

Generate data transfer objects:
- Request/Response models
- View models

---

## Inputs

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `template_type` | enum | Yes | Type: `entity`, `repository`, `service`, `value-object`, `dto` |
| `name` | string | Yes | Name for generated component |
| `properties` | array | No | Property definitions |
| `methods` | array | No | Method definitions |
| `options` | object | No | Additional options |

---

## Outputs

| Name | Type | Description |
|------|------|-------------|
| `code` | string | Generated code |
| `file_path` | string | Suggested file location |
| `dependencies` | array | Required imports/dependencies |
| `tests` | string | Optional: generated test code |

---

## Usage

### Basic Entity Generation

```yaml
invoke: template-generation
inputs:
  template_type: entity
  name: Order
  properties:
    - name: CustomerId
      type: Guid
      required: true
    - name: OrderDate
      type: DateTime
      required: true
    - name: Status
      type: OrderStatus
      required: true
    - name: TotalAmount
      type: Money
      required: false
```

### Repository Generation

```yaml
invoke: template-generation
inputs:
  template_type: repository
  name: Order
  options:
    custom_queries:
      - name: FindByCustomer
        params: [customerId: Guid]
        returns: List<Order>
      - name: FindPending
        returns: List<Order>
```

---

## Template Resolution

```
1. Check: templates/${adapter}/${type}.template.*
   ↓ (if not found)
2. Check: templates/abstract/${type}.template.md
   ↓ (apply adapter variables)
3. Generate code with substitutions
```

---

## Variable Substitution

Variables from multiple sources:

```yaml
# From adapter
${namespace}           → "Company.Project.Domain"
${id_type}             → "Guid"
${entity_base}         → "Entity<TId>"

# From pattern
${aggregate_interface} → "IAggregateRoot"

# From input
${entity_name}         → "Order"
${properties}          → [generated property code]
```

---

## Example Output

Input:
```yaml
template_type: entity
name: Order
adapter: dotnet
pattern: ddd
```

Output:
```csharp
using System;
using Company.Project.Domain.Shared;

namespace Company.Project.Domain.Entities
{
    /// <summary>
    /// Order entity.
    /// </summary>
    public class Order : Entity<Guid>, IAggregateRoot
    {
        public Guid CustomerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }
        public Money TotalAmount { get; private set; }
        
        public Order(Guid customerId, DateTime orderDate)
        {
            CustomerId = customerId;
            OrderDate = orderDate;
            Status = OrderStatus.Pending;
        }
        
        private Order() { }
    }
}
```

---

## Integration

This skill is used by:
- **Developer**: Primary user for code generation
- **Architect**: When creating initial structure
