# MiCake Troubleshooting Guide

> 常见问题排查和解决方案

## 目录

1. [启动和配置问题](#启动和配置问题)
2. [模块系统问题](#模块系统问题)
3. [仓储和数据访问问题](#仓储和数据访问问题)
4. [领域事件问题](#领域事件问题)
5. [EF Core 集成问题](#ef-core-集成问题)
6. [性能问题](#性能问题)

---

## 启动和配置问题

### 问题：应用启动时抛出 MiCake 初始化异常

**症状：**
```
System.InvalidOperationException: MiCake module has not been properly configured.
```

**原因：** 缺少必要的 MiCake 配置或配置顺序不正确。

**解决方案：**

1. 确保在 `Program.cs` 中正确配置 MiCake：

```csharp
// ✅ 正确配置
builder.Services.AddMiCakeWithContext<AppDbContext, AppModule>(options =>
{
    options.UseAudit();
});

// 在 app 配置中
app.StartMiCake();  // 必须在 UseEndpoints 之前
app.MapControllers();
```

2. 检查模块是否正确继承：

```csharp
// ✅ 正确
[RelyOn(typeof(MiCakeAspNetCoreModule))]
public class AppModule : MiCakeModule { }

// ❌ 错误 - 缺少依赖
public class AppModule : MiCakeModule { }
```

---

### 问题：找不到 StartMiCake 扩展方法

**症状：**
```
'IApplicationBuilder' does not contain a definition for 'StartMiCake'
```

**原因：** 缺少 `MiCake.AspNetCore` 包或 using 语句。

**解决方案：**

```csharp
// 1. 安装包
// dotnet add package MiCake.AspNetCore

// 2. 添加 using
using MiCake.AspNetCore;
```

---

## 模块系统问题

### 问题：模块依赖循环

**症状：**
```
System.InvalidOperationException: Circular dependency detected between modules: ModuleA -> ModuleB -> ModuleA
```

**原因：** 模块之间存在循环依赖关系。

**解决方案：**

1. 分析依赖关系：

```
ModuleA [RelyOn] ModuleB
ModuleB [RelyOn] ModuleA  ← 循环！
```

2. 重构为单向依赖或引入第三个模块：

```csharp
// 方案 1: 移除不必要的依赖
[RelyOn(typeof(MiCakeAspNetCoreModule))]
public class ModuleA : MiCakeModule { }

[RelyOn(typeof(MiCakeAspNetCoreModule))]
public class ModuleB : MiCakeModule { }

// 方案 2: 引入共享模块
[RelyOn(typeof(MiCakeAspNetCoreModule))]
public class SharedModule : MiCakeModule { }

[RelyOn(typeof(SharedModule))]
public class ModuleA : MiCakeModule { }

[RelyOn(typeof(SharedModule))]
public class ModuleB : MiCakeModule { }
```

---

### 问题：模块的 ConfigServices 未被调用

**症状：** 在模块中注册的服务无法解析。

**原因：**
1. 模块未被入口模块依赖
2. 模块未正确继承 MiCakeModule

**解决方案：**

```csharp
// 确保模块链完整
[RelyOn(typeof(MiCakeAspNetCoreModule))]
[RelyOn(typeof(FeatureModule))]  // 显式依赖
public class AppModule : MiCakeModule { }

// 或者在 FeatureModule 中依赖 AppModule
[RelyOn(typeof(AppModule))]
public class FeatureModule : MiCakeModule { }
```

---

## 仓储和数据访问问题

### 问题：无法解析 IRepository<TAggregate, TKey>

**症状：**
```
Unable to resolve service for type 'IRepository<Order, long>'
```

**原因：** 仓储未被注册。

**解决方案：**

```csharp
public override Task ConfigServices(ModuleConfigServiceContext context)
{
    // ✅ 自动注册仓储
    context.AutoRegisterRepositories(typeof(Order).Assembly);
    
    return base.ConfigServices(context);
}
```

---

### 问题：仓储操作不会持久化

**症状：** 调用 `AddAsync` 后数据未保存到数据库。

**原因：** 忘记调用 `SaveChangesAsync`。

**解决方案：**

```csharp
// ✅ 正确
await _repository.AddAsync(order);
await _repository.SaveChangesAsync();  // 必须调用！

// ❌ 错误 - 忘记保存
await _repository.AddAsync(order);
// 没有 SaveChangesAsync
```

---

### 问题：FindAsync 返回 null 但数据存在

**症状：** 数据库中有数据但查询返回 null。

**可能原因：**

1. **ID 类型不匹配：**
```csharp
// 实体定义使用 long
public class Order : AggregateRoot<long> { }

// 但查询时使用 int
await _repository.FindAsync(1);  // 1 是 int，会隐式转换

// ✅ 显式使用正确类型
await _repository.FindAsync(1L);  // 1L 是 long
```

2. **软删除过滤：** 如果使用了软删除，检查全局过滤器。

3. **不同的 DbContext 实例：** 确保使用同一个 Scope。

---

## 领域事件问题

### 问题：领域事件处理器未被调用

**症状：** `AddDomainEvent` 后事件处理器未执行。

**原因：**
1. 处理器未注册
2. 忘记调用 `SaveChangesAsync`
3. 处理器注册的事件类型不匹配

**解决方案：**

```csharp
// 1. 确保处理器已注册
public override Task ConfigServices(ModuleConfigServiceContext context)
{
    context.Services.AddScoped<IDomainEventHandler<OrderCreatedEvent>, OrderCreatedEventHandler>();
    return base.ConfigServices(context);
}

// 2. 确保调用 SaveChangesAsync
order.DoSomething();  // 内部调用 AddDomainEvent
await _repository.SaveChangesAsync();  // 事件在此时分发

// 3. 确保事件类型匹配
public class OrderCreatedEventHandler : IDomainEventHandler<OrderCreatedEvent>  // 类型必须完全匹配
{
    public async Task HandleAsync(OrderCreatedEvent domainEvent, CancellationToken ct)
    {
        // ...
    }
}
```

---

### 问题：领域事件处理器中的异常导致主操作回滚

**症状：** 事件处理器抛出异常，整个事务回滚。

**原因：** 默认情况下，领域事件在同一事务中处理。

**解决方案：**

```csharp
// 方案 1: 在处理器中捕获异常
public async Task HandleAsync(OrderCreatedEvent domainEvent, CancellationToken ct)
{
    try
    {
        await _emailService.SendConfirmationAsync(...);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to send confirmation email");
        // 不重新抛出，允许主操作完成
    }
}

// 方案 2: 使用集成事件进行异步处理
// 在领域事件处理器中发布集成事件到消息队列
public async Task HandleAsync(OrderCreatedEvent domainEvent, CancellationToken ct)
{
    await _messageBus.PublishAsync(new SendOrderConfirmationEmailCommand(domainEvent.OrderId));
}
```

---

## EF Core 集成问题

### 问题：DDD 实体配置未生效

**症状：** 值对象未正确映射、聚合根 ID 配置不正确。

**原因：** DbContext 中未调用 `base.OnModelCreating`。

**解决方案：**

```csharp
public class AppDbContext : MiCakeDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ✅ 必须调用基类方法
        base.OnModelCreating(modelBuilder);
        
        // 然后添加你的配置
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
```

---

### 问题：值对象映射失败

**症状：**
```
The entity type 'Money' requires a primary key to be defined.
```

**原因：** 值对象被错误地配置为实体。

**解决方案：**

```csharp
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // ✅ 使用 OwnsOne 映射值对象
        builder.OwnsOne(o => o.TotalAmount, money =>
        {
            money.Property(m => m.Amount).HasColumnName("TotalAmount");
            money.Property(m => m.Currency).HasColumnName("Currency");
        });
    }
}
```

---

### 问题：子实体集合未正确加载

**症状：** 聚合根的子实体集合为空。

**原因：** EF Core 延迟加载未配置或未使用 Include。

**解决方案：**

```csharp
// 方案 1: 显式 Include
public async Task<Order?> GetWithItemsAsync(long orderId)
{
    return await DbSet
        .Include(o => o.Items)
        .FirstOrDefaultAsync(o => o.Id == orderId);
}

// 方案 2: 在仓储中自动 Include（推荐）
public class OrderRepository : EFRepository<Order, long, AppDbContext>
{
    public override async Task<Order?> FindAsync(long id, CancellationToken ct = default)
    {
        return await DbSet
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }
}
```

---

## 性能问题

### 问题：N+1 查询问题

**症状：** 页面加载缓慢，日志中有大量相似的 SQL 查询。

**诊断：**
```sql
-- N+1 问题的典型模式
SELECT * FROM Orders WHERE Id = 1
SELECT * FROM OrderItems WHERE OrderId = 1
SELECT * FROM Orders WHERE Id = 2
SELECT * FROM OrderItems WHERE OrderId = 2
-- ... 重复 N 次
```

**解决方案：**

```csharp
// ❌ N+1 问题
var orders = await _repository.GetAllAsync();
foreach (var order in orders)
{
    // 每次循环都会执行一个新查询
    var items = order.Items;  // 延迟加载
}

// ✅ 使用 Include 一次性加载
var orders = await DbSet
    .Include(o => o.Items)
    .ToListAsync();
```

---

### 问题：大量数据导致内存溢出

**症状：** 查询大量数据时内存使用飙升。

**解决方案：**

```csharp
// ❌ 一次加载全部
var allOrders = await _repository.GetAllAsync();

// ✅ 分页查询
var pagedOrders = await DbSet
    .OrderByDescending(o => o.OrderDate)
    .Skip(pageIndex * pageSize)
    .Take(pageSize)
    .ToListAsync();

// ✅ 使用 AsNoTracking 进行只读查询
var orders = await DbSet
    .AsNoTracking()  // 不跟踪变更，减少内存使用
    .ToListAsync();
```

---

## 快速诊断清单

当遇到问题时，按此清单检查：

### 启动问题
- [ ] `AddMiCakeWithContext` 是否在 `builder.Services` 中调用？
- [ ] `StartMiCake()` 是否在 `app.MapControllers()` 之前调用？
- [ ] 入口模块是否正确配置 `[RelyOn(typeof(MiCakeAspNetCoreModule))]`？

### 依赖注入问题
- [ ] 服务是否在模块的 `ConfigServices` 中注册？
- [ ] 模块依赖链是否完整？
- [ ] 是否存在循环依赖？

### 数据访问问题
- [ ] 是否调用了 `context.AutoRegisterRepositories()`？
- [ ] 是否调用了 `SaveChangesAsync()`？
- [ ] DbContext 是否继承自 `MiCakeDbContext`？
- [ ] `OnModelCreating` 中是否调用了 `base.OnModelCreating()`？

### 领域事件问题
- [ ] 事件处理器是否已注册？
- [ ] 事件类型是否匹配？
- [ ] 是否在 `SaveChangesAsync()` 后期望事件执行？

---

## 获取帮助

如果以上方案未能解决问题：

1. **查看日志：** 设置 `MiCake` 日志级别为 `Debug`
2. **GitHub Issues：** 在 [MiCake GitHub](https://github.com/MiCake/MiCake) 提交 Issue
3. **示例项目：** 参考 `samples/BaseMiCakeApplication` 中的正确用法
