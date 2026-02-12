# Requirement to Code Workflow

## Overview

This workflow transforms a product requirement document (PRD) into implemented, reviewed, and tested code.

---

## Workflow Definition

```yaml
workflow:
  id: requirement-to-code
  name: "Requirement to Code"
  version: "1.0"
  description: "Complete workflow from PRD to production-ready code"

triggers:
  - type: user_request
    patterns:
      - "implement feature"
      - "build from PRD"
      - "create new functionality"
      - "develop requirement"
```

---

## Phases

### Phase 1: Orchestration Setup

**Agent**: Conductor

**Purpose**: Initialize workflow and validate inputs

**Activities**:
1. Receive and validate PRD document
2. Identify scope and complexity
3. Determine which agents are needed
4. Create execution plan
5. Set up tracking

**Input**:
- PRD document (`requirements/{feature}.md`)
- Project context (`context/project-structure.yaml`)

**Output**:
- Execution plan
- Agent assignments
- Timeline estimate

**Transition**:
- Success → Phase 2 (Analysis)
- Invalid PRD → Request clarification

---

### Phase 2: Requirement Analysis

**Agent**: Analyst

**Purpose**: Deep analysis of requirements and domain concepts

**Activities**:
1. Parse PRD for functional requirements
2. Identify domain concepts
3. Extract acceptance criteria
4. Note ambiguities and assumptions
5. Produce analysis document

**Input**:
- PRD document
- Existing domain model (if any)
- Pattern knowledge (`knowledge/patterns/${pattern}/`)

**Output**:
- Analyzed requirements (`changes/{change-id}/analysis.md`)
- Domain concept map
- Questions/clarifications needed

**Transition**:
- Analysis complete → Phase 3 (Design)
- Clarifications needed → Pause for user input

---

### Phase 3: Technical Design

**Agent**: Architect

**Purpose**: Create technical design for implementation

**Activities**:
1. Map requirements to technical components
2. Design data model (entities, value objects)
3. Define interfaces and contracts
4. Plan integration points
5. Consider non-functional requirements
6. Produce design document

**Input**:
- Analyzed requirements
- Current architecture (`context/architecture-model.yaml`)
- Pattern guidelines (`knowledge/patterns/${pattern}/`)
- Project conventions (`context/project-structure.yaml`)

**Output**:
- Technical design (`changes/{change-id}/design.md`)
- Component specifications
- Interface definitions

**Transition**:
- Design approved → Phase 4 (Implementation)
- Design needs revision → Iterate

---

### Phase 4: Implementation

**Agent**: Developer

**Purpose**: Write production code based on design

**Activities**:
1. Create/modify code files
2. Implement domain model
3. Implement application logic
4. Add infrastructure code
5. Follow coding standards
6. Add necessary tests

**Input**:
- Technical design
- Code templates (`templates/${adapter}/`)
- Coding standards (`knowledge/core/code-quality.md`)
- Pattern guidance

**Output**:
- Implemented code files
- Unit tests
- Implementation notes

**Transition**:
- Implementation complete → Phase 5 (Review)
- Blocked → Request Advisor help

---

### Phase 5: Code Review

**Agent**: Reviewer

**Purpose**: Ensure code quality and standards compliance

**Activities**:
1. Review code against design
2. Check pattern compliance
3. Verify coding standards
4. Assess security considerations
5. Evaluate maintainability
6. Provide feedback

**Input**:
- Implemented code
- Technical design
- Review checklist (`knowledge/core/review-checklist.md`)
- Pattern review checklist (`knowledge/patterns/${pattern}/review-checklist.md`)

**Output**:
- Review report (`changes/{change-id}/review.md`)
- Issues list with severity
- Approval status

**Transition**:
- Approved → Phase 6 (Testing)
- Changes needed → Return to Phase 4
- Major issues → Return to Phase 3

---

### Phase 6: Testing

**Agent**: Tester

**Purpose**: Verify implementation meets requirements

**Activities**:
1. Write/verify unit tests
2. Create integration tests
3. Verify acceptance criteria
4. Check edge cases
5. Document test coverage

**Input**:
- Implemented code
- Acceptance criteria
- Test templates

**Output**:
- Test suite
- Test results
- Coverage report
- QA sign-off

**Transition**:
- All tests pass → Phase 7 (Completion)
- Tests fail → Return to Phase 4

---

### Phase 7: Completion

**Agent**: Conductor

**Purpose**: Finalize and document the change

**Activities**:
1. Verify all phases complete
2. Compile change summary
3. Update project documentation
4. Archive change records
5. Report completion

**Input**:
- All phase outputs
- Original PRD

**Output**:
- Change summary (`changes/{change-id}/summary.md`)
- Updated documentation
- Completion notification

---

## Workflow Diagram

```
┌──────────────────────────────────────────────────────────────────┐
│                         START                                     │
│                     (PRD Received)                                │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 1: CONDUCTOR - Setup                                       │
│  • Validate PRD                                                   │
│  • Create execution plan                                          │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 2: ANALYST - Requirements Analysis                         │
│  • Parse requirements                                             │
│  • Identify domain concepts                                       │
│  • Extract acceptance criteria                                    │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 3: ARCHITECT - Technical Design                            │
│  • Design components                                              │
│  • Define interfaces                                              │
│  • Plan implementation                                            │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 4: DEVELOPER - Implementation                              │
│  • Write code                                                     │
│  • Follow patterns                                                │
│  • Add tests                                                      │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 5: REVIEWER - Code Review                                  │
│  • Check quality                         ◄────────────────┐      │
│  • Verify standards                      Changes needed   │      │
│  • Approve/reject                        ─────────────────┘      │
└─────────────────────────────┬────────────────────────────────────┘
                              │ Approved
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 6: TESTER - Quality Assurance                              │
│  • Run tests                             ◄────────────────┐      │
│  • Verify acceptance criteria            Tests fail       │      │
│  • Sign off                              ─────────────────┘      │
└─────────────────────────────┬────────────────────────────────────┘
                              │ All pass
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 7: CONDUCTOR - Completion                                  │
│  • Compile summary                                                │
│  • Update documentation                                           │
│  • Archive records                                                │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│                          END                                      │
│                   (Feature Delivered)                             │
└──────────────────────────────────────────────────────────────────┘
```

---

## Quality Gates

| Gate | Criteria | Owner |
|------|----------|-------|
| G1: Requirements | Clear, complete, testable | Analyst |
| G2: Design | Follows patterns, addresses requirements | Architect |
| G3: Code | Passes review checklist | Reviewer |
| G4: Tests | All tests pass, adequate coverage | Tester |

---

## Artifacts Summary

| Phase | Artifact | Location |
|-------|----------|----------|
| 1 | Execution Plan | `changes/{id}/plan.md` |
| 2 | Analysis | `changes/{id}/analysis.md` |
| 3 | Design | `changes/{id}/design.md` |
| 4 | Code | Project source directories |
| 5 | Review | `changes/{id}/review.md` |
| 6 | Tests | Project test directories |
| 7 | Summary | `changes/{id}/summary.md` |

---

## Configuration Options

```yaml
options:
  # Skip phases for simple changes
  skip_analysis: false    # Skip for trivial changes
  skip_review: false      # Not recommended
  skip_testing: false     # Not recommended
  
  # Quality thresholds
  test_coverage_min: 80   # Minimum test coverage %
  review_must_pass: true  # Require review approval
  
  # Iteration limits
  max_review_iterations: 3
  max_test_iterations: 3
```
