## What does this PR do?

## Type of change
- [ ] Bug fix
- [ ] New feature
- [ ] Refactoring
- [ ] Documentation

## Architecture Checklist
- [ ] Domain has no external dependencies
- [ ] Business logic is in Domain entities, not handlers
- [ ] New commands have FluentValidation validators
- [ ] Architecture tests pass
- [ ] No direct DbContext usage in Application layer
- [ ] DateTimeOffset used (not DateTime)
- [ ] EF configs use Fluent API only (no Data Annotations)

## Testing
- [ ] Unit tests added/updated
- [ ] Integration tests added/updated
- [ ] All tests pass locally
