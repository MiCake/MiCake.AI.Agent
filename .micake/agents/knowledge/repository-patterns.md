# Repository Patterns in MiCake

> MiCake 仓储模式的详细指南

## 概述

MiCake 提供了强大的仓储抽象，让你可以专注于业务逻辑而非数据访问细节。本指南涵盖仓储的核心概念、使用模式和最佳实践。

## 仓储架构

```
┌─────────────────────────────────────────────────────────────────┐
│                    Repository Architecture                       │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  Domain Layer                                                   │
│  ┌─────────────────────────────────────────────────┐            │
│  │  IRepository<TAggregateRoot, TKey>              │            │
│  │  └── I{Aggregate}Repository (自定义接口)         │            │
│  └─────────────────────────────────────────────────┘            │
│                          ↑ 依赖于                               │
│  Infrastructure Layer                                           │
│  ┌─────────────────────────────────────────────────┐            │
│  │  EFRepository<TAggregateRoot, TKey, TDbContext> │            │
│  │  └── {Aggregate}Repository (自定义实现)          │            │
│  └─────────────────────────────────────────────────┘            │
│                          ↑ 使用                                 │
│  Framework Layer                                                │
│  ┌─────────────────────────────────────────────────┐            │
│  │  IRepositoryFactory                             │            │
│  │  IRepositoryProvider                            │            │
│  └─────────────────────────────────────────────────┘            │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

## 核心接口

### IRepository<TAggregateRoot, TKey>

MiCake 的基础仓储接口，提供 CRUD 操作：

```csharp
public interface IRepository<TAggregateRoot, TKey>
    where TAggregateRoot : class, IAggregateRoot<TKey>
{
    // 查询
    Task<TAggregateRoot?> FindAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TAggregateRoot> GetAsync(TKey id, CancellationToken cancellationToken = default);
    
    // 命令
    Task AddAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default);
    Task UpdateAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default);
    Task DeleteAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default);
    
    // 持久化
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

### IReadOnlyRepository<TAggregateRoot, TKey>

只读仓储，适用于只需查询的场景：

```csharp
public interface IReadOnlyRepository<TAggregateRoot, TKey>
    where TAggregateRoot : class, IAggregateRoot<TKey>
{
    Task<TAggregateRoot?> FindAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TAggregateRoot> GetAsync(TKey id, CancellationToken cancellationToken = default);
}
```

## 仓储注册

### 自动注册（推荐）

在模块的 `ConfigServices` 中使用：

```csharp
public override Task ConfigServices(ModuleConfigServiceContext context)
{
    // 自动扫描并注册所有仓储
    context.AutoRegisterRepositories(typeof(MyDomainAssemblyMarker).Assembly);
    
    return base.ConfigServices(context);
}
```

**自动注册的工作原理：**
1. 扫描指定程序集中的聚合根
2. 为每个聚合根创建 `IRepository<TAggregateRoot, TKey>` 注册
3. 如果发现自定义接口（如 `IOrderRepository`），也会自动注册

### 手动注册

```csharp
services.AddScoped<IOrderRepository, OrderRepository>();
```

## 自定义仓储

### 定义接口（Domain 层）

```csharp
// Domain/Repositories/IOrderRepository.cs
public interface IOrderRepository : IRepository<Order, long>
{
    Task<IReadOnlyList<Order>> GetByCustomerIdAsync(
        long customerId, 
        CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<Order>> GetPendingOrdersAsync(
        CancellationToken cancellationToken = default);
    
    Task<bool> ExistsActiveOrderForProductAsync(
        long productId, 
        CancellationToken cancellationToken = default);
}
```

### 实现接口（Infrastructure 层）

```csharp
// Infrastructure/Repositories/OrderRepository.cs
public class OrderRepository : EFRepository<Order, long, AppDbContext>, IOrderRepository
{
    public OrderRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<IReadOnlyList<Order>> GetByCustomerIdAsync(
        long customerId, 
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.Items)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IReadOnlyList<Order>> GetPendingOrdersAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(o => o.Status == OrderStatus.Pending)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<bool> ExistsActiveOrderForProductAsync(
        long productId, 
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AnyAsync(o => 
                o.Items.Any(i => i.ProductId == productId) && 
                o.Status != OrderStatus.Cancelled,
                cancellationToken);
    }
}
```

## 查询模式

### 单个聚合查询

```csharp
// 获取，如果不存在则返回 null
var order = await _orderRepository.FindAsync(orderId);
if (order == null)
{
    // 处理不存在的情况
}

// 获取，如果不存在则抛出异常
var order = await _orderRepository.GetAsync(orderId);
// 如果不存在，抛出 EntityNotFoundException
```

### 列表查询

```csharp
// 按条件查询
public async Task<IReadOnlyList<Order>> GetByStatusAsync(
    OrderStatus status,
    CancellationToken cancellationToken = default)
{
    return await DbSet
        .Where(o => o.Status == status)
        .ToListAsync(cancellationToken);
}

// 分页查询
public async Task<(IReadOnlyList<Order> Items, int TotalCount)> GetPagedAsync(
    int pageIndex,
    int pageSize,
    CancellationToken cancellationToken = default)
{
    var query = DbSet.OrderByDescending(o => o.OrderDate);
    
    var totalCount = await query.CountAsync(cancellationToken);
    
    var items = await query
        .Skip(pageIndex * pageSize)
        .Take(pageSize)
        .ToListAsync(cancellationToken);
    
    return (items, totalCount);
}
```

### 包含关联数据

```csharp
// 显式加载关联
public async Task<Order?> GetWithItemsAsync(
    long orderId,
    CancellationToken cancellationToken = default)
{
    return await DbSet
        .Include(o => o.Items)
        .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
}

// 多层关联
public async Task<Order?> GetFullOrderAsync(
    long orderId,
    CancellationToken cancellationToken = default)
{
    return await DbSet
        .Include(o => o.Items)
            .ThenInclude(i => i.Product)
        .Include(o => o.ShippingAddress)
        .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
}
```

## 命令模式

### 添加聚合

```csharp
public async Task CreateOrderAsync(CreateOrderCommand command)
{
    var order = Order.Create(
        command.CustomerId,
        new Money(0, "USD")
    );
    
    foreach (var item in command.Items)
    {
        order.AddItem(OrderItem.Create(
            item.ProductId,
            item.ProductName,
            new Money(item.Price, "USD"),
            item.Quantity
        ));
    }
    
    await _orderRepository.AddAsync(order);
    await _orderRepository.SaveChangesAsync();
    // 领域事件会在 SaveChangesAsync 时自动分发
}
```

### 更新聚合

```csharp
public async Task ConfirmOrderAsync(long orderId)
{
    var order = await _orderRepository.GetAsync(orderId);
    
    order.Confirm(); // 领域方法，会添加领域事件
    
    await _orderRepository.UpdateAsync(order);
    await _orderRepository.SaveChangesAsync();
}
```

### 删除聚合

```csharp
public async Task CancelOrderAsync(long orderId)
{
    var order = await _orderRepository.GetAsync(orderId);
    
    order.Cancel(); // 可能添加 OrderCancelledEvent
    
    // 软删除模式 - 只改变状态
    await _orderRepository.UpdateAsync(order);
    
    // 或硬删除
    // await _orderRepository.DeleteAsync(order);
    
    await _orderRepository.SaveChangesAsync();
}
```

## 领域事件分发

MiCake 在 `SaveChangesAsync` 时自动收集和分发领域事件：

```csharp
// 在聚合中
public void Confirm()
{
    if (Status != OrderStatus.Pending)
        throw new BusinessException("Only pending orders can be confirmed");
    
    Status = OrderStatus.Confirmed;
    
    // 添加领域事件
    AddDomainEvent(new OrderConfirmedEvent(Id));
}

// 在服务中
await _orderRepository.SaveChangesAsync();
// 此时 OrderConfirmedEvent 会被自动分发到所有处理器
```

## 最佳实践

### ✅ 推荐做法

```csharp
// 1. 仓储只操作聚合根
public interface IOrderRepository : IRepository<Order, long> { }
// 不要创建 IOrderItemRepository

// 2. 返回 IReadOnlyList 而非 IQueryable
Task<IReadOnlyList<Order>> GetPendingOrdersAsync();
// 不暴露 IQueryable<Order> GetPendingOrders();

// 3. 使用 CancellationToken
Task<Order?> FindAsync(long id, CancellationToken cancellationToken = default);

// 4. 方法名清晰表达意图
Task<IReadOnlyList<Order>> GetRecentOrdersByCustomerAsync(long customerId);
// 而非 GetOrders(long? customerId = null, DateTime? from = null, ...)

// 5. 复杂查询使用独立方法
Task<IReadOnlyList<Order>> GetOrdersAwaitingShipmentAsync();
```

### ❌ 避免做法

```csharp
// 1. 不要在仓储中放业务逻辑
public async Task ConfirmOrderAsync(long orderId)  // ❌ 应该在 Application Service
{
    var order = await FindAsync(orderId);
    order.Status = OrderStatus.Confirmed;  // ❌ 直接修改，绕过领域方法
    await SaveChangesAsync();
}

// 2. 不要暴露 DbContext 或 DbSet
public DbSet<Order> Orders => _dbContext.Orders;  // ❌

// 3. 不要返回 IQueryable
public IQueryable<Order> GetOrders() => DbSet;  // ❌

// 4. 不要在一个仓储方法中修改多个聚合
public async Task TransferItemsAsync(long fromOrderId, long toOrderId)  // ❌
{
    // 这应该在 Application Service 中处理
}
```

## 测试仓储

### 使用内存数据库

```csharp
public class OrderRepositoryTests
{
    private readonly AppDbContext _dbContext;
    private readonly OrderRepository _repository;
    
    public OrderRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _dbContext = new AppDbContext(options);
        _repository = new OrderRepository(_dbContext);
    }
    
    [Fact]
    public async Task GetByCustomerId_ReturnsMatchingOrders()
    {
        // Arrange
        var order1 = Order.Create(customerId: 1, new Money(100, "USD"));
        var order2 = Order.Create(customerId: 1, new Money(200, "USD"));
        var order3 = Order.Create(customerId: 2, new Money(300, "USD"));
        
        await _repository.AddAsync(order1);
        await _repository.AddAsync(order2);
        await _repository.AddAsync(order3);
        await _repository.SaveChangesAsync();
        
        // Act
        var result = await _repository.GetByCustomerIdAsync(1);
        
        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(o => o.CustomerId.Should().Be(1));
    }
}
```

## 相关资源

- [DDD Patterns](ddd-patterns.md) - 领域驱动设计模式
- [Module System](module-system.md) - 模块系统和仓储注册
- [Best Practices](best-practices.md) - 开发最佳实践
