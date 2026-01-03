# 🧠 MiCake Knowledge Base

> MiCake Agent 系统的知识库，包含框架核心概念、设计模式和最佳实践

## 概述

知识库是 MiCake Agent 系统的核心组成部分，为 AI Agent 提供上下文信息和参考资料。Agent 在回答问题和生成代码时会自动查阅这些知识。

## 知识库结构

```
knowledge/
├── README.md                    # 本文件
├── ddd-patterns.md              # DDD 核心模式
├── module-system.md             # MiCake 模块系统
├── repository-patterns.md       # 仓储模式
├── best-practices.md            # 最佳实践
├── troubleshooting.md           # 常见问题排查
├── api-reference.md             # API 速查表
└── external/                    # 外部知识源
    └── micake-docs/             # 从 MiCake 官方文档同步
```

## 知识类别

### 1. DDD 模式 (ddd-patterns.md)
- Entity（实体）
- Aggregate Root（聚合根）
- Value Object（值对象）
- Domain Event（领域事件）
- Domain Service（领域服务）
- Repository（仓储）

### 2. 模块系统 (module-system.md)
- MiCakeModule 基类
- 模块生命周期
- 模块依赖管理
- 服务自动注册

### 3. 仓储模式 (repository-patterns.md)
- IRepository 接口
- EFRepository 实现
- 自定义仓储
- 工作单元模式

### 4. 最佳实践 (best-practices.md)
- 架构原则
- 编码规范
- 性能优化
- 测试策略

### 5. 问题排查 (troubleshooting.md)
- 常见错误及解决方案
- 调试技巧
- 性能诊断

## 外部知识源

### MiCake 官方文档

知识库支持从 [MiCake 官方文档仓库](https://github.com/MiCake/micake.github.io) 动态获取最新文档内容。

**同步方式：**
- Agent 在需要时自动查询官方文档
- 本地缓存常用文档片段
- 定期更新以保持同步

**文档源地址：**
```
https://github.com/MiCake/micake.github.io
├── src/                         # 文档源文件
│   ├── content/                 # MDX 文档内容
│   └── ...
└── doc/                         # 附加文档
```

### 开发原则文档

项目内置的开发原则文档提供权威的编码标准：
- `principles/development_principle.md` - MiCake 开发原则

## 如何扩展知识库

### 添加新知识文档

1. 在 `knowledge/` 目录创建新的 Markdown 文件
2. 使用标准的文档结构（见下方模板）
3. 在本 README 中注册新文档

### 文档模板

```markdown
# [知识主题]

## 概述
[简要描述]

## 核心概念
[详细说明]

## 代码示例
```csharp
// 示例代码
```

## 常见问题
[FAQ]

## 相关链接
- [链接1]
- [链接2]
```

### 链接外部资源

对于需要实时获取的外部资源，在 `external/` 目录创建配置文件：

```yaml
# external/source-config.yaml
sources:
  - name: "MiCake Official Docs"
    repo: "MiCake/micake.github.io"
    path: "src/content"
    refresh_interval: "daily"
```

## Agent 如何使用知识库

1. **问题识别** - Agent 分析用户问题，确定相关知识领域
2. **知识检索** - 从知识库中检索相关文档
3. **上下文注入** - 将相关知识作为上下文提供给 Agent
4. **响应生成** - Agent 基于知识库内容生成准确响应

## 知识库维护

### 更新频率
- 核心文档：随框架版本更新
- 最佳实践：持续更新
- 问题排查：基于社区反馈更新

### 贡献指南
欢迎通过 Pull Request 贡献知识内容：
1. Fork 仓库
2. 添加或更新知识文档
3. 提交 PR 并说明变更内容
