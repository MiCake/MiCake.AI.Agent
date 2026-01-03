# DDD 模式指南

> MiCake 框架中的领域驱动设计核心模式

## 概述

MiCake 是一个轻量级的 DDD 工具包，提供了构建领域驱动设计应用的核心抽象。本文档介绍 MiCake 中的 DDD 核心模式及其使用方法。

---

## 1. Entity（实体）

### 定义
实体是具有唯一标识的领域对象，其身份在整个生命周期中保持不变。

### MiCake 实现

```csharp
using MiCake.DDD.Domain;

/// <summary>
/// 订单项实体
/// </summary>
public class OrderItem : Entity<int>
{
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    
    // 使用私有构造函数保护不变量
    private OrderItem() { }
    
    public OrderItem(int productId, string productName, decimal unitPrice, int quantity)
    {
        ArgumentNullException.ThrowIfNull(productName);
        if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative");
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive");
        
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
    
    public decimal GetTotalPrice() => UnitPrice * Quantity;
}
```

### 关键特征
- 继承自 `Entity<TKey>` 或 `Entity`（默认 int 键）
- 通过 `Id` 属性标识唯一性
- 使用私有 setter 保护状态
- 在构造函数中验证不变量

---

## 2. Aggregate Root（聚合根）

### 定义
聚合根是聚合的入口点，定义了一致性边界。外部对象只能通过聚合根访问聚合内的实体。

### MiCake 实现

```csharp
using MiCake.DDD.Domain;

/// <summary>
/// 订单聚合根
/// </summary>
public class Order : AggregateRoot<long>
{
    private readonly List<OrderItem> _items = new();
    
    public long CustomerId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    
    // EF Core 需要的无参构造函数
    private Order() { }
    
    // 工厂方法模式（可选，根据偏好设置）
    public static Order Create(long customerId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId);
        
        var order = new Order
        {
            CustomerId = customerId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending
        };
        
        // 发布领域事件
        order.AddDomainEvent(new OrderCreatedEvent(order.Id, customerId));
        
        return order;
    }
    
    // 领域方法
    public void AddItem(int productId, string productName, decimal unitPrice, int quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot add items to a non-pending order");
            
        var item = new OrderItem(productId, productName, unitPrice, quantity);
        _items.Add(item);
        
        AddDomainEvent(new OrderItemAddedEvent(Id, productId, quantity));
    }
    
    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be confirmed");
        if (!_items.Any())
            throw new InvalidOperationException("Cannot confirm an empty order");
            
        Status = OrderStatus.Confirmed;
        AddDomainEvent(new OrderConfirmedEvent(Id));
    }
    
    public decimal GetTotalAmount() => _items.Sum(i => i.GetTotalPrice());
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Completed,
    Cancelled
}
```

### 关键特征
- 继承自 `AggregateRoot<TKey>`
- 是仓储操作的唯一入口
- 保护聚合内实体的不变量
- 通过 `AddDomainEvent()` 发布领域事件
- 提供有意义的领域方法，而非简单的 setter

### 设计原则
1. **最小化聚合** - 聚合应尽可能小，只包含必须保持一致性的对象
2. **引用其他聚合使用 ID** - 不要直接引用其他聚合根实例
3. **一个事务一个聚合** - 一次事务只修改一个聚合

---

## 3. Value Object（值对象）

### 定义
值对象是没有身份的不可变对象，通过其属性值来定义相等性。

### MiCake 实现

```csharp
using MiCake.DDD.Domain;

/// <summary>
/// 金额值对象
/// </summary>
public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(currency);
        
        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
    
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add money with different currencies");
        return new Money(Amount + other.Amount, Currency);
    }
    
    public override string ToString() => $"{Amount:N2} {Currency}";
}

/// <summary>
/// 地址值对象（使用 record 语法，C# 9+）
/// </summary>
public record Address(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country
) : RecordValueObject;
```

### 关键特征
- 继承自 `ValueObject` 或使用 `RecordValueObject`
- 不可变（所有属性都是只读的）
- 通过 `GetEqualityComponents()` 定义相等性
- 没有独立的生命周期

---

## 4. Domain Event（领域事件）

### 定义
领域事件表示领域中发生的重要事件，用于解耦聚合间的通信。

### MiCake 实现

```csharp
using MiCake.DDD.Domain;

/// <summary>
/// 订单创建事件
/// </summary>
public class OrderCreatedEvent : IDomainEvent
{
    public long OrderId { get; }
    public long CustomerId { get; }
    public DateTime OccurredAt { get; }
    
    public OrderCreatedEvent(long orderId, long customerId)
    {
        OrderId = orderId;
        CustomerId = customerId;
        OccurredAt = DateTime.UtcNow;
    }
}

/// <summary>
/// 订单确认事件
/// </summary>
public class OrderConfirmedEvent : IDomainEvent
{
    public long OrderId { get; }
    public DateTime OccurredAt { get; }
    
    public OrderConfirmedEvent(long orderId)
    {
        OrderId = orderId;
        OccurredAt = DateTime.UtcNow;
    }
}
```

### 事件处理器

```csharp
using MiCake.DDD.Domain;

/// <summary>
/// 订单创建事件处理器
/// </summary>
public class OrderCreatedEventHandler : IDomainEventHandler<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventHandler> _logger;
    private readonly INotificationService _notificationService;
    
    public OrderCreatedEventHandler(
        ILogger<OrderCreatedEventHandler> logger,
        INotificationService notificationService)
    {
        _logger = logger;
        _notificationService = notificationService;
    }
    
    public async Task HandleAsync(OrderCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Order {OrderId} was created for customer {CustomerId}", 
            @event.OrderId, @event.CustomerId);
            
        await _notificationService.NotifyOrderCreatedAsync(@event.OrderId, cancellationToken);
    }
}
```

### 关键特征
- 实现 `IDomainEvent` 接口
- 事件名称使用过去时态（表示已发生）
- 包含事件发生时间
- 事件是不可变的
- 通过 `AddDomainEvent()` 在聚合中发布
- 在 `SaveChangesAsync()` 时自动分发

---

## 5. Repository（仓储）

### 定义
仓储提供对聚合根的持久化操作，隐藏底层数据访问细节。

### MiCake 实现

```csharp
// 仓储接口（领域层）
public interface IOrderRepository : IRepository<Order, long>
{
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> GetByCustomerIdAsync(long customerId, CancellationToken cancellationToken = default);
}

// 仓储实现（基础设施层）
public class OrderRepository : EFRepository<MyDbContext, Order, long>, IOrderRepository
{
    public OrderRepository(EFRepositoryDependencies<MyDbContext> dependencies) 
        : base(dependencies)
    {
    }
    
    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
    }
    
    public async Task<IReadOnlyList<Order>> GetByCustomerIdAsync(long customerId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }
}
```

### 关键特征
- 仅为聚合根定义仓储
- 接口定义在领域层
- 实现在基础设施层
- 继承自 `EFRepository<TDbContext, TAggregateRoot, TKey>`
- 通过模块自动注册

---

## 6. 设计决策指南

### 何时使用实体 vs 值对象？

| 考虑因素 | 实体 | 值对象 |
|---------|------|--------|
| 需要跟踪身份？ | ✅ | ❌ |
| 可以替换整个对象？ | ❌ | ✅ |
| 需要独立生命周期？ | ✅ | ❌ |
| 相等性基于？ | ID | 属性值 |

### 何时发布领域事件？

- ✅ 重要的业务操作完成时（订单创建、支付完成）
- ✅ 需要通知其他聚合或系统时
- ✅ 需要记录审计信息时
- ❌ 简单的 CRUD 操作
- ❌ 技术性变更（如缓存更新）

### 聚合边界设计

1. **识别不变量** - 哪些规则必须始终满足？
2. **确定一致性需求** - 哪些数据必须同时保持一致？
3. **最小化聚合** - 只包含必要的实体
4. **最终一致性** - 聚合间使用领域事件保持最终一致

---

## 相关链接

- [MiCake 仓储模式](./repository-patterns.md)
- [MiCake 模块系统](./module-system.md)
- [最佳实践](./best-practices.md)
