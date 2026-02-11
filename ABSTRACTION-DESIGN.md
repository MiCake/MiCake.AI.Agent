# AI Agent 工作流模板抽象设计文档

## 1. 概述

### 1.1 目标

将当前 MiCake AI Agent 系统抽象为一个**通用的、可复用的 AI 多智能体协作框架**，使其能够适配不同的技术栈、框架和领域项目。

### 1.2 核心价值

| 维度 | 当前设计 | 抽象后 |
|------|----------|--------|
| 技术栈 | MiCake + ASP.NET Core | 任意技术栈 (Java, Python, Go, Rust...) |
| 领域 | DDD / 领域驱动设计 | 任意架构模式 (CQRS, 六边形, 分层...) |
| 框架 | EF Core, MiCake 特定API | 可插拔的框架适配器 |
| 语言 | C# 专属模板 | 语言无关的模板引擎 |

### 1.3 设计原则

1. **关注点分离**: 将"角色定义"与"领域知识"完全解耦
2. **配置驱动**: 通过配置文件适配不同项目，而非修改代码
3. **可扩展性**: 支持新增角色、工作流和知识库
4. **渐进增强**: 核心功能开箱即用，高级功能按需启用

---

## 2. 架构抽象分析

### 2.1 当前架构分层

```
┌─────────────────────────────────────────────────────────────────┐
│                     Agent System (通用层)                        │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  角色定义 (Conductor, Sage, Architect, Developer...)     │   │
│  │  工作流定义 (PRD-to-Code, Requirement-Change...)         │   │
│  │  交互协议 (确认机制, 边界检查, 上下文管理)                  │   │
│  └─────────────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│                     Domain Layer (领域层)                        │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  知识库 (DDD Patterns, Repository Patterns)              │   │ ← 需要抽象
│  │  模板 (Aggregate, Entity, ValueObject templates)         │   │ ← 需要抽象
│  │  偏好设置 (DDD 专属配置)                                  │   │ ← 需要抽象
│  └─────────────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│                     Context Layer (上下文层)                     │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  项目结构 (project-structure.yaml)                       │   │
│  │  领域模型 (domain-model.yaml) → 架构模型                  │   │
│  │  变更管理 (changes/)                                     │   │
│  └─────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
```

### 2.2 抽象要点

| 组件 | 当前绑定 | 抽象策略 |
|------|----------|----------|
| **角色定义** | MiCake 术语 | 提取通用角色模板，领域术语通过配置注入 |
| **知识库** | DDD 模式专属 | 知识库分层：核心知识 + 领域知识 + 项目知识 |
| **代码模板** | C# / MiCake 类 | 模板引擎 + 语言/框架适配器 |
| **工作流** | DDD 聚合创建 | 通用工作流 + 领域特定步骤插件 |
| **偏好配置** | DDD 术语 | 分层配置：系统配置 + 领域配置 + 项目配置 |

---

## 3. 通用框架设计

### 3.1 目录结构模板

```
.ai-agents/                           # 根目录 (可配置名称)
├── README.md                         # 系统说明
├── manifest.yaml                     # 系统清单 (新增)
│
├── agents/                           # 角色定义
│   ├── conductor.agent.md            # 协调者 (核心)
│   ├── analyst.agent.md              # 分析师 (原 Sage)
│   ├── architect.agent.md            # 架构师
│   ├── developer.agent.md            # 开发者
│   ├── reviewer.agent.md             # 审查者 (原 Inspector)
│   ├── tester.agent.md               # 测试者 (原 QA)
│   ├── advisor.agent.md              # 顾问 (原 Consultant)
│   └── _custom/                      # 用户自定义角色
│
├── config/                           # 配置层
│   ├── system.yaml                   # 系统级配置
│   ├── preferences.yaml              # 用户偏好
│   └── adapters/                     # 适配器配置
│       ├── dotnet.yaml               # .NET 适配
│       ├── java.yaml                 # Java 适配
│       ├── python.yaml               # Python 适配
│       └── _template.yaml            # 适配器模板
│
├── knowledge/                        # 知识库 (分层设计)
│   ├── core/                         # 核心知识 (通用)
│   │   ├── software-principles.md    # 软件工程原则
│   │   ├── code-quality.md           # 代码质量标准
│   │   └── review-checklist.md       # 通用审查清单
│   ├── patterns/                     # 架构模式 (可选包)
│   │   ├── ddd/                      # DDD 模式包
│   │   ├── clean-architecture/       # 整洁架构包
│   │   ├── cqrs/                     # CQRS 模式包
│   │   └── microservices/            # 微服务模式包
│   └── project/                      # 项目专属知识
│       └── README.md                 # 项目特定约定
│
├── templates/                        # 代码模板
│   ├── _engine/                      # 模板引擎配置
│   │   └── variables.yaml            # 模板变量定义
│   ├── dotnet/                       # .NET 模板
│   ├── java/                         # Java 模板
│   ├── python/                       # Python 模板
│   └── docs/                         # 文档模板
│
├── workflows/                        # 工作流定义
│   ├── core/                         # 核心工作流
│   │   ├── requirement-to-code.workflow.md
│   │   ├── requirement-change.workflow.md
│   │   └── code-review.workflow.md
│   └── extensions/                   # 扩展工作流
│       └── README.md
│
├── context/                          # 项目上下文
│   ├── .rule/
│   │   └── README.md                 # 输出规则
│   ├── project-structure.yaml        # 项目结构
│   └── architecture-model.yaml       # 架构模型 (重命名)
│
├── changes/                          # 变更管理
│   ├── .rule/
│   │   └── README.md
│   ├── pending/
│   ├── in-progress/
│   └── completed/
│
└── requirements/                     # 需求文档
    └── README.md
```

### 3.2 系统清单 (manifest.yaml) - 新增

```yaml
# AI Agent System Manifest
# 定义系统级元数据和能力声明

version: "1.0"
name: "AI Agent Workflow Framework"
description: "Multi-agent collaboration framework for software development"

# 技术栈适配器
adapter:
  # 当前激活的适配器
  active: "dotnet"
  # 可用适配器列表
  available:
    - "dotnet"
    - "java"
    - "python"
    - "go"
    - "typescript"

# 架构模式包
patterns:
  # 当前激活的模式包
  active: "ddd"
  # 可用模式包
  available:
    - "ddd"
    - "clean-architecture"
    - "hexagonal"
    - "cqrs"
    - "layered"

# 角色注册表
agents:
  conductor:
    file: "agents/conductor.agent.md"
    required: true
    description: "Task coordinator and dispatcher"
  analyst:
    file: "agents/analyst.agent.md"
    required: true
    description: "Requirements analysis expert"
  architect:
    file: "agents/architect.agent.md"
    required: true
    description: "System architecture designer"
  developer:
    file: "agents/developer.agent.md"
    required: true
    description: "Code implementation specialist"
  reviewer:
    file: "agents/reviewer.agent.md"
    required: true
    description: "Code quality reviewer"
  tester:
    file: "agents/tester.agent.md"
    required: false
    description: "Quality assurance engineer"
  advisor:
    file: "agents/advisor.agent.md"
    required: false
    description: "Domain and framework consultant"

# 工作流注册表
workflows:
  requirement-to-code:
    file: "workflows/core/requirement-to-code.workflow.md"
    description: "Transform requirements into working code"
  requirement-change:
    file: "workflows/core/requirement-change.workflow.md"
    description: "Handle requirement changes systematically"
  code-review:
    file: "workflows/core/code-review.workflow.md"
    description: "Comprehensive code review process"

# 知识库配置
knowledge:
  core: "knowledge/core/"
  patterns: "knowledge/patterns/${patterns.active}/"
  project: "knowledge/project/"

# 模板配置
templates:
  engine: "handlebars"  # 或 mustache, jinja2
  language: "${adapter.active}"
  path: "templates/${adapter.active}/"
```

---

## 4. 角色定义抽象

### 4.1 角色模板结构

每个角色应遵循统一的定义结构:

```markdown
# {RoleName} Agent

{Brief description of the role}

## Metadata

- **ID**: {system}-{role}
- **Name**: {Display Name}
- **Title**: {Professional Title}
- **Domain**: ${manifest.patterns.active}  # 动态注入

## Role Boundaries

<role-definition critical="MANDATORY">
### Primary Responsibilities
- {Responsibility 1}
- {Responsibility 2}

### Deliverables
- {Deliverable 1}
- {Deliverable 2}

### NOT My Responsibilities (Strict Boundaries)
- I do NOT {boundary 1} - that's {OtherAgent}'s job
- I do NOT {boundary 2} - that's {OtherAgent}'s job

### Interaction Protocol
1. {Protocol rule 1}
2. {Protocol rule 2}
</role-definition>

## Critical Actions

<activation critical="MANDATORY">
On activation:
1. LOAD system manifest from `manifest.yaml`
2. LOAD user preferences from `config/preferences.yaml`
3. LOAD active adapter configuration
4. LOAD domain-specific knowledge from `knowledge/patterns/{active}/`
5. VERIFY role boundaries
6. LOAD project context if exists
7. NEVER break character or exceed boundaries
</activation>

## Persona

### Role
{Role description using ${domain} placeholders}

### Identity
{Professional identity description}

### Communication Style
{How this agent communicates}

### Principles
- {Principle 1}
- {Principle 2}

## Commands

### {command-name}
{Command description}

<{command}-protocol critical="MANDATORY">
Process:
1. {Step 1}
2. {Step 2}
[CRITICAL] {Important constraint}
</{command}-protocol>

## Menu

| Command | Action | Description |
|---------|--------|-------------|
| {cmd1} | {action} | {desc} |

## References

- Knowledge: `knowledge/patterns/${active}/{relevant-file}.md`
- Templates: `templates/${adapter}/*.template.*`
```

### 4.2 角色命名抽象

| 原名称 | 通用名称 | 职责描述 |
|--------|----------|----------|
| Conductor | **Conductor** | 任务协调者，负责理解需求、分解任务、调度角色 |
| Sage | **Analyst** | 需求分析师，负责解析需求文档、提取关键概念 |
| Architect | **Architect** | 架构设计师，负责系统设计、模块规划 |
| Developer | **Developer** | 开发工程师，负责代码实现 |
| Inspector | **Reviewer** | 代码审查员，负责质量检查、规范遵从 |
| QA | **Tester** | 测试工程师，负责验证、测试用例 |
| Consultant | **Advisor** | 技术顾问，负责最佳实践指导 |

### 4.3 角色边界的参数化

通过配置注入领域术语，而非硬编码:

```yaml
# config/adapters/dotnet.yaml
adapter:
  name: "dotnet"
  language: "C#"
  framework: "ASP.NET Core"
  
  terminology:
    # 架构组件术语
    module: "Module"
    service: "Service"
    repository: "Repository"
    controller: "Controller"
    
    # 文件后缀
    source_extension: ".cs"
    test_extension: ".Tests.cs"
    
    # 项目类型
    project_types:
      - "ClassLibrary"
      - "WebAPI"
      - "Console"
      - "Test"

  commands:
    build: "dotnet build"
    test: "dotnet test"
    run: "dotnet run"

  patterns:
    namespace_convention: "PascalCase"
    file_naming: "PascalCase"
    method_naming: "PascalCase"
```

```yaml
# config/adapters/java.yaml
adapter:
  name: "java"
  language: "Java"
  framework: "Spring Boot"
  
  terminology:
    module: "Module"
    service: "Service"
    repository: "Repository"
    controller: "Controller"
    
    source_extension: ".java"
    test_extension: "Test.java"
    
    project_types:
      - "Library"
      - "WebApplication"
      - "CLI"
      - "Test"

  commands:
    build: "mvn compile"
    test: "mvn test"
    run: "mvn spring-boot:run"

  patterns:
    namespace_convention: "lowercase.dots"
    file_naming: "PascalCase"
    method_naming: "camelCase"
```

---

## 5. 知识库分层设计

### 5.1 三层知识架构

```
知识库 (Knowledge Base)
├── 核心层 (Core) - 通用软件工程知识
│   ├── 软件设计原则 (SOLID, DRY, KISS)
│   ├── 代码质量标准
│   ├── 通用审查清单
│   └── 常见重构模式
│
├── 模式层 (Patterns) - 架构模式专属知识
│   ├── DDD 模式包
│   │   ├── ddd-patterns.md
│   │   ├── aggregate-design.md
│   │   └── domain-events.md
│   ├── Clean Architecture 模式包
│   │   ├── layer-rules.md
│   │   └── use-cases.md
│   └── Microservices 模式包
│       ├── service-design.md
│       └── communication-patterns.md
│
└── 项目层 (Project) - 项目专属知识
    ├── 团队编码规范
    ├── 项目特定约定
    └── 业务领域术语
```

### 5.2 知识引用机制

在角色定义中使用变量引用知识:

```markdown
## Knowledge References

### Core Knowledge (Always Loaded)
- [Software Principles](../knowledge/core/software-principles.md)
- [Code Quality](../knowledge/core/code-quality.md)

### Pattern Knowledge (Based on ${manifest.patterns.active})
- [Pattern Guide](../knowledge/patterns/${active}/README.md)
- [Design Patterns](../knowledge/patterns/${active}/patterns.md)

### Project Knowledge (If Exists)
- [Project Conventions](../knowledge/project/conventions.md)
```

### 5.3 核心知识内容示例

```markdown
# software-principles.md

## SOLID Principles

### Single Responsibility (SRP)
A class should have only one reason to change.

**Review Checklist:**
- Does this class have a single, well-defined purpose?
- Would a change in one area require changes in unrelated methods?

### Open/Closed (OCP)
Open for extension, closed for modification.

**Review Checklist:**
- Can new behavior be added without modifying existing code?
- Are abstractions used appropriately?

### Liskov Substitution (LSP)
Subtypes must be substitutable for base types.

### Interface Segregation (ISP)
Clients shouldn't depend on interfaces they don't use.

### Dependency Inversion (DIP)
Depend on abstractions, not concretions.

---

## General Best Practices

### Code Clarity
- Meaningful names
- Small functions
- Single level of abstraction

### Error Handling
- Fail fast
- Specific exceptions
- Proper logging

### Testing
- Unit tests for business logic
- Integration tests for boundaries
- Test behavior, not implementation
```

---

## 6. 工作流抽象设计

### 6.1 通用工作流模板

```yaml
# workflows/core/requirement-to-code.workflow.yaml
name: "Requirement to Code"
description: "Transform requirements into working code"
version: "1.0"

# 工作流元数据
metadata:
  estimated_duration: "varies"
  complexity: "medium-high"
  requires_confirmation: true

# 阶段定义
phases:
  - id: "requirements-analysis"
    name: "Requirements Analysis"
    agent: "analyst"
    inputs:
      - type: "document"
        source: "requirements/"
        formats: ["*.md", "*.txt", "*.yaml"]
    outputs:
      - type: "document"
        name: "requirements-analysis.yaml"
        destination: "requirements/"
    confirmation_required: true
    
  - id: "architecture-design"
    name: "Architecture Design"
    agent: "architect"
    depends_on: ["requirements-analysis"]
    inputs:
      - type: "document"
        source: "requirements/requirements-analysis.yaml"
    outputs:
      - type: "document"
        name: "architecture-design.md"
        destination: "context/"
    confirmation_required: true
    
  - id: "implementation"
    name: "Code Implementation"
    agent: "developer"
    depends_on: ["architecture-design"]
    inputs:
      - type: "document"
        source: "context/architecture-design.md"
      - type: "templates"
        source: "templates/${adapter}/"
    outputs:
      - type: "code"
        destination: "${project.source_path}/"
    confirmation_required: false
    
  - id: "code-review"
    name: "Code Review"
    agent: "reviewer"
    depends_on: ["implementation"]
    inputs:
      - type: "code"
        source: "${project.source_path}/"
    outputs:
      - type: "document"
        name: "review-report.md"
        destination: "changes/"
    confirmation_required: true
    on_issues:
      action: "return_to"
      target: "implementation"
      
  - id: "validation"
    name: "Requirements Validation"
    agent: "tester"
    depends_on: ["code-review"]
    inputs:
      - type: "document"
        source: "requirements/"
      - type: "code"
        source: "${project.source_path}/"
    outputs:
      - type: "document"
        name: "validation-report.md"
        destination: "changes/"
    confirmation_required: true

# 错误处理
error_handling:
  on_unclear_requirements:
    agent: "analyst"
    action: "clarify"
  on_design_conflict:
    agent: "architect"
    action: "resolve"
  on_review_failure:
    agent: "developer"
    action: "fix"

# 工作流图 (用于可视化)
flow: |
  analyst --> architect --> developer --> reviewer --> tester
                               ^              |
                               |______________|
                                 (on issues)
```

### 6.2 工作流执行协议

每个阶段遵循统一的执行协议:

```markdown
## Phase Execution Protocol

### 1. Pre-execution
- Load phase configuration
- Verify all dependencies completed
- Load required inputs
- Check for conflicts

### 2. Execution
- Agent performs assigned task
- Generates outputs according to specification
- Records progress in changes/

### 3. Confirmation (if required)
- Present results to user
- Wait for explicit confirmation
- Options: approve / modify / discuss / reject

### 4. Post-execution
- Save outputs to specified destinations
- Update workflow status
- Prepare handoff context for next phase
```

---

## 7. 配置系统设计

### 7.1 配置层次

```
配置优先级 (从低到高):
1. 系统默认配置 (config/system.yaml)
2. 适配器配置 (config/adapters/{adapter}.yaml)
3. 模式包配置 (knowledge/patterns/{pattern}/config.yaml)
4. 用户偏好 (config/preferences.yaml)
5. 环境变量覆盖
```

### 7.2 通用配置模板

```yaml
# config/system.yaml - 系统级默认配置
system:
  version: "1.0"
  name: "AI Agent Framework"
  
  # 目录配置
  directories:
    agents: "agents/"
    config: "config/"
    knowledge: "knowledge/"
    templates: "templates/"
    workflows: "workflows/"
    context: "context/"
    changes: "changes/"
    requirements: "requirements/"
  
  # 文件格式偏好
  formats:
    structured_data: "yaml"      # yaml | json
    documents: "markdown"
    line_endings: "lf"           # lf | crlf
    encoding: "utf-8"
  
  # 交互配置
  interaction:
    confirm_before_generate: true
    confirm_before_handoff: true
    verbose_logging: false
    
  # 输出规则
  output:
    max_nesting_depth: 3
    avoid_prose: true
    no_emojis: true

# config/preferences.yaml - 用户偏好配置
preferences:
  # 语言设置
  language:
    communication: "zh-CN"       # Agent 交流语言
    documentation: "zh-CN"       # 文档输出语言
    code_comments: "en-US"       # 代码注释语言
  
  # 代码风格 (通用)
  code_style:
    generate_comments: true
    max_line_length: 120
    indent_style: "space"
    indent_size: 4
  
  # 确认行为
  behavior:
    auto_confirm_minor_changes: false
    show_reasoning: true
    detailed_reports: true
```

### 7.3 适配器配置扩展

可以为不同技术栈创建专属配置:

```yaml
# config/adapters/python.yaml
adapter:
  name: "python"
  language: "Python"
  version: "3.11+"
  
  terminology:
    module: "module"
    service: "service"
    repository: "repository"
    handler: "handler"
    
    source_extension: ".py"
    test_extension: "_test.py"
    
  commands:
    install: "pip install -r requirements.txt"
    build: "python -m build"
    test: "pytest"
    lint: "ruff check"
    format: "black ."
    run: "python -m ${project.main}"

  patterns:
    namespace_convention: "snake_case"
    file_naming: "snake_case"
    class_naming: "PascalCase"
    function_naming: "snake_case"
    
  code_style:
    type_hints: true
    docstrings: "google"         # google | numpy | sphinx
    
  project_structure:
    source_dir: "src/"
    test_dir: "tests/"
    config_files:
      - "pyproject.toml"
      - "requirements.txt"
```

---

## 8. 模板系统设计

### 8.1 模板引擎抽象

使用变量占位符，支持多种模板引擎:

```yaml
# templates/_engine/variables.yaml
variables:
  # 项目级变量
  project:
    name: "从项目配置读取"
    namespace: "项目根命名空间"
    source_path: "源代码路径"
    
  # 实体级变量
  entity:
    name: "实体名称"
    name_camel: "驼峰命名"
    name_snake: "下划线命名"
    
  # 类型映射
  type_mappings:
    # 从通用类型到语言特定类型的映射
    string:
      dotnet: "string"
      java: "String"
      python: "str"
    integer:
      dotnet: "int"
      java: "Integer"
      python: "int"
    uuid:
      dotnet: "Guid"
      java: "UUID"
      python: "uuid.UUID"
```

### 8.2 语言无关的模板定义

```yaml
# templates/abstract/entity.template.yaml
name: "Entity Template"
description: "Base entity class template"
type: "class"

structure:
  imports:
    - { module: "base_entity", from: "${framework.domain}" }
    
  class:
    name: "${entity.name}"
    extends: "${framework.entity_base}<${entity.id_type}>"
    
    sections:
      - name: "properties"
        items: "${entity.properties}"
        template: "property"
        
      - name: "constructor"
        visibility: "private"
        for: "orm"
        
      - name: "factory_method"
        condition: "${preferences.use_factory_methods}"
        name: "create"
        static: true
        
      - name: "business_methods"
        items: "${entity.methods}"
        template: "method"

# 然后每个语言适配器提供具体实现
```

### 8.3 多语言模板示例

```
templates/
├── abstract/                    # 抽象模板定义
│   ├── entity.template.yaml
│   ├── service.template.yaml
│   └── repository.template.yaml
│
├── dotnet/                      # C# 实现
│   ├── entity.template.cs
│   ├── service.template.cs
│   └── repository.template.cs
│
├── java/                        # Java 实现
│   ├── entity.template.java
│   ├── service.template.java
│   └── repository.template.java
│
└── python/                      # Python 实现
    ├── entity.template.py
    ├── service.template.py
    └── repository.template.py
```

---

## 9. 扩展机制设计

### 9.1 自定义角色

用户可以添加自定义角色:

```markdown
# agents/_custom/devops.agent.md

# DevOps Agent

Infrastructure and deployment specialist.

## Metadata

- **ID**: custom-devops
- **Name**: DevOps Engineer
- **Title**: Infrastructure Specialist
- **Custom**: true

## Role Boundaries

### Primary Responsibilities
- Infrastructure as Code
- CI/CD pipeline management
- Deployment automation
- Monitoring setup

### NOT My Responsibilities
- Application code implementation
- Business logic design
- Code review (quality)

## Commands

### setup-pipeline
Configure CI/CD pipeline for the project.

### deploy
Deploy application to target environment.

### monitor
Setup monitoring and alerting.
```

### 9.2 自定义工作流

```yaml
# workflows/extensions/release.workflow.yaml
name: "Release Workflow"
description: "End-to-end release process"
custom: true

phases:
  - id: "pre-release-review"
    agent: "reviewer"
    
  - id: "release-testing"
    agent: "tester"
    
  - id: "deployment"
    agent: "custom-devops"    # 使用自定义角色
    
  - id: "post-release-validation"
    agent: "tester"
```

### 9.3 知识包扩展

```yaml
# knowledge/patterns/my-custom-pattern/manifest.yaml
pattern:
  name: "My Custom Architecture"
  version: "1.0"
  
files:
  - "overview.md"
  - "components.md"
  - "patterns.md"
  
terminology:
  # 自定义术语
  aggregate: "Domain Model"
  service: "Use Case"
  
review_checklist:
  - "custom-checklist.md"
```

---

## 10. 迁移指南

### 10.1 从当前 MiCake Agent 迁移

1. **目录重命名**
   ```
   .micake/agents/  →  .ai-agents/
   ```

2. **创建系统清单**
   - 创建 `manifest.yaml`
   - 配置 `adapter: dotnet`
   - 配置 `patterns: ddd`

3. **重组知识库**
   ```
   knowledge/
   ├── core/          # 新增: 通用知识
   ├── patterns/ddd/  # 原有 DDD 知识移入
   └── project/       # 项目特定知识
   ```

4. **更新角色引用**
   - 将 MiCake 特定术语替换为变量
   - 添加 `${adapter}` 和 `${pattern}` 占位符

5. **配置分离**
   - DDD 相关配置移至 `knowledge/patterns/ddd/config.yaml`
   - 通用偏好保留在 `config/preferences.yaml`

### 10.2 适配新项目

1. **选择适配器**
   ```yaml
   # manifest.yaml
   adapter:
     active: "java"  # 或 python, go, etc.
   ```

2. **选择架构模式**
   ```yaml
   patterns:
     active: "clean-architecture"  # 或 ddd, hexagonal, etc.
   ```

3. **配置项目偏好**
   ```yaml
   # config/preferences.yaml
   preferences:
     language:
       communication: "zh-CN"
     code_style:
       # 项目特定样式
   ```

4. **添加项目知识**
   - 在 `knowledge/project/` 添加团队约定
   - 在 `templates/{adapter}/` 添加自定义模板

---

## 11. 实施建议

### 11.1 阶段性实施路线

| 阶段 | 内容 | 预计工作量 |
|------|------|------------|
| **Phase 1: 基础抽象** | 角色通用化、配置分离 | 3-5 天 |
| **Phase 2: 知识分层** | 知识库重组、核心知识提取 | 2-3 天 |
| **Phase 3: 模板系统** | 模板引擎、多语言支持 | 5-7 天 |
| **Phase 4: 适配器** | .NET 适配器完善、Java 适配器 | 5-7 天 |
| **Phase 5: 工作流** | 工作流 YAML 化、扩展机制 | 3-5 天 |
| **Phase 6: 文档 & 测试** | 用户文档、测试验证 | 3-5 天 |

### 11.2 优先级建议

**高优先级 (必须):**
1. 系统清单 (manifest.yaml)
2. 角色定义中的变量占位符
3. 配置分层 (system/adapter/preferences)
4. 知识库核心/模式分离

**中优先级 (推荐):**
1. 工作流 YAML 定义
2. 多语言模板抽象
3. 自定义角色支持

**低优先级 (可选):**
1. 模板引擎统一
2. 图形化工作流编辑
3. 模式包市场

### 11.3 向后兼容策略

- 保留 `.micake/` 目录支持作为 `.ai-agents/` 的别名
- 自动检测旧版配置并提示迁移
- 提供迁移脚本自动转换

---

## 12. 总结

### 12.1 抽象后的核心架构

```
┌─────────────────────────────────────────────────────────────────┐
│                    Framework Core (框架核心)                     │
│       角色协议 | 工作流引擎 | 配置系统 | 扩展机制                  │
├────────────────────┬────────────────────┬───────────────────────┤
│   Adapter Layer    │   Pattern Layer    │   Project Layer       │
│   (技术栈适配)      │   (架构模式)        │   (项目专属)           │
│   ─────────────    │   ─────────────    │   ─────────────       │
│   • .NET           │   • DDD            │   • 团队规范           │
│   • Java           │   • Clean Arch     │   • 业务术语           │
│   • Python         │   • Hexagonal      │   • 自定义模板         │
│   • Go             │   • CQRS           │   • 扩展工作流         │
│   • TypeScript     │   • Microservices  │   • 自定义角色         │
└────────────────────┴────────────────────┴───────────────────────┘
```

### 12.2 关键设计决策

| 决策点 | 选择 | 理由 |
|--------|------|------|
| 配置格式 | YAML | 人类可读、工具友好、表达力强 |
| 模板语法 | Handlebars 兼容 | 广泛支持、简洁、跨语言可用 |
| 知识组织 | 三层架构 | 平衡通用性与专业性 |
| 角色扩展 | 目录约定 | 零配置发现、简单直观 |
| 工作流定义 | 声明式 YAML | 可版本控制、易于理解和修改 |

### 12.3 预期收益

1. **复用性**: 同一框架支持多种技术栈和架构模式
2. **可维护性**: 关注点分离使更新更简单
3. **可扩展性**: 插件式架构支持自定义扩展
4. **一致性**: 统一的角色协议确保质量
5. **可移植性**: 项目间共享知识和最佳实践

---

## 附录 A: 完整目录结构

```
.ai-agents/
├── manifest.yaml
├── README.md
│
├── agents/
│   ├── conductor.agent.md
│   ├── analyst.agent.md
│   ├── architect.agent.md
│   ├── developer.agent.md
│   ├── reviewer.agent.md
│   ├── tester.agent.md
│   ├── advisor.agent.md
│   └── _custom/
│       └── README.md
│
├── config/
│   ├── system.yaml
│   ├── preferences.yaml
│   └── adapters/
│       ├── _template.yaml
│       ├── dotnet.yaml
│       ├── java.yaml
│       └── python.yaml
│
├── knowledge/
│   ├── core/
│   │   ├── software-principles.md
│   │   ├── code-quality.md
│   │   └── review-checklist.md
│   ├── patterns/
│   │   ├── ddd/
│   │   │   ├── manifest.yaml
│   │   │   ├── overview.md
│   │   │   ├── patterns.md
│   │   │   └── review-checklist.md
│   │   ├── clean-architecture/
│   │   └── microservices/
│   └── project/
│       └── README.md
│
├── templates/
│   ├── _engine/
│   │   └── variables.yaml
│   ├── abstract/
│   │   ├── entity.template.yaml
│   │   └── service.template.yaml
│   ├── dotnet/
│   │   ├── entity.template.cs
│   │   └── service.template.cs
│   ├── java/
│   └── python/
│
├── workflows/
│   ├── core/
│   │   ├── requirement-to-code.workflow.md
│   │   ├── requirement-change.workflow.md
│   │   └── code-review.workflow.md
│   └── extensions/
│       └── README.md
│
├── context/
│   ├── .rule/
│   │   └── README.md
│   ├── project-structure.yaml
│   └── architecture-model.yaml
│
├── changes/
│   ├── .rule/
│   │   └── README.md
│   ├── pending/
│   ├── in-progress/
│   └── completed/
│
└── requirements/
    └── README.md
```

---

## 附录 B: 术语对照表

| 中文术语 | 英文术语 | 说明 |
|----------|----------|------|
| 协调者 | Conductor | 任务分发和协调的核心角色 |
| 分析师 | Analyst | 需求分析专家 |
| 架构师 | Architect | 系统设计专家 |
| 开发者 | Developer | 代码实现专家 |
| 审查员 | Reviewer | 代码质量检查专家 |
| 测试员 | Tester | 质量保证专家 |
| 顾问 | Advisor | 领域和技术顾问 |
| 适配器 | Adapter | 技术栈适配配置 |
| 模式包 | Pattern Pack | 架构模式知识集合 |
| 知识库 | Knowledge Base | 参考知识文档集合 |
| 工作流 | Workflow | 多阶段任务流程定义 |
