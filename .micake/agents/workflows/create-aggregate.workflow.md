---
name: "create-aggregate"
description: "Step-by-step workflow to create a new aggregate with all related components"
---

# Create Aggregate Workflow

> 创建新聚合的完整工作流

## 概述

本工作流指导用户完成创建一个新聚合所需的所有步骤，包括聚合根、子实体、值对象、仓储、领域事件和 EF Core 配置。

## 工作流步骤

```
┌─────────────────────────────────────────────────────────────────┐
│                  Create Aggregate Workflow                      │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  Step 1: Gather Requirements                                    │
│  ┌─────────────────────────────────────────────────┐            │
│  │ • 聚合名称和用途                                 │            │
│  │ • 属性和关系                                     │            │
│  │ • 业务规则和不变量                               │            │
│  └─────────────────────────────────────────────────┘            │
│                          ↓                                      │
│  Step 2: Design Review                                          │
│  ┌─────────────────────────────────────────────────┐            │
│  │ • 确认聚合边界                                   │            │
│  │ • 验证设计决策                                   │            │
│  │ • 用户确认                                       │            │
│  └─────────────────────────────────────────────────┘            │
│                          ↓                                      │
│  Step 3: Generate Code                                          │
│  ┌─────────────────────────────────────────────────┐            │
│  │ • 聚合根类                                       │            │
│  │ • 子实体类                                       │            │
│  │ • 值对象类                                       │            │
│  │ • 仓储接口                                       │            │
│  │ • 仓储实现                                       │            │
│  │ • 领域事件                                       │            │
│  │ • EF Core 配置                                   │            │
│  └─────────────────────────────────────────────────┘            │
│                          ↓                                      │
│  Step 4: Register & Validate                                    │
│  ┌─────────────────────────────────────────────────┐            │
│  │ • 更新 DbContext                                 │            │
│  │ • 编译验证                                       │            │
│  │ • 生成迁移提示                                   │            │
│  └─────────────────────────────────────────────────┘            │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## Step 1: Gather Requirements

**Agent**: Sage 🧙 + Architect 🏗️

### 信息收集表

```yaml
aggregate:
  name: "聚合名称 (e.g., Order, Product)"
  purpose: "这个聚合代表什么业务概念？"
  
identity:
  id_type: "long | Guid | int | string"
  id_generation: "database | application | manual"
  
properties:
  - name: "属性名"
    type: "类型"
    required: true/false
    description: "描述"

child_entities:
  - name: "子实体名"
    relationship: "一对多 | 一对一"
    properties:
      - name: "属性名"
        type: "类型"

value_objects:
  - name: "值对象名"
    components:
      - name: "组件名"
        type: "类型"

business_rules:
  - "规则 1"
  - "规则 2"

domain_events:
  - trigger: "当...时"
    event: "...事件"
```

### 快速提问模板

```
🧙 让我了解你要创建的聚合：

1️⃣ **聚合名称是什么？** (如: Order, Product, User)

2️⃣ **它包含哪些主要属性？**

3️⃣ **它需要包含子实体吗？** (如: Order 包含 OrderItem)

4️⃣ **有哪些业务规则必须保证？** (如: 订单总金额不能为负)

5️⃣ **需要在什么时候发布领域事件？** (如: 创建订单时)
```

---

## Step 2: Design Review

**Agent**: Architect 🏗️

### 设计确认模板

```markdown
## 聚合设计确认: {{AggregateName}}

### 结构预览

```
{{AggregateName}} (聚合根)
├── Id: {{IdType}}
├── {{Property1}}: {{Type1}}
├── {{Property2}}: {{Type2}}
├── {{ChildEntity}}: List<{{ChildEntityType}}>
│   ├── Id: int
│   ├── {{ChildProp1}}: {{Type}}
│   └── {{ChildProp2}}: {{Type}}
└── {{ValueObject}}: {{ValueObjectType}}
    ├── {{Component1}}: {{Type}}
    └── {{Component2}}: {{Type}}
```

### 不变量（业务规则）
1. {{Invariant1}}
2. {{Invariant2}}

### 领域事件
| 事件 | 触发时机 |
|------|---------|
| {{AggregateName}}CreatedEvent | 聚合创建时 |
| {{Event2}} | {{Trigger2}} |

### 将生成的文件
- [ ] `Domain/Aggregates/{{AggregateName}}.cs`
- [ ] `Domain/Aggregates/{{ChildEntity}}.cs`
- [ ] `Domain/ValueObjects/{{ValueObject}}.cs`
- [ ] `Domain/Repositories/I{{AggregateName}}Repository.cs`
- [ ] `Domain/Events/{{AggregateName}}CreatedEvent.cs`
- [ ] `Infrastructure/Repositories/{{AggregateName}}Repository.cs`
- [ ] `Infrastructure/Configurations/{{AggregateName}}Configuration.cs`

**确认生成？** (yes/no/修改)
```

---

## Step 3: Generate Code

**Agent**: Baker 👨‍🍳

### 3.1 聚合根生成

读取用户偏好后生成聚合根：

```csharp
// 根据 config.yaml 中的偏好生成
// use_static_factory_methods: true → 使用静态工厂方法
// use_private_setters: true → 使用私有 setter
// generate_xml_docs: true → 生成 XML 文档
```

**模板文件**: 参见 `.github/agents/templates/aggregate.template.cs`

### 3.2 子实体生成

**模板文件**: 参见 `.github/agents/templates/entity.template.cs`

### 3.3 值对象生成

**模板文件**: 参见 `.github/agents/templates/value-object.template.cs`

### 3.4 仓储生成

**接口模板**: 参见 `.github/agents/templates/repository-interface.template.cs`
**实现模板**: 参见 `.github/agents/templates/repository-impl.template.cs`

### 3.5 领域事件生成

**模板文件**: 参见 `.github/agents/templates/domain-event.template.cs`

### 3.6 EF Core 配置生成

**模板文件**: 参见 `.github/agents/templates/ef-configuration.template.cs`

---

## Step 4: Register & Validate

**Agent**: Baker 👨‍🍳 + Inspector 🔍

### 更新 DbContext

```csharp
// 添加到 DbContext
public DbSet<{{AggregateName}}> {{AggregateName}}s => Set<{{AggregateName}}>();
```

### 编译验证

```bash
dotnet build
```

### 验证清单

- [ ] 聚合根继承自 `AggregateRoot<TKey>`
- [ ] 子实体继承自 `Entity<TKey>` 或 `Entity`
- [ ] 值对象继承自 `ValueObject` 或使用 `RecordValueObject`
- [ ] 仓储接口在 Domain 层
- [ ] 仓储实现在 Infrastructure 层
- [ ] EF 配置正确映射
- [ ] 编译无错误

### 输出后续步骤

```markdown
## ✅ 聚合创建完成！

### 生成的文件
| 文件 | 状态 |
|------|------|
| {{file1}} | ✅ |
| {{file2}} | ✅ |

### 下一步
1. **添加迁移**
   ```bash
   dotnet ef migrations add Add{{AggregateName}}
   ```

2. **更新数据库**
   ```bash
   dotnet ef database update
   ```

3. **创建 API 控制器** (可选)
   ```
   @MiCake create-controller {{AggregateName}}
   ```
```

---

## 完整示例交互

**User:** @MiCake create-aggregate 我需要创建一个 Order 聚合

**Sage:** 🧙 好的，让我了解一下 Order 聚合的详情：

1️⃣ **Order 包含哪些属性？**
   - 客户信息？
   - 下单时间？
   - 状态？

2️⃣ **需要子实体吗？** (如 OrderItem)

3️⃣ **有什么业务规则？**

**User:** 
- 属性：CustomerId, OrderDate, Status, TotalAmount
- 需要 OrderItem 子实体，包含 ProductId, ProductName, Price, Quantity
- 规则：订单金额不能为负，订单项不能为空

**Architect:** 🏗️ 理解了！这是我设计的结构：

```
Order (聚合根)
├── Id: long
├── CustomerId: long
├── OrderDate: DateTime
├── Status: OrderStatus
├── TotalAmount: Money
└── Items: List<OrderItem>
    ├── Id: int
    ├── ProductId: long
    ├── ProductName: string
    ├── Price: Money
    └── Quantity: int

值对象:
└── Money
    ├── Amount: decimal
    └── Currency: string

领域事件:
├── OrderCreatedEvent
└── OrderStatusChangedEvent
```

**不变量保护:**
1. ✅ Items 不能为空 → 在构造函数中验证
2. ✅ TotalAmount 不能为负 → 由 Money 值对象保证

确认生成？

**User:** 确认

**Baker:** 👨‍🍳 开始生成代码...

```
✅ Domain/Aggregates/Order.cs
✅ Domain/Aggregates/OrderItem.cs
✅ Domain/ValueObjects/Money.cs
✅ Domain/Repositories/IOrderRepository.cs
✅ Domain/Events/OrderCreatedEvent.cs
✅ Domain/Events/OrderStatusChangedEvent.cs
✅ Infrastructure/Repositories/OrderRepository.cs
✅ Infrastructure/Configurations/OrderConfiguration.cs
```

**Inspector:** 🔍 验证结果：
- ✅ 编译成功
- ✅ DDD 模式正确
- ✅ 仓储自动注册配置正确

```bash
# 下一步：添加迁移
dotnet ef migrations add AddOrder
```

需要我帮你创建 OrderController 吗？
