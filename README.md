# TaskManagement — Setup Guide

## Ön Koşullar
- VS Code + GitHub Copilot extension (Chat + Agent Mode)
- .NET 10 SDK
- Docker (PostgreSQL için)
- Git

---

## Repo Yapısı (başlangıç)

Bu dosyaları GitHub repo'na push'la. Copilot Agent bunları okuyarak projeyi oluşturacak.

```
TaskManagement/
├── .github/
│   └── copilot-instructions.md    ← Copilot HER mesajda otomatik okur
├── prompts/
│   ├── step-0-scaffold.md         ← Solution + projeler + paketler
│   ├── step-1-domain.md           ← Domain katmanı (28 dosya)
│   ├── step-2-application.md      ← Application katmanı (~100 dosya)
│   ├── step-3-infrastructure.md   ← Infrastructure katmanı (~25 dosya)
│   ├── step-4-webapi.md           ← WebApi katmanı (~45 dosya)
│   ├── step-5-tests.md            ← Tüm testler (~175 dosya)
│   └── step-6-finalize.md         ← CI, Docker, PR template
└── README.md                      ← Bu dosya
```

---

## Adım Adım Uygulama (VS Code Copilot Agent Mode)

### Hazırlık
1. VS Code'da projeyi aç
2. Copilot Chat panelini aç (Ctrl+Shift+I veya sidebar'dan)
3. Chat modunu **"Agent"** olarak seç (dropdown'dan)

---

### ADIM 0: Solution Scaffold
Copilot Chat'e şunu yaz:

```
Read prompts/step-0-scaffold.md and execute ALL phases in it sequentially. 
Run every terminal command yourself. Create all files yourself.
At the end, run `dotnet build` and verify it succeeds.
```

> Copilot her terminal komutu öncesi onay isteyecek → "Continue" de.
> Bittikten sonra `dotnet build` başarılı olmalı.
> **Git commit:** `git add -A && git commit -m "chore: initial solution scaffold"`

---

### ADIM 1: Domain Layer
Copilot Chat'e şunu yaz:

```
Read prompts/step-1-domain.md and generate ALL 28 files listed in it.
Follow the folder structure exactly. Place files under src/Domain/.
Use the namespace prefix TaskManagement.Domain.
After creating all files, run `dotnet build` to verify.
```

> Domain projesinde 0 NuGet paketi olmalı — Copilot bunu copilot-instructions.md'den bilecek.
> **Git commit:** `git add -A && git commit -m "feat: domain layer - aggregates, entities, value objects"`

---

### ADIM 2: Application Layer
Copilot Chat'e şunu yaz:

```
Read prompts/step-2-application.md and generate ALL files listed in it.
Follow the folder structure exactly. Place files under src/Application/.
Use the namespace prefix TaskManagement.Application.
After creating all files, run `dotnet build` to verify.
```

> ~100 dosya. Büyük iş — Copilot context limitine takılırsa bölümlere ayır:
> - "Generate the Common folder files from step-2-application.md"
> - "Generate the Projects feature files from step-2-application.md"
> - "Generate the Tasks feature files from step-2-application.md"
> - "Generate the Teams feature files from step-2-application.md"
> 
> **Git commit:** `git add -A && git commit -m "feat: application layer - CQRS, validators, behaviors"`

---

### ADIM 3: Infrastructure Layer
Copilot Chat'e şunu yaz:

```
Read prompts/step-3-infrastructure.md and generate ALL files listed in it.
Follow the folder structure exactly. Place files under src/Infrastructure/.
Use the namespace prefix TaskManagement.Infrastructure.
After creating all files, run `dotnet build` to verify.
```

> **Git commit:** `git add -A && git commit -m "feat: infrastructure layer - EF Core, repositories, AWS services"`

---

### ADIM 4: WebApi Layer
Copilot Chat'e şunu yaz:

```
Read prompts/step-4-webapi.md and generate ALL files listed in it.
Follow the folder structure exactly. Place files under src/WebApi/.
Use the namespace prefix TaskManagement.WebApi.
After creating all files, run `dotnet build` to verify.
```

> **Git commit:** `git add -A && git commit -m "feat: webapi layer - controllers, middleware, configuration"`

---

### ADIM 5: Tests
Copilot Chat'e şunu yaz:

```
Read prompts/step-5-tests.md and generate ALL test files.
Follow the folder structure exactly.
After creating all files, run `dotnet build` and `dotnet test` to verify.
```

> ~175 dosya. Kesinlikle bölümlere ayır:
> - "Generate Domain.UnitTests from step-5-tests.md"
> - "Generate Application.UnitTests for Projects from step-5-tests.md"
> - "Generate Application.UnitTests for Tasks from step-5-tests.md"
> - "Generate Application.UnitTests for Teams from step-5-tests.md"
> - "Generate Infrastructure.IntegrationTests from step-5-tests.md"
> - "Generate Api.IntegrationTests from step-5-tests.md"
> 
> **Git commit:** `git add -A && git commit -m "test: unit and integration tests"`

---

### ADIM 6: Finalize
Copilot Chat'e şunu yaz:

```
Read prompts/step-6-finalize.md and execute ALL phases.
Create CI pipeline, Dockerfile, docker-compose, PR template.
Run final `dotnet build && dotnet test` to verify everything works.
```

> **Git commit:** `git add -A && git commit -m "chore: CI pipeline, Docker, PR template"`

---

## Sorun Giderme

### Copilot context limitine takılırsa
Prompt'ları daha küçük parçalara böl. Örnek:
```
From prompts/step-2-application.md, generate ONLY the files under 
Application/Projects/Commands/. Follow the same rules.
```

### Build hataları
```
Fix all build errors. Run `dotnet build` and resolve each error.
Do NOT change the project references or add packages to Domain.
```

### Copilot yanlış katmana kod koyarsa
copilot-instructions.md'deki kurallar bunu önlemeli, ama olursa:
```
This file violates the dependency rule. [Entity].cs is in Infrastructure but 
uses Domain types directly. Move the logic to the correct layer as defined 
in .github/copilot-instructions.md.
```

---

## Sonraki Adımlar

Proje oluşturulduktan sonra yeni feature eklemek için Copilot'a:

```
I need a new feature: User aggregate with registration and profile management.
Follow the patterns in .github/copilot-instructions.md.
Create Domain entity, Application commands/queries, Infrastructure repository,
WebApi controller, and tests — in that order.
```

Copilot instructions dosyasını okuduğu için tüm kuralları otomatik uygulayacak.
