# Entity Template (Abstract)

This template defines the structure for creating domain entities.

## Purpose

Entities are objects with:
- **Identity**: Distinguished by a unique identifier
- **Lifecycle**: Created, modified, and potentially deleted over time
- **Behavior**: Encapsulate business logic and invariants

## Structure

### Required Elements

1. **Identity Field**
   - Type determined by adapter (Guid, UUID, Long, etc.)
   - Immutable after creation
   - Used for equality comparison

2. **Properties**
   - Private setters for encapsulation
   - Validation in setters if mutable
   - Value objects for complex types

3. **Constructor**
   - Validates all invariants
   - Sets required properties
   - Can have private parameterless for ORM

4. **Business Methods**
   - State-changing operations
   - Invariant enforcement
   - Named using domain language

### Optional Elements

1. **Domain Events**
   - Raised when significant state changes
   - Named in past tense

2. **Factory Methods**
   - For complex construction
   - Named descriptively

## Template Variables

| Variable | Type | Description |
|----------|------|-------------|
| `entity_name` | string | PascalCase class name |
| `id_type` | string | Type for ID (from adapter) |
| `entity_base` | string | Base class (from adapter) |
| `properties` | array | Property definitions |
| `methods` | array | Method definitions |
| `events` | array | Domain events to raise |

## Code Structure (Pseudocode)

```
CLASS ${entity_name} EXTENDS ${entity_base}<${id_type}>

    // Properties
    FOR EACH property IN properties:
        PROPERTY property.name : property.type (private setter)
    
    // Constructor
    CONSTRUCTOR(required_params):
        VALIDATE invariants
        SET properties
    
    // Private constructor for ORM (optional)
    PRIVATE CONSTRUCTOR()
    
    // Business Methods
    FOR EACH method IN methods:
        METHOD method.name(method.params) -> method.return_type:
            VALIDATE preconditions
            PERFORM logic
            VALIDATE invariants
            RAISE events if needed
    
    // Equality (based on ID)
    OVERRIDE equals(other):
        RETURN this.id == other.id

END CLASS
```

## Validation Rules

- Entity name must be PascalCase
- At least one property beyond ID
- Constructor must validate required fields
- Business methods should use domain language

## Examples

See language-specific implementations:
- `../dotnet/entity.template.cs`
- `../java/Entity.template.java`
- `../python/entity.template.py`
