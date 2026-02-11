# Context Directory

This directory contains project context information that agents use to understand the current state of the project.

## Purpose

The context directory provides:

1. **Project Structure**: How the codebase is organized
2. **Architecture Model**: Current architectural decisions
3. **Technology Stack**: Languages, frameworks, tools in use
4. **Conventions**: Project-specific coding standards

## Files

| File | Purpose |
|------|---------|
| `project-structure.yaml` | Directory structure and module organization |
| `architecture-model.yaml` | Architectural decisions and patterns |
| `tech-stack.yaml` | Technologies, versions, dependencies |
| `conventions.yaml` | Project-specific coding conventions |

## Usage

Agents automatically load context files when:

- Starting a new task
- Making design decisions
- Generating code
- Reviewing changes

## Maintenance

Keep context files updated as the project evolves:

- After structural changes
- When adding new technologies
- When conventions change
- After architectural decisions

## Rules

See `.rule/README.md` for rules that govern this directory.
