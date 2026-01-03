# MiCake AI Agent
本仓库用于构建为MiCake框架量身定制的AI助手，旨在提升开发效率，优化项目管理，并提供智能化的代码建议和解决方案。

## 功能特色
七大智能体协同，涵盖从项目初始化到代码质量保障的全流程支持：

1. **Conductor**：主持人与协调者，引导用户完成初始化配置并连接到合适的智能体
2. **Sage**：需求分析专家，从PRD/用户故事中提取领域概念
3. **Architect**：系统架构师，负责领域建模与模块结构设计
4. **Developer**：代码实现专家，完成功能开发与Bug修复
5. **Inspector**：代码审查专家，确保代码符合DDD规范与最佳实践
6. **QA**：质量保障专家，验证实现是否符合需求规格
7. **Consultant**：技术顾问，提供MiCake框架与.NET生态系统的指导与最佳实践
   
## 项目特色
- 专为MiCake框架设计，深度集成框架特性
- 多智能体协同工作，覆盖软件开发全生命周期
- Conductor智能体作为入口，提供友好的引导体验
- 提供详尽的命令菜单，方便用户快速调用所需功能
- 多平台支持，Agents可集成至各类开发环境与工具链
  - GitHub Copilot
  - Claude AI [ In progress ]
  - ChatGPT [ In progress ]

## 快速开始

### 创建新项目
当你使用`dotnet new`命令创建一个新的MiCake项目时，项目会提示你是否需要添加AI Agent支持。选择"是"后，项目将自动集成AI Agent相关代码和配置。

```bash
dotnet new micake-standardweb -n YourProject.Name
```

### 使用AI Agent

1. 在VS Code中打开项目，确保已安装GitHub Copilot
2. 打开聊天面板（Ctrl+Shift+I 或 Cmd+Shift+I）
3. 从智能体选择器中选择 `@micake_conductor` 开始
4. 运行 `start` 命令查看欢迎信息和可用服务
5. 运行 `setup` 配置你的偏好设置

### 智能体工作流

```
@micake_conductor (初始化配置)
        ↓
@micake_sage (分析需求文档)
        ↓
@micake_architect (设计领域模型)
        ↓
@micake_developer (实现功能代码)
        ↓
@micake_inspector (审查代码质量)
        ↓
@micake_qa (验证需求符合性)
```

## 配置

所有智能体共享的配置文件位于 `.micake/agents/config/preferences.yaml`，你可以自定义：

- 语言偏好（交流、代码注释、文档）
- DDD设计偏好（领域事件、工厂方法等）
- 代码风格偏好
- 自定义实践规范文件路径

## 文档

- [智能体系统文档](./.micake/agents/README.md)
- [MiCake框架文档](https://micake.github.io/)

## 贡献

欢迎提交Issue和Pull Request来改进MiCake AI Agent系统。
