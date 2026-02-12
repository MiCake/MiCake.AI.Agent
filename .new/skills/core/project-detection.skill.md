# Project Detection Skill

Automatically detect project structure, technology stack, and recommend configuration.

---

## Skill Definition

```yaml
skill:
  id: project-detection
  name: "Project Detection"
  version: "1.0"
  description: "Detect project structure and technology stack"
```

---

## Context Loading

```yaml
context:
  required: []  # No pre-loaded context - this skill discovers context
  optional:
    - knowledge/patterns/*/manifest.yaml  # Available patterns for matching
  never:
    - knowledge/*              # Not needed for detection
    - templates/*              # Not needed for detection
    - requirements/*           # Not needed for detection
```

**Context Size**: ~1-2KB (discovery-focused)

---

## Capabilities

### 1. Technology Detection

Detect technology stack:
- Programming language
- Framework
- Package manager
- Build tools

### 2. Architecture Detection

Identify architecture:
- Project structure
- Layer organization
- Module boundaries

### 3. Pattern Recognition

Recognize patterns in use:
- DDD indicators
- Clean Architecture indicators
- CQRS indicators
- Layered architecture

### 4. Configuration Generation

Generate configuration:
- Adapter selection
- Pattern selection
- Project structure mapping

---

## Inputs

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `project_root` | string | Yes | Root directory to analyze |
| `depth` | enum | No | `quick`, `standard`, `thorough` |

---

## Outputs

| Name | Type | Description |
|------|------|-------------|
| `technology` | object | Detected technology stack |
| `architecture` | object | Detected architecture |
| `patterns` | array | Detected patterns |
| `recommended_config` | object | Suggested configuration |
| `confidence` | number | Detection confidence (0-100) |

---

## Usage

### Conductor Auto-Configuration

```yaml
invoke: project-detection
inputs:
  project_root: "."
  depth: standard
```

---

## Detection Indicators

### Technology Indicators

| Indicator | Technology |
|-----------|------------|
| `*.csproj`, `*.sln` | .NET |
| `tsconfig.json`, `package.json` (with TS) | TypeScript |
| `requirements.txt`, `setup.py`, `pyproject.toml` | Python |
| `pom.xml`, `build.gradle` | Java |

### Framework Indicators

| Indicator | Framework |
|-----------|-----------|
| `Microsoft.AspNetCore` | ASP.NET Core |
| `Microsoft.EntityFrameworkCore` | EF Core |
| `@nestjs/core` | NestJS |
| `express` | Express.js |
| `fastapi`, `django`, `flask` | Python Web |

---

## Example Output

```yaml
detection_result:
  technology:
    language: "csharp"
    version: "12.0"
    framework: "aspnetcore"
    framework_version: "8.0"
    orm: "efcore"
    test_framework: "xunit"
    package_manager: "nuget"
    
  architecture:
    style: "layered"
    layers:
      - name: "Domain"
        path: "src/Company.Domain"
        type: "domain"
        
      - name: "Application"
        path: "src/Company.Application"
        type: "application"
        
      - name: "Infrastructure"
        path: "src/Company.Infrastructure"
        type: "infrastructure"
        
      - name: "WebApi"
        path: "src/Company.WebApi"
        type: "presentation"
        
  patterns:
    detected:
      - pattern: "ddd"
        confidence: 85
        evidence:
          - "Domain/Entities/ folder exists"
          - "Aggregate root base class found"
          - "Domain events present"
          
      - pattern: "repository"
        confidence: 90
        evidence:
          - "IRepository interface defined"
          - "Repository implementations exist"
          
  recommended_config:
    adapter: "dotnet"
    pattern: "ddd"
    
    project_structure:
      project:
        name: "Company.Product"
        root: "."
      modules:
        - name: "Domain"
          path: "src/Company.Domain"
          type: "domain"
        - name: "Application"
          path: "src/Company.Application"
          type: "application"
        
    architecture_model:
      style: "layered"
      pattern: "ddd"
      layers:
        - name: "Domain"
          level: 0
          
  confidence: 87
  
  notes:
    - "Strong DDD indicators found"
    - "Consider adding domain events if not present"
    - "CQRS pattern not detected but could be added"
```

---

## Auto-Configuration Workflow

When user invokes auto-configuration:

```
1. Conductor receives request
2. Invoke project-detection skill
   ├── Scan project root
   ├── Identify technology
   ├── Detect architecture
   └── Recognize patterns
3. Present findings to user
4. On confirmation:
   ├── Generate context/project-structure.yaml
   ├── Generate context/architecture-model.yaml
   └── Update manifest.yaml
5. Report completion
```

---

## Integration

This skill is used by:
- **Conductor**: For auto-configuration feature
