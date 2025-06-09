---
mode: 'agent'
---

Given a description of a new feature for the codebase, create a list of technical tasks in markdown format that need to be completed to implement the feature.

The tasks should be detailed enough for a junior software developer to understand what needs to be done.

Ask follow-up questions to clarify any details needed to complete the task list. Ask these questions in a numbered list format, nested if necessary.

# Output

The tasks should be saved in the "tasks" directory with the filename format `X-description.md` where:
- `X` is the next sequential number in the existing filenames (or '0' if there are no existing tasks in that directory).
- `description` is a short, descriptive title of the feature in lowercase, with spaces replaced by hyphens.

The content of the file should be a numbered markdown list of tasks as a series of checks, prepended with `[ ]` to indicate the tasks are incomplete. Each task should be a single line and should be clear and actionable.

The tasks should accommodate the technical and domain context of this application.