# 🍰 MiCake Agent System

> AI-Powered Development Assistant for MiCake Framework

## 概述

MiCake Agent System 是一个专为 MiCake 框架用户设计的 AI 助手系统，帮助开发者快速构建符合 DDD（领域驱动设计）规范的 .NET 应用程序。

## 特性

- 🧙 **智能向导** - 从需求到代码的完整开发链路
- 🏗️ **架构设计** - DDD 领域建模与模块规划
- 👨‍🍳 **代码生成** - 自动生成符合 MiCake 规范的代码
- 🔍 **代码审查** - 基于最佳实践的自动化审查
- 📚 **知识库** - 内置 MiCake 框架完整知识体系

## 快速开始

### 在 VS Code / GitHub Copilot Chat 中使用

1. 在项目根目录创建 `.micake/` 文件夹（如果需要自定义配置）
2. 在 Copilot Chat 中选择 MiCake Agent
3. 开始对话：`@MiCake 帮我创建一个订单聚合根`

### 可用命令

| 命令 | 说明 | Agent |
|------|------|-------|
| `*help` | 显示帮助信息 | Helper |
| `*init` | 初始化 MiCake 项目 | Sage |
| `*create-aggregate` | 创建聚合根 | Baker |
| `*create-entity` | 创建实体 | Baker |
| `*create-module` | 创建模块 | Baker |
| `*review` | 代码审查 | Inspector |
| `*diagnose` | 问题诊断 | Inspector |

## Agent 角色

| Agent | 图标 | 职责 |
|-------|------|------|
| [Sage](./sage.agent.md) | 🧙 | 项目向导、需求分析 |
| [Architect](./architect.agent.md) | 🏗️ | 架构设计、领域建模 |
| [Baker](./baker.agent.md) | 👨‍🍳 | 代码生成、实现指导 |
| [Inspector](./inspector.agent.md) | 🔍 | 代码审查、问题诊断 |
| [Helper](./helper.agent.md) | 💡 | 快速帮助、API 查询 |

## 目录结构

```
.github/agents/
├── README.md                        # 本文件
├── sage.agent.md                    # Sage Agent - 项目向导
├── architect.agent.md               # Architect Agent - 架构设计
├── baker.agent.md                   # Baker Agent - 代码生成
├── inspector.agent.md               # Inspector Agent - 代码审查
├── helper.agent.md                  # Helper Agent - 快速帮助
│
├── config/
│   └── preferences.yaml             # 用户偏好设置模板
│
├── knowledge/                       # 知识库
│   ├── README.md                    # 知识库概述
│   ├── ddd-patterns.md              # DDD 核心模式
│   ├── module-system.md             # 模块系统详解
│   ├── repository-patterns.md       # 仓储模式详解
│   ├── best-practices.md            # 开发最佳实践
│   └── troubleshooting.md           # 常见问题排查
│
├── workflows/                       # 工作流定义
│   ├── new-project.workflow.md      # 新建项目工作流
│   ├── create-aggregate.workflow.md # 创建聚合工作流
│   └── prd-to-code.workflow.md      # PRD 到代码工作流
│
└── templates/                       # 代码模板
    ├── aggregate.template.cs        # 聚合根模板
    ├── entity.template.cs           # 实体模板
    ├── value-object.template.cs     # 值对象模板
    ├── repository.template.cs       # 仓储模板
    ├── domain-event.template.cs     # 领域事件模板
    ├── module.template.cs           # 模块模板
    └── ef-configuration.template.cs # EF Core 配置模板
```

## 用户配置

用户可在项目中创建 `.micake/config.yaml` 来配置偏好设置。详见 [用户偏好设置](./config/preferences.yaml)。

## 外部需求文档

支持导入外部 AI Agent 生成的需求文档（PRD、User Story 等）。将文档放置于 `.micake/requirements/` 目录即可。

## 参考链接

- [MiCake 框架文档](https://micake.github.io/)
- [MiCake GitHub 仓库](https://github.com/MiCake/MiCake)
- [开发原则](../../principles/development_principle.md)
