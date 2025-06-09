---
mode: 'agent'
---

Given a description of a new feature for the codebase, create a Product Requirements Document (PRD) in markdown format for that new feature. The PRD should include the following sections:

1. **Title**: A clear and concise title for the feature
2. **Overview**: A brief description of the feature or product, including its purpose and goals, such that it is understandable for a junior software developer
3. **Goals**: Specific, measurable objectives that the feature aims to achieve
4. **Domain details**: Define the domain language that will be used when describing the feature details
5. **User Stories**: A list of single sentence user stories that describe how consumers of the feature will interact with it.
6. **Acceptance Criteria**: A list of conditions that must be met for the feature to be considered complete

Each of these sections should be brief.

Ask follow-up question to clarify any details needed to complete the PRD. Ask these questions in a number list format, nested if necessary.

# Output

The PRD should be saved in the "requirements" directory with the filename format `X-description.md` where:
- `X` is the next sequential number in the existing filenames (or '0' if there are no existing PRDs in that directory).
- `describetion` is a short, descriptive title of the feature in lowercase, with spaces replaced by hyphens.