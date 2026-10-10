# MaxxPro

<div align="center">

[![Execute Buffalo test](https://github.com/Tayra-Sakurai/MaxxPro/actions/workflows/test.yml/badge.svg)](https://github.com/Tayra-Sakurai/MaxxPro/actions/workflows/test.yml)
[![Build WinUI App](https://github.com/Tayra-Sakurai/MaxxPro/actions/workflows/build.yml/badge.svg)](https://github.com/Tayra-Sakurai/MaxxPro/actions/workflows/build.yml)
[![Deploy Github Pages](https://github.com/Tayra-Sakurai/MaxxPro/actions/workflows/github-pages.yml/badge.svg)](https://github.com/Tayra-Sakurai/MaxxPro/actions/workflows/github-pages.yml)
[![GitHub Release](https://img.shields.io/github/v/release/Tayra-Sakurai/MaxxPro?include_prereleases)](https://github.com/Tayra-Sakurai/MaxxPro/releases)
[![GitHub License](https://img.shields.io/github/license/Tayra-Sakurai/MaxxPro)](https://spdx.org/licenses/GPL-3.0-or-later.html)
[![GitHub Issues](https://img.shields.io/github/issues/Tayra-Sakurai/MaxxPro)](https://github.com/Tayra-Sakurai/MaxxPro/issues)
[![GitHub commit activity](https://img.shields.io/github/commit-activity/m/Tayra-Sakurai/MaxxPro)](https://github.com/Tayra-Sakurai/MaxxPro/commits/master)
[![GitHub contributors](https://img.shields.io/github/contributors/Tayra-Sakurai/MaxxPro)](https://github.com/Tayra-Sakurai/MaxxPro/graphs/contributors)

**A local-first, zero-cloud Windows desktop application for emergency and household stockpile management, powered by WinUI 3, .NET 10, and offline AI.**

[Documentation & Homepage](https://tayra-sakurai.github.io/MaxxPro/) • [Quickstart Guide](https://tayra-sakurai.github.io/MaxxPro/docs/getting-started/quickstart) • [Download Releases](https://github.com/Tayra-Sakurai/MaxxPro/releases) • [Architecture](https://tayra-sakurai.github.io/MaxxPro/docs/architecture/overview)

</div>

---

## Overview

**MaxxPro** is a modern Windows desktop application engineered for reliable emergency and everyday stockpile management without dependency on online databases or cloud services.

In times of natural disaster, network failure, or privacy-critical situations, your inventory data must remain accessible and safe. MaxxPro is built on a **Local-First (Zero-Cloud)** architecture: all data is stored securely in an on-device SQLite database. Additionally, MaxxPro features an embedded offline AI assistant (**Cougar**) driven by [Ollama](https://ollama.com/), providing conversational inventory querying and management while ensuring 100% data confidentiality.

---

## Key Features

- **Local-First & Offline Resilience**
  - Works entirely without an internet connection.
  - Zero cloud dependencies or remote accounts required.
  - All inventory data is stored locally in SQLite (`Caiman.db`).

- **Hierarchical Stockpile Categorization**
  - **3-Tier Taxonomy**: Organize supplies by Major &gt; Medium &gt; Minor categories (`LargeCategory`, `MediumCategory`, `SmallCategory`) for intuitive filtering.
  - Track food, potable water, medical supplies, sanitation, batteries, and equipment.

- **Storage Place & Expiration Tracking**
  - Manage inventory across physical locations (Pantry, Basement, Emergency Bag, Garage).
  - Track item counts and expiration dates with proactive monitoring.

- **Cougar: Offline AI Assistant**
  - Powered by local LLMs via [Ollama](https://ollama.com/) (e.g., `gemma4:e2b` and `embeddinggemma:latest`).
  - Vector search and conversation history indexing powered by `sqlite-vec`.
  - **Human-in-the-Loop Safety**: AI actions that create, modify, or delete stockpile data require explicit user approval before execution.
  - Completely optional: MaxxPro functions normally without Ollama installed.

- **Native Windows 11 User Experience**
  - Built with **WinUI 3** and the **Windows App SDK 1.7+** on **.NET 10**.
  - Modern Fluent Design with Mica material and automatic light/dark theme adaptation.
  - Multi-language localization support: Japanese (`ja-JP`), English (`en-US`, `en-GB`).

---

## Solution Architecture

MaxxPro follows a modular architecture separating presentation, domain data access, local AI services, and automated testing:

```
MaxxPro.slnx
├── MaxxPro      # WinUI 3 desktop presentation layer (Views, ViewModels, Mica UI)
├── Caiman       # Domain models, Entity Framework Core SQLite context & migrations
├── Cougar       # Local AI agent service, Ollama tools integration & sqlite-vec memory
├── Buffalo      # Unit and integration test suite for Caiman and Cougar (MSTest)
└── docs         # Docusaurus-powered documentation website and project homepage
```

| Project | Target | Description |
| :--- | :--- | :--- |
| **`MaxxPro`** | `net10.0-windows10.0.26100.0` | Desktop UI application with WinUI 3, Mica backdrop, navigation, and DI setup. |
| **`Caiman`** | `net10.0-windows10.0.26100.0` | Domain entities (`Item`, `Place`, `Category`), EF Core 10 SQLite context, and migrations. |
| **`Cougar`** | `net10.0-windows10.0.26100.0` | Microsoft Agents AI integration, OllamaSharp client, `IModelTool<T>`, and approval gates. |
| **`Buffalo`** | `net10.0-windows10.0.26100.0` | Automated test suite verifying data operations and AI chat behaviors. |

---

## System Requirements

| Component | Requirement |
| :--- | :--- |
| **Operating System** | Windows 11 (Build 22000 or later; Build 26100+ recommended, 64-bit / ARM64) |
| **Runtime** | .NET 10.0 Runtime (bundled automatically in self-contained releases) |
| **Local AI (Optional)** | [Ollama](https://ollama.com/) with models `gemma4:e2b` and `embeddinggemma:latest` |

---

## Installation

Download the latest packaged installer (`.msix` / `.msixbundle`) from the [GitHub Releases](https://github.com/Tayra-Sakurai/MaxxPro/releases) page.

For step-by-step installation instructions, certificate trust, and initial setup, refer to the [Quickstart Guide](https://tayra-sakurai.github.io/MaxxPro/docs/getting-started/quickstart).

---

## Development & Building

### Prerequisites

- **Visual Studio 2026** (or latest Visual Studio with Windows App SDK tooling):
  - `.NET Desktop Development` workload
  - `Windows Application Development (WinUI 3 / Windows App SDK)`
- **.NET 10.0 SDK**
- **Windows 11 SDK** (`10.0.26100.0`)

### Clone the Repository

```powershell
git clone https://github.com/Tayra-Sakurai/MaxxPro.git
cd MaxxPro
```

### Build via Visual Studio

1. Open `MaxxPro.slnx` in Visual Studio.
2. Select configuration (`Debug` or `Release`) and platform (`x64` or `ARM64`).
3. Set `MaxxPro` as the startup project and press `F5`.

### Build via Command Line (MSBuild)

```powershell
# Restore dependencies
msbuild MaxxPro /t:Restore /p:Configuration=Release /p:TargetFramework=net10.0-windows10.0.26100.0

# Build and generate sideload package
msbuild MaxxPro /p:Configuration=Release /p:Platform=x64 /p:UapAppxPackageBuildMode=SideloadOnly /p:AppxBundle=Always /p:GenerateAppxPackageOnBuild=true /p:TargetFramework=net10.0-windows10.0.26100.0
```

### Running Tests

All unit and integration tests are hosted in the **`Buffalo`** project:

```powershell
dotnet test Buffalo/Buffalo.csproj -a x64 -c Debug -p:Platform=x64
```

> [!IMPORTANT]
> **Repository Testing Rules ([AGENTS.md](./AGENTS.md)):**
> - All tests for `Caiman` and `Cougar` must be included in and runnable via `Buffalo/Buffalo.csproj`.
> - Always run `dotnet test Buffalo/Buffalo.csproj` before opening a pull request.
> - Adding new projects to the solution is prohibited.

---

## Documentation & Project Homepage

The project documentation and homepage are built with [Docusaurus](https://docusaurus.io/) and hosted via GitHub Pages:

- **Website**: [https://tayra-sakurai.github.io/MaxxPro/](https://tayra-sakurai.github.io/MaxxPro/)
- **Documentation Source**: [`docs/`](./docs)

To run the documentation site locally:

```bash
cd docs
npm install
npm start
```

---

## Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](./CONTRIBUTING.md) and [CODE_OF_CONDUCT.md](./CODE_OF_CONDUCT.md) before getting started.

Key contribution guidelines:
- Create an issue/discussion before starting work.
- Use the branch naming convention:
  - Non-AI: `[type-prefix]/[pull-request-title]` (e.g. `docs/strengthen-readme`)
  - AI-assisted: `[type-prefix]/[ai-agent-name]/[pull-request-title]` (e.g. `docs/antigravity/strengthen-readme`)
  - Valid prefixes: `fix`, `feat`, `docs`, `ref`, `test`.
- Maintain `CRLF` line endings for Windows compatibility.
- Ensure all tests in `Buffalo/Buffalo.csproj` pass.

---

## License

This project is licensed under the **GNU General Public License v3.0 or later** ([GPL-3.0-or-later](./LICENSE.txt)).

Copyright &copy; 2026 Tayra Sakurai. All rights reserved.
