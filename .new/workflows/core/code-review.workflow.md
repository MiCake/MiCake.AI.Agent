# Code Review Workflow

## Overview

This workflow provides systematic code review for pull requests or code changes outside of the full implementation workflow.

---

## Workflow Definition

```yaml
workflow:
  id: code-review
  name: "Code Review"
  version: "1.0"
  description: "Independent code review workflow"

triggers:
  - type: user_request
    patterns:
      - "review code"
      - "check my code"
      - "review pull request"
      - "code review"
```

---

## Phases

### Phase 1: Review Setup

**Agent**: Conductor

**Purpose**: Prepare for code review

**Activities**:
1. Identify code to review
2. Load relevant context
3. Determine review type
4. Assign appropriate reviewer focus
5. Load applicable checklists

**Input**:
- Code files or PR reference
- Review scope (specific concerns or general)
- Project context

**Output**:
- Review scope definition
- Applicable checklists
- Context loaded

**Transition**:
- Setup complete → Phase 2

---

### Phase 2: Static Analysis

**Agent**: Reviewer

**Purpose**: Automated and systematic code checks

**Activities**:
1. Check code formatting
2. Verify naming conventions
3. Identify code smells
4. Check complexity metrics
5. Scan for security issues
6. Verify documentation

**Input**:
- Code files
- Coding standards
- Pattern-specific rules

**Output**:
- Static analysis findings
- Categorized issues
- Auto-fixable items identified

**Transition**:
- Analysis complete → Phase 3

---

### Phase 3: Design Review

**Agent**: Reviewer (+ Architect for complex cases)

**Purpose**: Review architectural and design decisions

**Activities**:
1. Verify pattern compliance
2. Check SOLID principles
3. Review abstractions
4. Assess cohesion/coupling
5. Check separation of concerns
6. Evaluate extensibility

**Input**:
- Code structure
- Pattern guidelines
- Architecture principles

**Output**:
- Design review findings
- Architectural concerns
- Improvement suggestions

**Transition**:
- Design review complete → Phase 4

---

### Phase 4: Logic Review

**Agent**: Reviewer

**Purpose**: Review business logic correctness

**Activities**:
1. Verify logic correctness
2. Check edge cases handling
3. Review error handling
4. Assess null safety
5. Check boundary conditions
6. Verify state management

**Input**:
- Code logic
- Requirements (if available)
- Domain knowledge

**Output**:
- Logic review findings
- Correctness concerns
- Suggestions

**Transition**:
- Logic review complete → Phase 5

---

### Phase 5: Report Generation

**Agent**: Reviewer

**Purpose**: Compile comprehensive review report

**Activities**:
1. Aggregate all findings
2. Categorize by severity
3. Prioritize issues
4. Provide actionable feedback
5. Include positive observations
6. Generate summary

**Input**:
- All review phase outputs

**Output**:
- Complete review report
- Issue summary
- Approval status

**Transition**:
- Report complete → Phase 6

---

### Phase 6: Feedback Delivery

**Agent**: Conductor

**Purpose**: Deliver review results and coordinate fixes

**Activities**:
1. Present review findings
2. Discuss with developer (if needed)
3. Coordinate fixes
4. Track resolution
5. Re-review if necessary

**Input**:
- Review report
- Developer questions

**Output**:
- Delivered feedback
- Resolution tracking
- Final approval

---

## Review Types

| Type | Focus | Depth |
|------|-------|-------|
| **Quick** | Critical issues only | Surface |
| **Standard** | All categories | Moderate |
| **Deep** | Comprehensive analysis | Thorough |
| **Security** | Security-focused | Specialized |
| **Performance** | Performance-focused | Specialized |

---

## Review Checklist Categories

### 1. Formatting & Style
- [ ] Consistent formatting
- [ ] Follows naming conventions
- [ ] Appropriate comments
- [ ] No commented-out code
- [ ] Meaningful names

### 2. Code Quality
- [ ] No code duplication
- [ ] Appropriate abstractions
- [ ] Single responsibility
- [ ] Clean code principles
- [ ] Readable and maintainable

### 3. Design & Architecture
- [ ] Follows project patterns
- [ ] Proper layer separation
- [ ] Appropriate dependencies
- [ ] Interface usage
- [ ] Extensibility considered

### 4. Logic & Correctness
- [ ] Logic is correct
- [ ] Edge cases handled
- [ ] Null checks present
- [ ] Error handling proper
- [ ] State managed correctly

### 5. Security
- [ ] Input validation
- [ ] No sensitive data exposure
- [ ] Proper authorization
- [ ] SQL injection prevention
- [ ] XSS prevention

### 6. Performance
- [ ] No obvious bottlenecks
- [ ] Efficient algorithms
- [ ] Appropriate caching
- [ ] Resource management
- [ ] Async where appropriate

### 7. Testing
- [ ] Tests included
- [ ] Tests meaningful
- [ ] Edge cases tested
- [ ] Mocking appropriate
- [ ] Coverage adequate

---

## Severity Levels

| Level | Description | Action Required |
|-------|-------------|-----------------|
| **Critical** | Security vulnerability, data loss risk | Block merge |
| **Major** | Bug, significant design flaw | Must fix |
| **Minor** | Style issue, minor improvement | Should fix |
| **Suggestion** | Nice to have improvement | Consider |
| **Positive** | Good practice observed | Acknowledge |

---

## Review Report Format

```markdown
# Code Review Report

## Summary
- **Status**: Approved / Approved with Comments / Changes Requested / Rejected
- **Files Reviewed**: X
- **Issues Found**: Critical: X | Major: X | Minor: X

## Critical Issues
[List critical issues requiring immediate attention]

## Major Issues
[List major issues that must be fixed]

## Minor Issues
[List minor issues that should be addressed]

## Suggestions
[List optional improvements]

## Positive Observations
[Acknowledge good practices]

## Overall Assessment
[Summary paragraph]
```

---

## Workflow Diagram

```
┌──────────────────────────────────────────────────────────────────┐
│                    CODE SUBMITTED                                 │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 1: Setup                                                   │
│  • Identify scope                                                 │
│  • Load context                                                   │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 2: Static Analysis                                         │
│  • Format checks                                                  │
│  • Code smells                                                    │
│  • Security scan                                                  │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 3: Design Review                                           │
│  • Pattern compliance                                             │
│  • SOLID principles                                               │
│  • Architecture fit                                               │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 4: Logic Review                                            │
│  • Correctness                                                    │
│  • Edge cases                                                     │
│  • Error handling                                                 │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 5: Report Generation                                       │
│  • Aggregate findings                                             │
│  • Categorize & prioritize                                        │
│  • Generate report                                                │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│  Phase 6: Feedback Delivery                                       │
│  • Present findings          ◄──────────────┐                    │
│  • Coordinate fixes          Re-review      │                    │
│  • Final approval            ───────────────┘                    │
└─────────────────────────────┬────────────────────────────────────┘
                              │
                              ▼
┌──────────────────────────────────────────────────────────────────┐
│                    REVIEW COMPLETE                                │
└──────────────────────────────────────────────────────────────────┘
```

---

## Configuration

```yaml
review_config:
  # Default review type
  default_type: "standard"
  
  # Auto-block settings
  block_on_critical: true
  block_on_major: false
  
  # Include categories
  categories:
    - formatting
    - code_quality
    - design
    - logic
    - security
    - performance
    - testing
  
  # Skip categories (override)
  skip_categories: []
  
  # Severity thresholds
  max_minor_issues: 10
  require_all_critical_fixed: true
```
