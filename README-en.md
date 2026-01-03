# MiCake AI Agent

This repository provides AI-powered development assistants tailored for the MiCake framework, designed to enhance development efficiency, optimize project management, and deliver intelligent code suggestions and solutions.

## Features

Seven coordinated AI agents covering the entire software development lifecycle from project initialization to quality assurance:

1. **Conductor**: Main coordinator and guide, helps users with initial setup and routes them to appropriate agents
2. **Sage**: Requirements analysis expert, extracts domain concepts from PRD/User Stories
3. **Architect**: System architect, responsible for domain modeling and module structure design
4. **Developer**: Code implementation expert, handles feature development and bug fixes
5. **Inspector**: Code review expert, ensures code follows DDD patterns and best practices
6. **QA**: Quality assurance expert, validates implementation against requirements
7. **Consultant**: Technical advisor, provides guidance on MiCake framework and .NET ecosystem best practices

## Project Highlights

- Specifically designed for MiCake framework with deep integration
- Multi-agent collaboration covering the entire software development lifecycle
- Conductor agent as entry point for friendly guided experience
- Comprehensive command menus for quick access to functionalities
- Multi-platform support, agents can integrate with various development environments
  - GitHub Copilot
  - Claude AI [ In progress ]
  - ChatGPT [ In progress ]

## Quick Start

### Create New Project

When creating a new MiCake project using the `dotnet new` command, you'll be prompted whether to add AI Agent support. Selecting "Yes" will automatically integrate AI Agent code and configuration.

```bash
dotnet new micake-standardweb -n YourProject.Name
```

### Using AI Agents

1. Open your project in VS Code with GitHub Copilot installed
2. Open the Chat panel (Ctrl+Shift+I or Cmd+Shift+I)
3. Select `@micake_conductor` from the agent picker to start
4. Run `start` command to see welcome message and available services
5. Run `setup` to configure your preferences

### Agent Workflow

```
@micake_conductor (Initialize configuration)
        ↓
@micake_sage (Analyze requirements)
        ↓
@micake_architect (Design domain model)
        ↓
@micake_developer (Implement features)
        ↓
@micake_inspector (Review code quality)
        ↓
@micake_qa (Validate requirements compliance)
```

## Configuration

Shared configuration for all agents is located at `.micake/agents/config/preferences.yaml`. You can customize:

- Language preferences (communication, code comments, documentation)
- DDD design preferences (domain events, factory methods, etc.)
- Code style preferences
- Custom practices file path

## Documentation

- [Agent System Documentation](./.micake/agents/README.md)
- [MiCake Framework Documentation](https://micake.github.io/)

## Contributing

Issues and Pull Requests are welcome to improve the MiCake AI Agent system.
