# Requirement Change Workflow

## Overview

This workflow handles modifications to existing implementations when requirements change.

---

## Workflow Definition

```yaml
workflow:
  id: requirement-change
  name: "Requirement Change"
  version: "1.0"
  description: "Modify existing code based on changed requirements"

triggers:
  - type: user_request
    patterns:
      - "change requirement"
      - "modify feature"
      - "update implementation"
      - "requirement changed"
```

---

## Key Differences from New Implementation

| Aspect | New Implementation | Requirement Change |
|--------|-------------------|-------------------|
| Starting Point | PRD only | PRD + Existing code |
| Analysis | Create new design | Impact analysis |
| Implementation | Build from scratch | Modify existing |
| Risk | Lower | Higher (regression) |
| Testing | New tests | New + regression tests |

---

## Phases

### Phase 1: Change Request Analysis

**Agent**: Conductor

**Purpose**: Understand the change and its potential impact

**Activities**:
1. Receive change request
2. Identify affected components
3. Assess change complexity
4. Determine scope (minor/major)
5. Create change tracking record

**Input**:
- Change request document
- Current implementation context
- Original requirements (if available)

**Output**:
- Change scope assessment
- Affected component list
- Risk assessment
- Change tracking ID

**Transition**:
- Minor change → Phase 3 (skip detailed analysis)
- Major change → Phase 2

---

### Phase 2: Impact Analysis

**Agent**: Analyst + Architect

**Purpose**: Detailed analysis of change impact

**Activities**:
1. Map changed requirements to code
2. Identify all affected modules
3. Assess backward compatibility
4. Identify potential breaking changes
5. Document dependencies

**Input**:
- Change request
- Current codebase structure
- Architecture documentation

**Output**:
- Impact analysis report (`changes/{id}/impact-analysis.md`)
- Dependency map
- Risk mitigation plan

**Transition**:
- Analysis complete → Phase 3
- High risk identified → Request user confirmation

---

### Phase 3: Design Changes

**Agent**: Architect

**Purpose**: Design the modification approach

**Activities**:
1. Review current implementation
2. Design minimal change approach
3. Ensure pattern compliance
4. Plan backward compatibility
5. Define migration steps (if needed)

**Input**:
- Impact analysis
- Current design documents
- Pattern guidelines

**Output**:
- Change design document (`changes/{id}/change-design.md`)
- Migration plan (if applicable)
- Updated interfaces (if needed)

**Transition**:
- Design complete → Phase 4
- Breaking changes → User approval required

---

### Phase 4: Implementation

**Agent**: Developer

**Purpose**: Implement the changes

**Activities**:
1. Modify existing code
2. Maintain backward compatibility
3. Update/add tests
4. Document changes
5. Handle deprecations

**Input**:
- Change design
- Current code
- Coding standards

**Output**:
- Modified code
- Updated tests
- Change log

**Transition**:
- Implementation complete → Phase 5

---

### Phase 5: Regression Review

**Agent**: Reviewer

**Purpose**: Ensure changes don't break existing functionality

**Activities**:
1. Review code changes
2. Check for unintended side effects
3. Verify backward compatibility
4. Assess regression risk
5. Review test coverage

**Input**:
- Code changes (diff)
- Impact analysis
- Original tests

**Output**:
- Review report with regression focus
- Regression risk assessment
- Approval/rejection

**Transition**:
- Approved → Phase 6
- Issues found → Return to Phase 4

---

### Phase 6: Regression Testing

**Agent**: Tester

**Purpose**: Verify no regression introduced

**Activities**:
1. Run existing test suite
2. Add tests for new behavior
3. Verify edge cases
4. Check affected integrations
5. Validate acceptance criteria

**Input**:
- Modified code
- Existing tests
- New acceptance criteria

**Output**:
- Test results
- Regression report
- Coverage comparison

**Transition**:
- All pass → Phase 7
- Regression found → Return to Phase 4

---

### Phase 7: Completion

**Agent**: Conductor

**Purpose**: Finalize the change

**Activities**:
1. Verify all checks passed
2. Update documentation
3. Update change log
4. Archive change records
5. Notify completion

**Input**:
- All phase outputs

**Output**:
- Change summary
- Updated documentation
- Completion notification

---

## Workflow Diagram

```
┌──────────────────────────────────────────────────────────────────┐
│                    CHANGE REQUEST                                 │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 1: CONDUCTOR - Change Assessment                           │
│  • Identify scope                                                 │
│  • Assess complexity                                              │
│  • Create tracking                                                │
└─────────────────────────────┬────────────────────────────────────┘
                              │
              ┌───────────────┴───────────────┐
              │ Major?                        │ Minor
              ▼                               │
┌─────────────────────────────────┐           │
│  Phase 2: Impact Analysis        │           │
│  • Map dependencies              │           │
│  • Identify risks                │           │
│  • Plan mitigation               │           │
└─────────────────┬───────────────┘           │
                  │                           │
                  └───────────────┬───────────┘
                                  │
                                  ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 3: ARCHITECT - Change Design                               │
│  • Design modifications                                           │
│  • Plan backward compatibility                                    │
│  • Define migration                                               │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 4: DEVELOPER - Implementation                              │
│  • Modify code                         ◄──────────────────┐      │
│  • Update tests                        Issues found       │      │
│  • Maintain compatibility                                 │      │
└─────────────────────────────┬─────────────────────────────┼──────┘
                              │                             │
                              ▼                             │
┌──────────────────────────────────────────────────────────────────┐
│  Phase 5: REVIEWER - Regression Review                            │
│  • Check side effects                                    ─┘      │
│  • Verify compatibility                                          │
│  • Assess regression risk                                        │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 6: TESTER - Regression Testing                             │
│  • Run all tests                       ◄────────────────┐        │
│  • Check regressions                   Failures         │        │
│  • Validate new behavior               ─────────────────┘        │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 7: CONDUCTOR - Completion                                  │
│  • Update docs                                                    │
│  • Archive records                                                │
│  • Notify complete                                                │
└──────────────────────────────────────────────────────────────────┘
```

---

## Change Categories

| Category | Description | Process |
|----------|-------------|---------|
| **Trivial** | Typo fixes, comment updates | Direct to Implementation |
| **Minor** | Small behavior changes, no interface change | Skip Impact Analysis |
| **Major** | Interface changes, new dependencies | Full process |
| **Breaking** | Incompatible changes | Requires user approval |

---

## Risk Assessment Matrix

| Factor | Low | Medium | High |
|--------|-----|--------|------|
| **Scope** | Single file | Single module | Multiple modules |
| **Interface** | No change | Additive only | Breaking change |
| **Dependencies** | None | Few internal | External systems |
| **Data** | No migration | Simple migration | Complex migration |

---

## Artifacts

| Artifact | Purpose | Location |
|----------|---------|----------|
| Impact Analysis | Document affected components | `changes/{id}/impact-analysis.md` |
| Change Design | Describe modification approach | `changes/{id}/change-design.md` |
| Code Diff | Track actual changes | Version control |
| Regression Report | Document test results | `changes/{id}/regression-report.md` |
| Change Summary | Final documentation | `changes/{id}/summary.md` |

---

## Best Practices

1. **Minimal Change**: Make smallest change that satisfies requirement
2. **Backward Compatibility**: Preserve existing behavior where possible
3. **Deprecation**: Use deprecation before removal
4. **Documentation**: Update all affected documentation
5. **Testing**: Add tests for new behavior, keep regression tests
6. **Communication**: Notify affected teams/systems
