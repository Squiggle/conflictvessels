---
mode: 'agent'
---

Given a task file
1. clarify the number of the task to implement
2. suggest the implementation steps of that task
3. clarify for any further requirements. Each question for clarification should be numbered so that it can be referenced.
4. if all clarifications are done, implement the task by:
    - creating new documentation
    - editing existing documentation
    - creating new code
    - editing existing code

Finally once implemented, prompt to raise a git commit with the changes made.