# MiCake 最佳实践

> 基于 MiCake 开发原则的最佳实践指南

## 概述

本文档汇总了使用 MiCake 框架开发应用程序的最佳实践，基于 `principles/development_principle.md` 中定义的开发原则。

---

## 1. 架构原则

### 四层架构

保持清晰的四层架构：

```
┌─────────────────────────────────────────────┐
│         Presentation Layer                   │
│      (Controllers, API Endpoints)            │
├─────────────────────────────────────────────┤
│         Application Layer                    │
│    (Application Services, DTOs, Mappers)     │
├─────────────────────────────────────────────┤
│           Domain Layer                       │
│  (Entities, Aggregates, Domain Services)     │
├─────────────────────────────────────────────┤
│        Infrastructure Layer                  │
│  (Repositories, DbContext, External APIs)    │
└─────────────────────────────────────────────┘
```

### 依赖方向

```
✅ 正确的依赖方向（向内）:
   Presentation → Application → Domain ← Infrastructure

❌ 错误的依赖（向外）:
   Domain → Infrastructure
   Domain → Presentation
```

### 命名空间约定

```csharp
// ✅ 正确
namespace MyCompany.MyApp.Domain.Aggregates
namespace MyCompany.MyApp.Application.Services
namespace MyCompany.MyApp.Infrastructure.Repositories

// ❌ 避免创建新的根命名空间
namespace MyNewRootNamespace.Something
```

---

## 2. 依赖注入

### 显式依赖

```csharp
// ✅ 推荐：显式构造函数注入
public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderService> _logger;
    
    public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }
}

// ❌ 避免：服务定位器模式
public class OrderService
{
    private readonly IServiceProvider _serviceProvider;
    
    public OrderService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public void DoWork()
    {
        var repo = _serviceProvider.GetRequiredService<IOrderRepository>(); // 避免
    }
}
```

### 依赖包装器

当依赖超过 2 个时，使用依赖包装器：

```csharp
// ✅ 推荐：使用依赖包装器
public class OrderServiceDependencies
{
    public IOrderRepository OrderRepository { get; }
    public ICustomerRepository CustomerRepository { get; }
    public IPaymentService PaymentService { get; }
    public ILogger<OrderService> Logger { get; }
    
    public OrderServiceDependencies(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IPaymentService paymentService,
        ILogger<OrderService> logger)
    {
        OrderRepository = orderRepository;
        CustomerRepository = customerRepository;
        PaymentService = paymentService;
        Logger = logger;
    }
}

public class OrderService
{
    private readonly OrderServiceDependencies _dependencies;
    
    public OrderService(OrderServiceDependencies dependencies)
    {
        _dependencies = dependencies;
    }
}
```

---

## 3. 异步编程

### ConfigureAwait(false)

库代码中始终使用 `ConfigureAwait(false)`：

```csharp
// ✅ 推荐
public async Task<Order> GetOrderAsync(long id, CancellationToken cancellationToken)
{
    return await _dbContext.Orders
        .FirstOrDefaultAsync(o => o.Id == id, cancellationToken)
        .ConfigureAwait(false);
}

// ❌ 避免（在库代码中）
public async Task<Order> GetOrderAsync(long id, CancellationToken cancellationToken)
{
    return await _dbContext.Orders
        .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
}
```

### 避免阻塞异步

```csharp
// ❌ 严禁
var result = GetOrderAsync(id).Result;        // 死锁风险
var result = GetOrderAsync(id).Wait();        // 死锁风险
var result = GetOrderAsync(id).GetAwaiter().GetResult(); // 死锁风险

// ✅ 正确
var result = await GetOrderAsync(id);
```

---

## 4. 错误处理

### 输入验证

```csharp
// ✅ 使用新的 ThrowIf 方法
public void ProcessOrder(Order order, string notes)
{
    ArgumentNullException.ThrowIfNull(order);
    ArgumentException.ThrowIfNullOrWhiteSpace(notes);
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(order.Id);
}

// 旧方式（仍然有效，但更冗长）
public void ProcessOrder(Order order, string notes)
{
    if (order == null) throw new ArgumentNullException(nameof(order));
    if (string.IsNullOrWhiteSpace(notes)) throw new ArgumentException("Notes required", nameof(notes));
}
```

### 日志记录

```csharp
// ✅ 包含上下文信息的日志
_logger.LogInformation("Processing order {OrderId} for customer {CustomerId}", 
    order.Id, order.CustomerId);

_logger.LogError(ex, "Failed to process order {OrderId}. Status: {Status}", 
    order.Id, order.Status);

// ❌ 避免无上下文的日志
_logger.LogInformation("Processing order");
_logger.LogError("Error occurred");
```

---

## 5. 资源管理

### Dispose 模式

```csharp
public class MyResource : IDisposable
{
    private bool _disposed;
    private readonly SomeUnmanagedResource _resource;
    
    public void DoWork()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        // 执行工作
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        if (disposing)
        {
            // 释放托管资源
        }
        
        // 释放非托管资源
        try
        {
            _resource?.Dispose();
        }
        catch
        {
            // 吞掉 Dispose 中的异常
        }
        
        _disposed = true;
    }
    
    ~MyResource()
    {
        Dispose(false);
    }
}
```

---

## 6. 命名规范

### 通用规范

| 类型 | 规范 | 示例 |
|------|------|------|
| 公共类型和成员 | PascalCase | `OrderService`, `GetOrder()` |
| 私有字段 | _camelCase | `_orderRepository` |
| 局部变量和参数 | camelCase | `orderId`, `customerName` |
| 常量 | PascalCase | `MaxRetryCount` |
| 异步方法 | Async 后缀 | `GetOrderAsync()` |

### 领域对象命名

```csharp
// 聚合根
public class Order : AggregateRoot<long> { }
public class Customer : AggregateRoot<Guid> { }

// 实体
public class OrderItem : Entity<int> { }

// 值对象
public class Money : ValueObject { }
public class Address : RecordValueObject { }

// 领域事件（过去时态）
public class OrderCreatedEvent : IDomainEvent { }
public class OrderShippedEvent : IDomainEvent { }

// 仓储
public interface IOrderRepository : IRepository<Order, long> { }
public class OrderRepository : EFRepository<MyDbContext, Order, long> { }
```

---

## 7. 测试

### 测试命名

```csharp
// 格式：{Method}_{Scenario}_{ExpectedResult}
[Fact]
public void Create_WithValidData_ReturnsNewOrder()
{
}

[Fact]
public async Task GetByIdAsync_WhenOrderNotFound_ReturnsNull()
{
}

[Fact]
public void AddItem_WhenOrderIsConfirmed_ThrowsInvalidOperationException()
{
}
```

### AAA 模式

```csharp
[Fact]
public void Create_WithValidData_ReturnsNewOrder()
{
    // Arrange
    var customerId = 123L;
    
    // Act
    var order = Order.Create(customerId);
    
    // Assert
    Assert.NotNull(order);
    Assert.Equal(customerId, order.CustomerId);
    Assert.Equal(OrderStatus.Pending, order.Status);
}
```

---

## 8. 性能优化

### 缓存

```csharp
// ✅ 使用缓存避免重复计算
private readonly ConcurrentDictionary<Type, TypeInfo> _typeInfoCache = new();

public TypeInfo GetTypeInfo(Type type)
{
    return _typeInfoCache.GetOrAdd(type, t => ComputeTypeInfo(t));
}
```

### 避免 O(n) 扫描

```csharp
// ❌ 每次调用都扫描
public Order? FindOrder(IEnumerable<Order> orders, long id)
{
    return orders.FirstOrDefault(o => o.Id == id); // O(n)
}

// ✅ 使用索引
private readonly Dictionary<long, Order> _ordersById = new();

public Order? FindOrder(long id)
{
    return _ordersById.TryGetValue(id, out var order) ? order : null; // O(1)
}
```

### 反射优化

```csharp
// ❌ 避免频繁使用 Activator.CreateInstance
var instance = Activator.CreateInstance(type);

// ✅ 使用编译后的激活器
private static readonly ConcurrentDictionary<Type, Func<object>> _activators = new();

public static object CreateInstance(Type type)
{
    return _activators.GetOrAdd(type, t =>
    {
        var ctor = t.GetConstructor(Type.EmptyTypes);
        var newExp = Expression.New(ctor);
        var lambda = Expression.Lambda<Func<object>>(newExp);
        return lambda.Compile();
    })();
}
```

---

## 9. 文档

### XML 文档注释

```csharp
/// <summary>
/// Represents an order in the system.
/// </summary>
/// <remarks>
/// Orders are aggregate roots that manage order items and track order status.
/// All modifications should go through the aggregate root methods.
/// </remarks>
public class Order : AggregateRoot<long>
{
    /// <summary>
    /// Adds a new item to the order.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="productName">The product name.</param>
    /// <param name="unitPrice">The unit price in the order's currency.</param>
    /// <param name="quantity">The quantity to add. Must be positive.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the order is not in pending status.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when quantity is not positive or price is negative.
    /// </exception>
    public void AddItem(int productId, string productName, decimal unitPrice, int quantity)
    {
        // ...
    }
}
```

---

## 10. PR 检查清单

在提交 Pull Request 前，确保：

- [ ] 遵循依赖规则（无向外依赖违规）
- [ ] 公共 API 有 XML 文档注释
- [ ] 无阻塞异步调用（.Result, .Wait()）
- [ ] 需要时实现了 Dispose 模式
- [ ] 添加/更新了单元测试
- [ ] 审查了性能敏感路径的缓存和索引
- [ ] 日志包含上下文信息
- [ ] 遵循命名规范

---

## 相关链接

- [开发原则](../../principles/development_principle.md)
- [DDD 模式指南](./ddd-patterns.md)
- [模块系统](./module-system.md)
- [问题排查](./troubleshooting.md)
