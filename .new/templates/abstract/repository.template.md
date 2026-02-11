# Repository Template (Abstract)

This template defines the structure for creating repositories.

## Purpose

Repositories provide:
- **Collection-like Interface**: Abstraction over persistence
- **Aggregate Access**: One repository per aggregate type
- **Persistence Ignorance**: Domain doesn't know storage details

## Structure

### Interface (in Domain Layer)

1. **Basic Operations**
   - GetById / FindById
   - Add / Save
   - Remove / Delete

2. **Query Methods**
   - FindAll
   - FindBy{Criteria}
   - Custom domain queries

3. **Optional Operations**
   - Count
   - Exists
   - Batch operations

### Implementation (in Infrastructure Layer)

1. **Persistence Logic**
   - ORM integration
   - Query translation
   - Connection management

2. **Mapping**
   - Domain to persistence model
   - Persistence to domain model

## Template Variables

| Variable | Type | Description |
|----------|------|-------------|
| `entity_name` | string | Entity name (PascalCase) |
| `entity_name_plural` | string | Plural form for collection methods |
| `id_type` | string | ID type from adapter |
| `repository_base` | string | Base interface from adapter |
| `queries` | array | Custom query method definitions |

## Code Structure (Pseudocode)

```
// Interface (Domain Layer)
INTERFACE I${entity_name}Repository EXTENDS ${repository_base}<${entity_name}, ${id_type}>

    // Basic operations (may be inherited from base)
    METHOD FindById(id: ${id_type}) -> ${entity_name} OR NULL
    METHOD Add(entity: ${entity_name}) -> VOID
    METHOD Update(entity: ${entity_name}) -> VOID
    METHOD Remove(entity: ${entity_name}) -> VOID
    
    // Custom queries
    FOR EACH query IN queries:
        METHOD query.name(query.params) -> query.return_type

END INTERFACE


// Implementation (Infrastructure Layer)
CLASS ${entity_name}Repository 
    IMPLEMENTS I${entity_name}Repository

    // Dependencies
    PRIVATE db_context : DatabaseContext
    
    // Constructor
    CONSTRUCTOR(context: DatabaseContext):
        SET db_context = context
    
    // Implementations
    METHOD FindById(id: ${id_type}) -> ${entity_name} OR NULL:
        RETURN db_context.${entity_name_plural}.Find(id)
    
    METHOD Add(entity: ${entity_name}) -> VOID:
        db_context.${entity_name_plural}.Add(entity)
    
    METHOD Update(entity: ${entity_name}) -> VOID:
        db_context.${entity_name_plural}.Update(entity)
    
    METHOD Remove(entity: ${entity_name}) -> VOID:
        db_context.${entity_name_plural}.Remove(entity)
    
    // Custom query implementations
    FOR EACH query IN queries:
        METHOD query.name(query.params) -> query.return_type:
            RETURN db_context query implementation

END CLASS
```

## Common Query Patterns

| Pattern | Example | Return Type |
|---------|---------|-------------|
| Find by unique field | `FindByEmail(email)` | Entity or null |
| Find multiple | `FindByStatus(status)` | List |
| Exists check | `ExistsByCode(code)` | Boolean |
| Count | `CountByCategory(category)` | Integer |
| Paged | `FindAllPaged(page, size)` | Page result |

## Design Rules

1. **One per Aggregate**: Only aggregate roots get repositories
2. **Domain Language**: Method names use ubiquitous language
3. **No Leak**: Implementation details stay in infrastructure
4. **Return Domain Objects**: Never return persistence models

## Validation Rules

- Interface name: I{Entity}Repository
- Implementation name: {Entity}Repository
- Only for aggregate roots
- No business logic in repository
- Unit of Work pattern for transactions

## Examples

See language-specific implementations:
- `../dotnet/repository.template.cs`
- `../java/Repository.template.java`
- `../python/repository.template.py`
