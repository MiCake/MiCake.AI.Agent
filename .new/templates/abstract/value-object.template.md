# Value Object Template (Abstract)

This template defines the structure for creating value objects.

## Purpose

Value Objects are objects with:
- **No Identity**: Defined entirely by their attributes
- **Immutability**: Cannot be changed after creation
- **Conceptual Whole**: Represent a single domain concept

## Structure

### Required Elements

1. **Properties**
   - All readonly/immutable
   - No public setters
   - Defined at construction

2. **Constructor**
   - Validates all invariants
   - Sets all properties
   - Throws on invalid state

3. **Equality**
   - Based on all attributes
   - Override equals and hashcode
   - Two instances with same values are equal

### Optional Elements

1. **Factory Methods**
   - Parse from string/other formats
   - Create with defaults

2. **Transformation Methods**
   - "Modify" by returning new instance
   - Named descriptively (e.g., `WithUpdatedAmount`)

## Template Variables

| Variable | Type | Description |
|----------|------|-------------|
| `vo_name` | string | PascalCase class name |
| `vo_base` | string | Base class (from adapter) |
| `properties` | array | Property definitions |
| `validations` | array | Validation rules |

## Code Structure (Pseudocode)

```
CLASS ${vo_name} EXTENDS ${vo_base}

    // Properties (all readonly)
    FOR EACH property IN properties:
        READONLY PROPERTY property.name : property.type
    
    // Constructor with validation
    CONSTRUCTOR(all_params):
        FOR EACH validation IN validations:
            IF NOT validation.condition:
                THROW ArgumentException(validation.message)
        
        FOR EACH property IN properties:
            SET property.name = param.value
    
    // Equality (based on all attributes)
    OVERRIDE equals(other):
        IF other IS NULL: RETURN false
        IF other IS NOT Same Type: RETURN false
        RETURN all properties equal
    
    OVERRIDE getHashCode():
        RETURN compute hash from all properties
    
    // Transformation (returns new instance)
    METHOD With{PropertyName}(newValue) -> ${vo_name}:
        RETURN new ${vo_name}(existing props, newValue)

END CLASS
```

## Common Value Object Examples

| Type | Properties | Validations |
|------|------------|-------------|
| Money | amount, currency | amount >= 0, valid currency |
| Email | value | valid email format |
| Address | street, city, postal, country | non-empty required fields |
| DateRange | start, end | start <= end |
| PhoneNumber | countryCode, number | valid format |

## Validation Rules

- Name should describe the concept (Money, Address, not MoneyVO)
- All properties immutable
- No setters or mutation methods
- "With" methods return new instances
- Equality properly implemented

## Examples

See language-specific implementations:
- `../dotnet/value-object.template.cs`
- `../java/ValueObject.template.java`
- `../python/value_object.template.py`
