---
title: Radical WPF MVVM framework — project structure and build config
tags:
  - radical
  - wpf
  - mvvm
  - dotnet
  - build
  - warnings
lifecycle: permanent
createdAt: '2026-03-12T19:20:48.366Z'
updatedAt: '2026-03-12T19:20:48.366Z'
project: https-github-com-radicalfx-radical
projectName: Radical
memoryVersion: 1
---
# Radical — WPF MVVM Framework

.NET infrastructure framework for WPF MVVM applications.

## Project Structure

- Solution: `/src/Radical.sln`
- Main library: `/src/Radical/Radical.csproj` (targets `netstandard2.0`)
- Tests: `/src/Radical.Tests/Radical.Tests.csproj` (targets `net8.0;net9.0;net10.0`)
- Source files use descriptive names with spaces and parentheses, e.g. `EntityCollection.(Generic).cs`
- Key areas: ComponentModel, Model, Observers, Validation, Diagnostics, Threading, Helpers

## Build Configuration

- `GenerateDocumentationFile` is enabled → CS1591 (missing XML docs) suppressed globally via `<NoWarn>1591</NoWarn>`
- Release-only NoWarn: `1701;1702;NU5105`
- FxCopAnalyzers removed (deprecated) — .NET SDK built-in analyzers are used instead

## Warning Fixes (2026-03-11, branch: fix-warnings)

Branch cleaned up all build warnings → 0 warnings, 0 errors.

- CS1723: `<see cref="T"/>` → `<typeparamref name="T"/>` in EntityCollection.(Generic).cs
- CS1584/CS1658: Fixed backtick in cref (``ICollection`1`` → `ICollection{T}`)
- CS1591: Moved to global NoWarn (was Release-only)
- CA9998: Removed deprecated `Microsoft.CodeAnalysis.FxCopAnalyzers` package
- CA1805: Removed explicit `= null/false/0` initializations across 11 files
- CA1825: `new ValidationError[0]` → `Array.Empty<ValidationError>()`
- CA1200: Removed `T:`, `M:`, `P:`, `E:` prefixes from cref attributes in 7 files
- CA1304/CA1305: Added `CultureInfo.InvariantCulture` to string operations in Ensure and TraceSourceExtensions
- CA1062: Added null parameter validation in ValidationResults.AddError and TraceSourceExtensions.Error methods
