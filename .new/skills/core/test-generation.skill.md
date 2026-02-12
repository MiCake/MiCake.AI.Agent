# Test Generation Skill

Generate test cases and test code based on implementation.

---

## Skill Definition

```yaml
skill:
  id: test-generation
  name: "Test Generation"
  version: "1.0"
  description: "Generate test cases and test code"
```

---

## Context Loading

```yaml
context:
  required:
    - context/project-structure.yaml    # Project test conventions
  optional:
    - knowledge/patterns/${pattern}/overview.md  # Pattern understanding for better tests
  never:
    - requirements/*                      # Direct requirement access not needed
    - knowledge/core/review-checklist.md  # Not relevant for test generation
    - workflows/*                         # Not relevant
```

**Context Size**: ~2-4KB (test-focused)

---

## Capabilities

### 1. Unit Test Generation

Generate unit tests for:
- Methods and functions
- Classes and modules
- Edge cases
- Error conditions

### 2. Test Case Design

Design test cases:
- Happy path scenarios
- Edge cases
- Error scenarios
- Boundary conditions

### 3. Mock/Stub Generation

Generate test doubles:
- Mock objects
- Stubs
- Fakes
- Spy objects

### 4. Assertion Design

Design appropriate assertions:
- State verification
- Behavior verification
- Exception verification

---

## Inputs

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `target` | string | Yes | Code to generate tests for |
| `test_type` | enum | No | `unit`, `integration`, `both` |
| `coverage_focus` | enum | No | `happy_path`, `edge_cases`, `comprehensive` |
| `framework` | string | No | Test framework (auto-detected from adapter) |

---

## Outputs

| Name | Type | Description |
|------|------|-------------|
| `test_cases` | array | Designed test cases |
| `test_code` | string | Generated test code |
| `file_path` | string | Suggested test file location |
| `mocks_needed` | array | Required mock objects |

---

## Usage

### Basic Unit Tests

```yaml
invoke: test-generation
inputs:
  target: src/Domain/Entities/Order.cs
  test_type: unit
  coverage_focus: comprehensive
```

### Edge Case Focus

```yaml
invoke: test-generation
inputs:
  target: src/Domain/Services/PricingService.cs
  coverage_focus: edge_cases
```

---

## Test Case Structure

```yaml
test_cases:
  - name: "Should create order with valid data"
    type: happy_path
    given: "Valid customer and items"
    when: "Creating new order"
    then: "Order is created with Pending status"
    
  - name: "Should throw when customer is null"
    type: error_case
    given: "Null customer"
    when: "Creating new order"
    then: "ArgumentNullException is thrown"
    
  - name: "Should handle empty items list"
    type: edge_case
    given: "Valid customer, empty items"
    when: "Creating new order"
    then: "InvalidOperationException is thrown"
```

---

## Example Output (C#)

Input: `src/Domain/Entities/Order.cs`

```csharp
using Xunit;
using FluentAssertions;
using Company.Domain.Entities;
using Company.Domain.ValueObjects;

namespace Company.Domain.Tests.Entities
{
    public class OrderTests
    {
        #region Constructor Tests
        
        [Fact]
        public void Constructor_WithValidData_ShouldCreateOrder()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var orderDate = DateTime.UtcNow;
            
            // Act
            var order = new Order(customerId, orderDate);
            
            // Assert
            order.CustomerId.Should().Be(customerId);
            order.OrderDate.Should().Be(orderDate);
            order.Status.Should().Be(OrderStatus.Pending);
        }
        
        [Fact]
        public void Constructor_WithEmptyCustomerId_ShouldThrow()
        {
            // Arrange
            var customerId = Guid.Empty;
            
            // Act
            var act = () => new Order(customerId, DateTime.UtcNow);
            
            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*customerId*");
        }
        
        #endregion
        
        #region Behavior Tests
        
        [Fact]
        public void Cancel_WhenPending_ShouldChangeToCancelled()
        {
            // Arrange
            var order = CreateTestOrder();
            
            // Act
            order.Cancel();
            
            // Assert
            order.Status.Should().Be(OrderStatus.Cancelled);
        }
        
        [Fact]
        public void Cancel_WhenShipped_ShouldThrow()
        {
            // Arrange
            var order = CreateShippedOrder();
            
            // Act
            var act = () => order.Cancel();
            
            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
        
        #endregion
        
        #region Helpers
        
        private static Order CreateTestOrder()
        {
            return new Order(Guid.NewGuid(), DateTime.UtcNow);
        }
        
        private static Order CreateShippedOrder()
        {
            var order = CreateTestOrder();
            // Set up shipped state
            return order;
        }
        
        #endregion
    }
}
```

---

## Integration

This skill is used by:
- **Tester**: Primary user for test creation
- **Developer**: Creating tests alongside implementation
