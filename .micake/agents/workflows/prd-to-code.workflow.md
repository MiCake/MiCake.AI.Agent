---
name: "prd-to-code"
description: "Complete workflow from PRD/requirements to working MiCake code"
---

# PRD to Code Workflow

> 从需求文档到可运行代码的完整开发链路

## 概述

本工作流指导 Agent 从 PRD（产品需求文档）或 User Story 开始，通过领域分析、架构设计、代码生成，最终产出符合 MiCake 规范的完整代码。

## 工作流架构

```
┌─────────────────────────────────────────────────────────────────┐
│                    PRD to Code Workflow                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  Phase 1: Requirements Analysis (Sage)                          │
│  ┌─────────────────────────────────────────────────┐            │
│  │ • 读取 .micake/requirements/ 中的文档            │            │
│  │ • 提取 User Stories 和业务规则                   │            │
│  │ • 识别核心领域概念                               │            │
│  │ • 产出: 领域概念清单                             │            │
│  └─────────────────────────────────────────────────┘            │
│                          ↓                                      │
│  Phase 2: Domain Modeling (Architect)                           │
│  ┌─────────────────────────────────────────────────┐            │
│  │ • 定义聚合边界                                   │            │
│  │ • 设计实体和值对象                               │            │
│  │ • 规划领域事件                                   │            │
│  │ • 设计模块结构                                   │            │
│  │ • 产出: 领域模型设计文档                         │            │
│  └─────────────────────────────────────────────────┘            │
│                          ↓                                      │
│  Phase 3: Code Generation (Baker)                               │
│  ┌─────────────────────────────────────────────────┐            │
│  │ • 生成聚合根和实体                               │            │
│  │ • 生成值对象                                     │            │
│  │ • 生成仓储接口和实现                             │            │
│  │ • 生成领域事件和处理器                           │            │
│  │ • 生成模块配置                                   │            │
│  │ • 生成 EF Core 配置                              │            │
│  │ • 产出: 完整代码文件                             │            │
│  └─────────────────────────────────────────────────┘            │
│                          ↓                                      │
│  Phase 4: Validation (Inspector)                                │
│  ┌─────────────────────────────────────────────────┐            │
│  │ • 代码审查                                       │            │
│  │ • 架构合规检查                                   │            │
│  │ • DDD 模式验证                                   │            │
│  │ • 产出: 审查报告                                 │            │
│  └─────────────────────────────────────────────────┘            │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

## 初始化

### 配置加载

```yaml
# 从 .micake/config.yaml 加载
required_config:
  - project.name
  - project.namespace
  - ddd.prefer_domain_events
  - ddd.use_static_factory_methods
  - ddd.aggregate_id_type
  - code_style.generate_xml_docs
```

### 需求文档发现

```yaml
# 扫描 .micake/requirements/ 目录
supported_formats:
  - "*.md"       # Markdown PRD
  - "*.txt"      # 纯文本需求
  - "*.json"     # 结构化需求
  - "*.yaml"     # YAML 格式需求
  
parsing_rules:
  user_story_pattern: "As a .* I want .* so that .*"
  acceptance_criteria_pattern: "Given .* When .* Then .*"
  business_rule_pattern: "Rule:|BR:|业务规则:"
```

---

## Phase 1: Requirements Analysis

**Agent**: Sage 🧙

### Step 1.1: 文档读取

```
<action>
读取 .micake/requirements/ 目录下所有支持格式的文档
</action>

<output>
列出发现的文档清单:
- document_name.md (PRD)
- user-stories.md (User Stories)
- ...
</output>
```

### Step 1.2: User Story 提取

```
<action>
从文档中提取 User Stories
</action>

<template>
| ID | As a | I want | So that | Priority |
|----|------|--------|---------|----------|
| US-001 | Customer | to place an order | I can purchase products | High |
</template>
```

### Step 1.3: 业务规则识别

```
<action>
识别并记录业务规则和约束
</action>

<template>
| ID | Rule | Domain Concept | Type |
|----|------|----------------|------|
| BR-001 | Order cannot be modified after confirmation | Order | Invariant |
</template>
```

### Step 1.4: 领域概念提取

```
<action>
从需求中识别领域概念
</action>

<template>
| Concept | Type Suggestion | Reasoning |
|---------|-----------------|-----------|
| Order | Aggregate Root | Has lifecycle, contains items |
| OrderItem | Entity | Needs identity within order |
| Money | Value Object | Immutable, equality by value |
</template>
```

### Step 1.5: 产出物

```markdown
# Requirements Analysis Report

## Documents Analyzed
- [List of documents]

## User Stories Summary
[Extracted user stories table]

## Business Rules
[Business rules table]

## Domain Concepts
[Identified concepts with type suggestions]

## Questions for Clarification
[Any ambiguities that need resolution]

## Recommended Next Steps
Proceed to domain modeling with Architect agent.
```

---

## Phase 2: Domain Modeling

**Agent**: Architect 🏗️

### Step 2.1: 聚合边界设计

```
<input>
Phase 1 的领域概念清单
</input>

<action>
为每个识别的聚合根设计边界
</action>

<template>
## Aggregate: {{Name}}

### Boundary Justification
{{Why these elements belong together}}

### Invariants Protected
1. {{Invariant 1}}
2. {{Invariant 2}}

### Structure
- Root: {{AggregateName}} ({{IdType}})
  - Entities: {{List}}
  - Value Objects: {{List}}
</template>
```

### Step 2.2: 实体和值对象设计

```
<action>
详细设计每个实体和值对象
</action>

<template>
### Entity: {{Name}}
| Property | Type | Constraints |
|----------|------|-------------|
| Id | {{Type}} | Primary Key |
| {{Prop}} | {{Type}} | {{Constraint}} |

### Value Object: {{Name}}
| Component | Type | Description |
|-----------|------|-------------|
| {{Comp}} | {{Type}} | {{Desc}} |
</template>
```

### Step 2.3: 领域事件规划

```
<action>
设计领域事件及其触发条件
</action>

<template>
| Event | Trigger | Published By | Handlers |
|-------|---------|--------------|----------|
| OrderCreatedEvent | Order.Create() | Order | InventoryHandler, NotificationHandler |
</template>
```

### Step 2.4: 模块结构设计

```
<action>
设计 MiCake 模块结构
</action>

<template>
## Module: {{ModuleName}}

### Dependencies
```csharp
[RelyOn(typeof(MiCakeAspNetCoreModule))]
```

### Folder Structure
```
{{ModuleName}}/
├── Domain/
│   ├── Aggregates/
│   │   └── {{Aggregate}}.cs
│   ├── Events/
│   │   └── {{Event}}.cs
│   └── Repositories/
│       └── I{{Aggregate}}Repository.cs
├── Application/
│   └── Services/
└── Infrastructure/
    ├── Repositories/
    │   └── {{Aggregate}}Repository.cs
    └── Configurations/
        └── {{Aggregate}}Configuration.cs
```
</template>
```

### Step 2.5: 产出物

```markdown
# Domain Model Design Document

## Overview
{{Project description and scope}}

## Aggregates
{{Detailed aggregate designs}}

## Domain Events
{{Event specifications}}

## Module Structure
{{Module organization}}

## Integration Points
{{How modules/aggregates communicate}}

## Design Decisions Log
| Decision | Rationale | Alternatives Considered |
|----------|-----------|------------------------|
| {{Dec}} | {{Why}} | {{Alts}} |
```

---

## Phase 3: Code Generation

**Agent**: Baker 👨‍🍳

### Step 3.1: 读取用户偏好

```
<action>
从 .micake/config.yaml 读取代码生成偏好
</action>

<preferences>
- use_static_factory_methods: {{true/false}}
- use_private_setters: {{true/false}}
- generate_xml_docs: {{true/false}}
- aggregate_id_type: {{long/Guid/etc}}
- domain_event_naming: {{past-tense/present-tense}}
</preferences>
```

### Step 3.2: 生成聚合根

```
<input>
Phase 2 的聚合设计文档
</input>

<action>
为每个聚合生成代码文件
</action>

<files>
- Domain/Aggregates/{{AggregateName}}.cs
- Domain/Aggregates/{{EntityName}}.cs
- Domain/Aggregates/{{ValueObjectName}}.cs
</files>
```

### Step 3.3: 生成仓储

```
<action>
生成仓储接口和实现
</action>

<files>
- Domain/Repositories/I{{Aggregate}}Repository.cs
- Infrastructure/Repositories/{{Aggregate}}Repository.cs
</files>
```

### Step 3.4: 生成领域事件

```
<action>
生成事件类和处理器
</action>

<files>
- Domain/Events/{{EventName}}.cs
- Domain/EventHandlers/{{EventName}}Handler.cs
</files>
```

### Step 3.5: 生成模块配置

```
<action>
生成 MiCake 模块类
</action>

<files>
- {{ModuleName}}Module.cs
</files>
```

### Step 3.6: 生成 EF Core 配置

```
<action>
生成实体配置和 DbContext
</action>

<files>
- Infrastructure/Configurations/{{Aggregate}}Configuration.cs
- Infrastructure/{{ProjectName}}DbContext.cs
</files>
```

### Step 3.7: 产出物

```markdown
# Generated Code Summary

## Files Created
| File | Purpose |
|------|---------|
| {{Path}} | {{Description}} |

## Configuration Required
- Add connection string to appsettings.json
- Register DbContext in Program.cs

## Next Steps
- Run `dotnet build` to verify compilation
- Add migrations: `dotnet ef migrations add Initial`
- Review code with Inspector agent
```

---

## Phase 4: Validation

**Agent**: Inspector 🔍

### Step 4.1: 编译检查

```
<action>
验证生成的代码可以编译
</action>

<check>
- No syntax errors
- All references resolved
- No missing using statements
</check>
```

### Step 4.2: 架构合规检查

```
<action>
验证代码符合 MiCake 架构原则
</action>

<check>
- Layer dependencies correct
- No circular references
- Module dependencies declared
</check>
```

### Step 4.3: DDD 模式验证

```
<action>
验证 DDD 模式正确实现
</action>

<check>
- Aggregates protect invariants
- Entities have identity
- Value objects are immutable
- Domain events properly raised
</check>
```

### Step 4.4: 最终报告

```markdown
# Validation Report

## Build Status: ✅ PASS / ❌ FAIL

## Compliance Score: {{X}}/100

## Issues Found
### Critical
{{List or "None"}}

### Warnings
{{List or "None"}}

### Suggestions
{{List}}

## Certification
{{APPROVED / REQUIRES CHANGES}}

## Recommended Actions
1. {{Action 1}}
2. {{Action 2}}
```

---

## 使用方式

### 启动工作流

```
User: @MiCake 我已经把PRD文档放到 .micake/requirements/ 了，请开始生成代码

Sage: 🧙 好的，让我开始分析你的需求文档...
[执行 Phase 1]

Sage: 分析完成，发现了以下领域概念... 准备好进行领域建模了吗？

User: 继续

Architect: 🏗️ 接手进行领域设计...
[执行 Phase 2]

Architect: 设计完成，准备生成代码吗？

User: 生成代码

Baker: 👨‍🍳 让我来烘焙代码...
[执行 Phase 3]

Baker: 代码生成完成！需要审查吗？

User: 审查

Inspector: 🔍 开始审查生成的代码...
[执行 Phase 4]

Inspector: 审查完成！✅ 代码符合 MiCake 规范。
```

### 跳过特定阶段

```
User: @MiCake 直接根据这个设计文档生成代码（跳过分析）

Baker: 👨‍🍳 好的，我会直接根据你的设计生成代码...
```

---

## 文档模板

工作流产出的文档保存在 `.micake-output/` 目录：

```
.micake-output/
├── requirements-analysis.md    # Phase 1 产出
├── domain-model-design.md      # Phase 2 产出
├── code-generation-log.md      # Phase 3 产出
└── validation-report.md        # Phase 4 产出
```
