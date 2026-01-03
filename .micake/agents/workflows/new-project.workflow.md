---
name: "new-project"
description: "Initialize a new MiCake DDD project from scratch"
---

# New Project Workflow

> 从零开始初始化一个 MiCake DDD 项目

## 概述

本工作流指导用户创建一个新的基于 MiCake 框架的 DDD 项目，包括解决方案结构、项目创建、依赖配置和基础代码生成。

## 工作流步骤

```
┌─────────────────────────────────────────────────────────────────┐
│                    New Project Workflow                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  Step 1: Project Configuration                                  │
│  ┌─────────────────────────────────────────────────┐            │
│  │ • 收集项目信息（名称、命名空间等）               │            │
│  │ • 创建 .micake/config.yaml                      │            │
│  │ • 设置用户偏好                                   │            │
│  └─────────────────────────────────────────────────┘            │
│                          ↓                                      │
│  Step 2: Solution Structure                                     │
│  ┌─────────────────────────────────────────────────┐            │
│  │ • 创建解决方案文件                               │            │
│  │ • 创建分层项目结构                               │            │
│  │ • 配置项目引用                                   │            │
│  └─────────────────────────────────────────────────┘            │
│                          ↓                                      │
│  Step 3: Dependencies Installation                              │
│  ┌─────────────────────────────────────────────────┐            │
│  │ • 安装 MiCake NuGet 包                           │            │
│  │ • 安装 EF Core 相关包                            │            │
│  │ • 配置包版本                                     │            │
│  └─────────────────────────────────────────────────┘            │
│                          ↓                                      │
│  Step 4: Base Code Generation                                   │
│  ┌─────────────────────────────────────────────────┐            │
│  │ • 生成入口模块                                   │            │
│  │ • 生成 DbContext                                 │            │
│  │ • 配置 Startup/Program.cs                        │            │
│  │ • 生成示例聚合                                   │            │
│  └─────────────────────────────────────────────────┘            │
│                          ↓                                      │
│  Step 5: Verification                                           │
│  ┌─────────────────────────────────────────────────┐            │
│  │ • 编译验证                                       │            │
│  │ • 运行测试                                       │            │
│  │ • 输出后续指导                                   │            │
│  └─────────────────────────────────────────────────┘            │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## Step 1: Project Configuration

**Agent**: Sage 🧙

### 收集项目信息

```yaml
project_info:
  name: "请输入项目名称 (e.g., MyShop)"
  namespace: "请输入根命名空间 (e.g., MyShop)"
  description: "项目简要描述"
  target_framework: "net8.0" # 默认值
  
database:
  provider: "SqlServer | PostgreSQL | SQLite | InMemory"
  connection_name: "DefaultConnection"
  
preferences:
  use_sample_code: true # 是否生成示例代码
```

### 创建配置文件

```bash
# 创建配置目录
mkdir .micake
mkdir .micake/requirements
```

生成 `.micake/config.yaml`:

```yaml
# MiCake Agent Configuration
# Generated by MiCake Agent System

project:
  name: "{{project_name}}"
  namespace: "{{namespace}}"
  description: "{{description}}"
  target_framework: "{{target_framework}}"
  created_at: "{{timestamp}}"

ddd:
  prefer_domain_events: true
  use_static_factory_methods: true
  use_private_setters: true
  aggregate_id_type: "long"
  domain_event_naming: "past-tense"

code_style:
  use_csharp_12_features: true
  generate_xml_docs: true
  use_nullable_reference_types: true
  use_file_scoped_namespaces: true

database:
  provider: "{{db_provider}}"
  connection_name: "{{connection_name}}"

repository:
  auto_register: true
  generate_interfaces: true

external_requirements:
  folder: ".micake/requirements"
  watch_for_changes: true
```

---

## Step 2: Solution Structure

**Agent**: Baker 👨‍🍳

### 创建解决方案

```bash
dotnet new sln -n {{ProjectName}}
```

### 创建项目结构

```
{{ProjectName}}/
├── {{ProjectName}}.sln
├── .micake/
│   ├── config.yaml
│   └── requirements/
├── src/
│   ├── {{ProjectName}}.Domain/                # 领域层
│   │   ├── {{ProjectName}}.Domain.csproj
│   │   ├── Aggregates/
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   ├── Events/
│   │   ├── Repositories/
│   │   └── Services/
│   │
│   ├── {{ProjectName}}.Application/          # 应用层
│   │   ├── {{ProjectName}}.Application.csproj
│   │   ├── Services/
│   │   ├── DTOs/
│   │   └── Contracts/
│   │
│   ├── {{ProjectName}}.Infrastructure/       # 基础设施层
│   │   ├── {{ProjectName}}.Infrastructure.csproj
│   │   ├── Repositories/
│   │   ├── Configurations/
│   │   └── {{ProjectName}}DbContext.cs
│   │
│   └── {{ProjectName}}.Api/                  # 表示层
│       ├── {{ProjectName}}.Api.csproj
│       ├── Controllers/
│       ├── Program.cs
│       ├── appsettings.json
│       └── {{ProjectName}}Module.cs
│
└── tests/
    └── {{ProjectName}}.Tests/
        └── {{ProjectName}}.Tests.csproj
```

### 项目创建命令

```bash
# 创建各层项目
dotnet new classlib -n {{ProjectName}}.Domain -o src/{{ProjectName}}.Domain
dotnet new classlib -n {{ProjectName}}.Application -o src/{{ProjectName}}.Application
dotnet new classlib -n {{ProjectName}}.Infrastructure -o src/{{ProjectName}}.Infrastructure
dotnet new webapi -n {{ProjectName}}.Api -o src/{{ProjectName}}.Api
dotnet new xunit -n {{ProjectName}}.Tests -o tests/{{ProjectName}}.Tests

# 添加到解决方案
dotnet sln add src/{{ProjectName}}.Domain
dotnet sln add src/{{ProjectName}}.Application
dotnet sln add src/{{ProjectName}}.Infrastructure
dotnet sln add src/{{ProjectName}}.Api
dotnet sln add tests/{{ProjectName}}.Tests

# 配置项目引用 (依赖方向: Api → Application → Domain, Infrastructure → Domain)
dotnet add src/{{ProjectName}}.Application reference src/{{ProjectName}}.Domain
dotnet add src/{{ProjectName}}.Infrastructure reference src/{{ProjectName}}.Domain
dotnet add src/{{ProjectName}}.Api reference src/{{ProjectName}}.Application
dotnet add src/{{ProjectName}}.Api reference src/{{ProjectName}}.Infrastructure
dotnet add tests/{{ProjectName}}.Tests reference src/{{ProjectName}}.Domain
dotnet add tests/{{ProjectName}}.Tests reference src/{{ProjectName}}.Application
```

---

## Step 3: Dependencies Installation

### NuGet 包安装

```bash
# Domain 层 - 只需要 MiCake 核心
dotnet add src/{{ProjectName}}.Domain package MiCake

# Infrastructure 层 - EF Core 集成
dotnet add src/{{ProjectName}}.Infrastructure package MiCake.EntityFrameworkCore
dotnet add src/{{ProjectName}}.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer  # 或其他提供商

# Api 层 - ASP.NET Core 集成
dotnet add src/{{ProjectName}}.Api package MiCake.AspNetCore

# 测试项目
dotnet add tests/{{ProjectName}}.Tests package Microsoft.EntityFrameworkCore.InMemory
dotnet add tests/{{ProjectName}}.Tests package Moq
dotnet add tests/{{ProjectName}}.Tests package FluentAssertions
```

---

## Step 4: Base Code Generation

### 生成入口模块

**文件**: `src/{{ProjectName}}.Api/{{ProjectName}}Module.cs`

```csharp
using MiCake;
using MiCake.AspNetCore;
using MiCake.Modularity;

namespace {{ProjectName}}.Api;

/// <summary>
/// MiCake entry module for {{ProjectName}}.
/// </summary>
[RelyOn(typeof(MiCakeAspNetCoreModule))]
public class {{ProjectName}}Module : MiCakeModule
{
    public override Task ConfigServices(ModuleConfigServiceContext context)
    {
        // Auto-register repositories from Domain assembly
        context.AutoRegisterRepositories(typeof({{ProjectName}}.Domain.AssemblyMarker).Assembly);
        
        return base.ConfigServices(context);
    }
    
    public override Task Initialization(ModuleLoadContext context)
    {
        // Module initialization logic
        return base.Initialization(context);
    }
}
```

### 生成 DbContext

**文件**: `src/{{ProjectName}}.Infrastructure/{{ProjectName}}DbContext.cs`

```csharp
using MiCake.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using {{ProjectName}}.Domain.Aggregates;

namespace {{ProjectName}}.Infrastructure;

/// <summary>
/// Main database context for {{ProjectName}}.
/// </summary>
public class {{ProjectName}}DbContext : MiCakeDbContext
{
    public {{ProjectName}}DbContext(DbContextOptions<{{ProjectName}}DbContext> options)
        : base(options)
    {
    }
    
    // Add DbSets for your aggregates here
    // public DbSet<Order> Orders => Set<Order>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Required for MiCake DDD entity configuration
        
        // Apply all configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof({{ProjectName}}DbContext).Assembly);
    }
}
```

### 配置 Program.cs

**文件**: `src/{{ProjectName}}.Api/Program.cs`

```csharp
using MiCake;
using MiCake.AspNetCore;
using {{ProjectName}}.Api;
using {{ProjectName}}.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
builder.Services.AddDbContext<{{ProjectName}}DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure MiCake
builder.Services.AddMiCakeWithContext<{{ProjectName}}DbContext, {{ProjectName}}Module>(miCake =>
{
    miCake.UseAudit();  // Enable audit support
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Start MiCake - must be before UseEndpoints
app.StartMiCake();

app.MapControllers();
app.Run();
```

### 配置 appsettings.json

**文件**: `src/{{ProjectName}}.Api/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database={{ProjectName}};Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "MiCake": "Debug"
    }
  },
  "AllowedHosts": "*"
}
```

### 生成程序集标记

**文件**: `src/{{ProjectName}}.Domain/AssemblyMarker.cs`

```csharp
namespace {{ProjectName}}.Domain;

/// <summary>
/// Marker class for assembly scanning.
/// </summary>
public sealed class AssemblyMarker { }
```

---

## Step 5: Verification

### 编译验证

```bash
dotnet build
```

### 检查清单

- [ ] 解决方案编译成功
- [ ] 无循环引用
- [ ] MiCake 包已正确安装
- [ ] DbContext 继承自 MiCakeDbContext
- [ ] 入口模块标记了正确的依赖

### 后续指导

```markdown
# 项目创建成功！🎉

## 接下来的步骤

1. **创建第一个聚合**
   ```
   @MiCake create-aggregate
   ```

2. **添加迁移**
   ```bash
   dotnet ef migrations add Initial -p src/{{ProjectName}}.Infrastructure -s src/{{ProjectName}}.Api
   ```

3. **运行项目**
   ```bash
   dotnet run --project src/{{ProjectName}}.Api
   ```

4. **添加需求文档**
   将 PRD 或 User Story 放入 `.micake/requirements/` 目录，然后运行:
   ```
   @MiCake prd-to-code
   ```

## 有用的命令

| 命令 | 描述 |
|------|------|
| `@MiCake create-aggregate` | 创建新聚合 |
| `@MiCake create-entity` | 创建新实体 |
| `@MiCake review` | 代码审查 |
| `@MiCake help` | 显示帮助 |
```

---

## 示例交互

**User:** @MiCake 帮我创建一个新的 MiCake 项目，名称叫 EShop

**Sage:** 🧙 好的！让我来帮你初始化 EShop 项目。首先需要了解一些配置：

| 配置项 | 默认值 | 你的选择 |
|--------|-------|---------|
| 项目命名空间 | EShop | |
| 目标框架 | net8.0 | |
| 数据库提供商 | SqlServer | |
| 生成示例代码 | 是 | |

请确认或修改这些配置。

**User:** 都用默认的

**Baker:** 👨‍🍳 开始创建项目结构...

```
✅ 创建解决方案: EShop.sln
✅ 创建 EShop.Domain 项目
✅ 创建 EShop.Application 项目
✅ 创建 EShop.Infrastructure 项目
✅ 创建 EShop.Api 项目
✅ 配置项目引用
✅ 安装 NuGet 包
✅ 生成入口模块
✅ 生成 DbContext
✅ 配置 Program.cs
```

**Baker:** 项目创建完成！运行 `dotnet build` 验证...

**Inspector:** 🔍 验证结果:
- ✅ 编译成功
- ✅ 结构符合 DDD 分层
- ✅ MiCake 配置正确

项目已就绪！你可以使用 `@MiCake create-aggregate` 开始创建你的第一个聚合。
