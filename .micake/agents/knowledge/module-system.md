# MiCake 模块系统指南

> 理解和使用 MiCake 的模块化架构

## 概述

MiCake 使用模块系统组织应用程序，每个模块是一个独立的功能单元，具有明确的生命周期和依赖管理。

---

## 1. 模块基础

### MiCakeModule 基类

```csharp
using MiCake.Core.Modularity;

/// <summary>
/// 应用程序入口模块
/// </summary>
[RelyOn(typeof(MiCakeAspNetCoreModule))]
public class MyAppModule : MiCakeModule
{
    /// <summary>
    /// 配置服务
    /// </summary>
    public override void ConfigureServices(ModuleConfigServiceContext context)
    {
        // 自动注册仓储
        context.AutoRegisterRepositories(typeof(MyAppModule).Assembly);
        
        // 注册应用服务
        context.Services.AddScoped<IOrderService, OrderService>();
        
        base.ConfigureServices(context);
    }
    
    /// <summary>
    /// 应用初始化
    /// </summary>
    public override void OnApplicationInitialization(ModuleInitializationContext context)
    {
        // 应用启动时的初始化逻辑
        base.OnApplicationInitialization(context);
    }
}
```

### 模块依赖声明

使用 `[RelyOn]` 属性声明模块依赖：

```csharp
// 依赖单个模块
[RelyOn(typeof(MiCakeAspNetCoreModule))]
public class MyAppModule : MiCakeModule { }

// 依赖多个模块
[RelyOn(typeof(MiCakeAspNetCoreModule), typeof(MySharedModule))]
public class MyAppModule : MiCakeModule { }
```

---

## 2. 模块生命周期

### 标准生命周期（MiCakeModule）

| 阶段 | 方法 | 说明 |
|------|------|------|
| 1 | `ConfigureServices()` | 配置依赖注入服务 |
| 2 | `OnApplicationInitialization()` | 应用初始化 |
| 3 | `OnApplicationShutdown()` | 应用关闭 |

### 高级生命周期（MiCakeModuleAdvanced）

```csharp
public class AdvancedModule : MiCakeModuleAdvanced
{
    // ========== 服务配置阶段 ==========
    
    public override void PreConfigureServices(ModuleConfigServiceContext context)
    {
        // 在所有模块 ConfigureServices 之前执行
        // 适用于：配置选项、注册早期服务
    }
    
    public override void ConfigureServices(ModuleConfigServiceContext context)
    {
        // 标准服务配置
    }
    
    public override void PostConfigureServices(ModuleConfigServiceContext context)
    {
        // 在所有模块 ConfigureServices 之后执行
        // 适用于：服务装饰、后置处理
    }
    
    // ========== 初始化阶段 ==========
    
    public override void PreInitialization(ModuleInitializationContext context)
    {
        // 在所有模块初始化之前执行
    }
    
    public override void OnApplicationInitialization(ModuleInitializationContext context)
    {
        // 标准初始化
    }
    
    public override void PostInitialization(ModuleInitializationContext context)
    {
        // 在所有模块初始化之后执行
    }
    
    // ========== 关闭阶段 ==========
    
    public override void PreShutdown(ModuleShutdownContext context)
    {
        // 在关闭之前执行
    }
    
    public override void OnApplicationShutdown(ModuleShutdownContext context)
    {
        // 标准关闭
    }
}
```

### 生命周期执行顺序

```
┌──────────────────────────────────────────────────────────────┐
│                    服务配置阶段                               │
├──────────────────────────────────────────────────────────────┤
│ PreConfigureServices (所有模块，按依赖顺序)                    │
│         ↓                                                    │
│ ConfigureServices (所有模块，按依赖顺序)                       │
│         ↓                                                    │
│ PostConfigureServices (所有模块，按依赖顺序)                   │
├──────────────────────────────────────────────────────────────┤
│                    初始化阶段                                 │
├──────────────────────────────────────────────────────────────┤
│ PreInitialization (所有模块，按依赖顺序)                       │
│         ↓                                                    │
│ OnApplicationInitialization (所有模块，按依赖顺序)             │
│         ↓                                                    │
│ PostInitialization (所有模块，按依赖顺序)                      │
├──────────────────────────────────────────────────────────────┤
│                    关闭阶段                                   │
├──────────────────────────────────────────────────────────────┤
│ PreShutdown (所有模块，逆序)                                  │
│         ↓                                                    │
│ OnApplicationShutdown (所有模块，逆序)                        │
└──────────────────────────────────────────────────────────────┘
```

---

## 3. 服务自动注册

### 通过标记接口注册

MiCake 支持通过标记接口自动注册服务：

```csharp
// 瞬态服务
public class MyService : IMyService, ITransientService
{
}

// 作用域服务
public class MyScopedService : IMyScopedService, IScopedService
{
}

// 单例服务
public class MySingletonService : IMySingletonService, ISingletonService
{
}
```

### 仓储自动注册

```csharp
public override void ConfigureServices(ModuleConfigServiceContext context)
{
    // 扫描程序集并注册所有仓储
    context.AutoRegisterRepositories(typeof(MyAppModule).Assembly);
    
    base.ConfigureServices(context);
}
```

---

## 4. 框架级模块

### 模块层次结构

```
MiCakeRootModule (根模块)
    ↓
MiCakeEssentialModule (核心功能)
    ↓
MiCakeEFCoreModule (EF Core 支持)
    ↓
MiCakeAspNetCoreModule (ASP.NET Core 支持)
    ↓
YourAppModule (应用模块)
```

### 框架模块说明

| 模块 | 职责 |
|------|------|
| `MiCakeRootModule` | 根模块，自动服务注册 |
| `MiCakeEssentialModule` | 领域元数据、工作单元、审计 |
| `MiCakeEFCoreModule` | EF Core 集成、仓储实现 |
| `MiCakeAspNetCoreModule` | ASP.NET Core 集成、API 日志 |

### IsFrameworkLevel 属性

```csharp
public class MyFrameworkModule : MiCakeModule
{
    // 标记为框架级模块
    // 框架级模块始终在普通模块之前加载
    public override bool IsFrameworkLevel => true;
}
```

---

## 5. 应用启动配置

### Program.cs 配置

```csharp
var builder = WebApplication.CreateBuilder(args);

// 添加 MiCake 服务
builder.Services.AddMiCake<MyAppModule>(options =>
{
    // 配置领域层程序集（用于扫描领域对象）
    options.DomainLayerAssemblies = new[] 
    { 
        typeof(MyAppModule).Assembly 
    };
})
.UseEFCore<MyDbContext>()  // 使用 EF Core
.UseAudit()                 // 启用审计
.Build();

var app = builder.Build();

// 启动 MiCake
app.StartMiCake();

// 配置中间件
app.UseRouting();
app.MapControllers();

app.Run();
```

### Startup.cs 配置（传统方式）

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        
        services.AddMiCake<MyAppModule>()
            .UseEFCore<MyDbContext>()
            .Build();
    }
    
    public void Configure(IApplicationBuilder app)
    {
        app.StartMiCake();
        
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
```

---

## 6. 模块间通信

### 通过依赖注入

```csharp
// 模块 A 提供的服务
public interface ISharedService
{
    Task DoWorkAsync();
}

// 模块 B 使用服务
public class ModuleBService
{
    private readonly ISharedService _sharedService;
    
    public ModuleBService(ISharedService sharedService)
    {
        _sharedService = sharedService;
    }
}
```

### 通过领域事件

```csharp
// 模块 A 发布事件
public class OrderAggregate : AggregateRoot<long>
{
    public void Complete()
    {
        Status = OrderStatus.Completed;
        AddDomainEvent(new OrderCompletedEvent(Id));
    }
}

// 模块 B 订阅事件
public class InventoryEventHandler : IDomainEventHandler<OrderCompletedEvent>
{
    public async Task HandleAsync(OrderCompletedEvent @event, CancellationToken cancellationToken)
    {
        // 更新库存
    }
}
```

---

## 7. 最佳实践

### ✅ 推荐做法

1. **显式声明依赖** - 始终使用 `[RelyOn]` 声明模块依赖
2. **单一职责** - 每个模块专注于一个功能领域
3. **最小化公共 API** - 只暴露必要的接口
4. **使用标记接口** - 利用自动注册减少样板代码

### ❌ 避免做法

1. 避免模块间循环依赖
2. 避免在模块中使用 `IServiceProvider` 解析服务
3. 避免在 `ConfigureServices` 中执行运行时逻辑
4. 避免跨模块直接引用具体实现

---

## 相关链接

- [DDD 模式指南](./ddd-patterns.md)
- [仓储模式](./repository-patterns.md)
- [最佳实践](./best-practices.md)
