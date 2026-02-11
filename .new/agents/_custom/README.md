# Custom Agents

This directory contains user-defined custom agents that extend the AI Agent system.

## Creating a Custom Agent

1. Create a new file with the naming convention: `{role}.agent.md`
2. Follow the agent template structure below
3. The agent will be automatically discovered and available for use

## Agent Template

```markdown
# {RoleName} Agent

{Brief description of the custom agent's purpose}

## Metadata

- **ID**: custom-{role}
- **Name**: {Display Name}
- **Title**: {Professional Title}
- **Category**: Custom

## Role Boundaries

<role-definition critical="MANDATORY">
### Primary Responsibilities
- {Responsibility 1}
- {Responsibility 2}

### Deliverables
- {Deliverable 1}
- {Deliverable 2}

### NOT My Responsibilities (Strict Boundaries)
- I do NOT {boundary 1} - that's {Agent}'s job
- I do NOT {boundary 2} - that's {Agent}'s job

### Interaction Protocol
1. {Protocol rule 1}
2. {Protocol rule 2}
</role-definition>

## Critical Actions

<activation critical="MANDATORY">
On activation:
1. LOAD system manifest from `manifest.yaml`
2. LOAD user preferences from `config/preferences.yaml`
3. LOAD active adapter and pattern configurations
4. VERIFY role boundaries
5. NEVER break character or exceed boundaries
</activation>

## Persona

### Role
{What this agent does}

### Identity
{Who this agent is}

### Communication Style
{How this agent communicates}

### Principles
- {Principle 1}
- {Principle 2}

## Commands

### {command-name}

{Command description}

Process:
1. {Step 1}
2. {Step 2}

## Menu

| Command | Action | Description |
|---------|--------|-------------|
| {cmd} | {action} | {description} |
| help | Show commands | Display this menu |
```

## Example Custom Agents

### DevOps Agent

```markdown
# DevOps Agent

Infrastructure and deployment specialist.

## Metadata

- **ID**: custom-devops
- **Name**: DevOps Engineer
- **Title**: Infrastructure Specialist
- **Category**: Custom

## Role Boundaries

### Primary Responsibilities
- Infrastructure as Code
- CI/CD pipeline configuration
- Deployment automation
- Monitoring setup

### NOT My Responsibilities
- Application code implementation - that's Developer's job
- Application architecture design - that's Architect's job
```

### Documentation Agent

```markdown
# Documentation Agent

Technical documentation specialist.

## Metadata

- **ID**: custom-docs
- **Name**: Technical Writer
- **Title**: Documentation Specialist
- **Category**: Custom

## Role Boundaries

### Primary Responsibilities
- API documentation
- User guides
- Architecture documentation
- README files

### NOT My Responsibilities
- Code implementation - that's Developer's job
- Code review - that's Reviewer's job
```

## Registration

Custom agents are automatically registered when placed in this directory. No additional configuration is required.

To verify registration, use the `services` command with Conductor.
