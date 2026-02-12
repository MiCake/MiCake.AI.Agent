# DDD Modeling Skill

Domain modeling using DDD tactical patterns.

---

## Skill Definition

```yaml
skill:
  id: ddd-modeling
  name: "DDD Modeling"
  version: "1.0"
  description: "Domain modeling using DDD tactical patterns"
```

---

## Context Loading

```yaml
context:
  required:
    - knowledge/patterns/ddd/tactical-patterns.md   # DDD patterns
    - knowledge/patterns/ddd/manifest.yaml          # DDD terminology
  optional:
    - context/architecture-model.yaml               # Current architecture
  never:
    - knowledge/core/code-quality.md                # Not relevant for modeling
    - knowledge/core/review-checklist.md            # Not relevant
    - templates/*                                    # Code generation separate
```

**Context Size**: ~4-6KB (DDD-focused)

---

## Capabilities

### 1. Entity Identification

Identify domain entities:
- What has identity?
- What has lifecycle?
- What needs tracking?

### 2. Value Object Identification

Identify value objects:
- What is immutable?
- What is defined by attributes?
- What represents a concept?

### 3. Aggregate Design

Design aggregates:
- Consistency boundaries
- Root identification
- Size optimization

### 4. Domain Event Identification

Identify domain events:
- What significant changes occur?
- What needs to be communicated?
- What triggers reactions?

### 5. Relationship Modeling

Model relationships:
- Aggregate references
- Entity relationships
- Navigation design

---

## Inputs

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `domain_concepts` | array | Yes | Concepts from requirement parsing |
| `existing_model` | string | No | Path to existing domain model |
| `focus` | enum | No | `entities`, `aggregates`, `events`, `full` |

---

## Outputs

| Name | Type | Description |
|------|------|-------------|
| `entities` | array | Identified entities |
| `value_objects` | array | Identified value objects |
| `aggregates` | array | Aggregate designs |
| `domain_events` | array | Domain events |
| `relationships` | array | Entity relationships |
| `diagram` | string | Domain model diagram (mermaid) |

---

## Usage

### Full Domain Model

```yaml
invoke: ddd-modeling
inputs:
  domain_concepts:
    - name: Order
      description: Customer purchase request
    - name: OrderItem
      description: Line item in order
    - name: Money
      description: Monetary amount
  focus: full
```

### Aggregate Design

```yaml
invoke: ddd-modeling
inputs:
  domain_concepts: [...]
  focus: aggregates
```

---

## Example Output

```yaml
modeling_result:
  entities:
    - name: Order
      identity: Guid
      is_aggregate_root: true
      properties:
        - name: CustomerId
          type: Guid
          description: Reference to customer
        - name: OrderDate
          type: DateTime
        - name: Status
          type: OrderStatus
      behaviors:
        - name: AddItem
          description: Add item to order
        - name: Cancel
          description: Cancel the order
          
    - name: OrderItem
      identity: Guid
      is_aggregate_root: false
      owned_by: Order
      properties:
        - name: ProductId
          type: Guid
        - name: Quantity
          type: int
        - name: UnitPrice
          type: Money
          
  value_objects:
    - name: Money
      properties:
        - name: Amount
          type: decimal
        - name: Currency
          type: string
      invariants:
        - "Amount >= 0"
        - "Currency is valid ISO code"
        
    - name: Address
      properties:
        - name: Street
          type: string
        - name: City
          type: string
        - name: PostalCode
          type: string
        - name: Country
          type: string
          
  aggregates:
    - root: Order
      contains:
        - OrderItem
      invariants:
        - "Order must have at least one item"
        - "Total amount equals sum of item amounts"
      lifecycle:
        - "Created with Pending status"
        - "Can be cancelled only when Pending"
      references:
        - entity: Customer
          type: by_id
          
  domain_events:
    - name: OrderPlaced
      triggered_by: Order creation
      data:
        - orderId
        - customerId
        - totalAmount
        
    - name: OrderCancelled
      triggered_by: Order.Cancel()
      data:
        - orderId
        - reason
        
  diagram: |
    ```mermaid
    classDiagram
        class Order {
            +Guid Id
            +Guid CustomerId
            +DateTime OrderDate
            +OrderStatus Status
            +Money TotalAmount
            +AddItem()
            +Cancel()
        }
        class OrderItem {
            +Guid Id
            +Guid ProductId
            +int Quantity
            +Money UnitPrice
        }
        class Money {
            +decimal Amount
            +string Currency
        }
        Order "1" *-- "*" OrderItem
        OrderItem --> Money
        Order --> Money
    ```
```

---

## Integration

This skill is used by:
- **Architect**: Primary user for domain design
- **Analyst**: To validate domain understanding
- **Developer**: To understand domain model during implementation
