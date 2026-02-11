# Service Template (Abstract)

This template defines the structure for creating services (domain and application).

## Purpose

Services encapsulate:
- **Domain Services**: Business logic that doesn't fit in entities
- **Application Services**: Use case orchestration

## Domain Service

### When to Use
- Logic spans multiple aggregates
- Logic is a domain concept itself
- Operation doesn't belong to any single entity

### Structure

```
CLASS ${service_name}Service

    // Dependencies (other domain services, NOT repositories)
    PRIVATE dependencies...
    
    // Constructor
    CONSTRUCTOR(dependencies...):
        SET dependencies
    
    // Domain operations (stateless)
    METHOD ${operation_name}(params) -> result:
        VALIDATE preconditions
        PERFORM domain logic
        RETURN result

END CLASS
```

## Application Service (Use Case)

### When to Use
- Orchestrating domain objects
- Handling transactions
- Coordinating external services

### Structure

```
CLASS ${use_case_name}UseCase

    // Dependencies
    PRIVATE repository: I${entity_name}Repository
    PRIVATE domain_service: ${service_name}Service (optional)
    PRIVATE event_publisher: IEventPublisher (optional)
    
    // Constructor
    CONSTRUCTOR(dependencies...):
        SET dependencies
    
    // Use case execution
    METHOD Execute(request: ${use_case_name}Request) -> ${use_case_name}Response:
        // 1. Validate input
        VALIDATE request
        
        // 2. Load aggregates
        entity = repository.FindById(request.id)
        
        // 3. Perform domain logic
        entity.DoSomething(params)
        
        // 4. Persist changes
        repository.Update(entity)
        
        // 5. Publish events (optional)
        event_publisher.Publish(entity.Events)
        
        // 6. Return response
        RETURN new ${use_case_name}Response(result)

END CLASS
```

## Template Variables

| Variable | Type | Description |
|----------|------|-------------|
| `service_name` | string | Service name (PascalCase) |
| `service_type` | enum | "domain" or "application" |
| `operations` | array | Operation definitions |
| `dependencies` | array | Required dependencies |

## Input/Output Models (Application Services)

### Request Model
```
CLASS ${use_case_name}Request
    // Input data
    PROPERTY param1: type
    PROPERTY param2: type
    
    // Validation (optional, framework-dependent)
    VALIDATION rules...
END CLASS
```

### Response Model
```
CLASS ${use_case_name}Response
    // Output data
    PROPERTY result: type
    PROPERTY message: string (optional)
END CLASS
```

## Common Patterns

### Domain Service Examples

| Service | Operation | Description |
|---------|-----------|-------------|
| PricingService | CalculatePrice(items, customer) | Complex pricing logic |
| TransferService | Transfer(from, to, amount) | Cross-aggregate operation |
| AvailabilityService | CheckAvailability(product, date) | Spans multiple entities |

### Application Service Examples

| Use Case | Description |
|----------|-------------|
| CreateOrderUseCase | Orchestrates order creation |
| ApproveRequestUseCase | Handles approval workflow |
| GenerateReportUseCase | Coordinates data aggregation |

## Design Rules

### Domain Services
- Stateless
- No repository dependencies
- Pure domain logic
- Named using domain language

### Application Services
- Thin orchestration layer
- No business logic (delegate to domain)
- Handle transactions
- Map between external and domain models

## Validation Rules

- Domain services: No infrastructure dependencies
- Application services: Orchestration only
- Single responsibility per service
- Dependencies injected, not created

## Examples

See language-specific implementations:
- `../dotnet/service.template.cs`
- `../java/Service.template.java`
- `../python/service.template.py`
