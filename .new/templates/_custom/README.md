# Custom Templates

This directory is for project-specific templates that extend or override the default templates.

## How to Add Custom Templates

### 1. Create Language-Specific Template

```
_custom/
├── dotnet/
│   └── my-custom-template.template.cs
├── java/
│   └── MyCustomTemplate.template.java
└── python/
    └── my_custom_template.template.py
```

### 2. Register in Preferences

In `config/preferences.yaml`:

```yaml
templates:
  custom:
    - name: "my-custom-template"
      path: "templates/_custom/dotnet/my-custom-template.template.cs"
      description: "Custom template for specific project need"
```

### 3. Reference in Workflows

Agents can then reference your template by name in code generation.

## Template Naming Convention

- **Abstract**: `{type}.template.md`
- **.NET**: `{type}.template.cs`
- **Java**: `{Type}.template.java`
- **Python**: `{type}.template.py`

## Example: Custom DTO Template

```csharp
// _custom/dotnet/dto.template.cs

/*
 * DTO Template for ${project_name}
 * 
 * Variables:
 *   ${namespace} - Project namespace
 *   ${dto_name} - DTO class name
 *   ${properties} - Property definitions
 */

namespace ${namespace}.Application.DTOs
{
    /// <summary>
    /// ${dto_description}
    /// </summary>
    public record ${dto_name}
    {
        ${properties}
    }
}
```

## Tips

1. Use consistent variable naming across templates
2. Include documentation comments for generated code
3. Follow project coding standards
4. Test templates with various inputs
