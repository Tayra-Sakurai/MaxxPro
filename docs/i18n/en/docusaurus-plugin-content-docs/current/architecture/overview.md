---
sidebar_position: 1
---

# Architecture Overview

MaxxPro is structured as a multi-project .NET solution with strict separation of concerns and a local-first architecture.

---

## Solution Projects

```
MaxxPro.slnx
├── MaxxPro      # WinUI 3 Desktop App (Presentation Layer)
├── Caiman       # Domain Models & EF Core SQLite (Data & Domain Layer)
├── Cougar       # Local AI Agent & Function Calling Tools (AI Layer)
└── Buffalo      # Integration & Unit Test Suite (Testing Layer)
```

---

## Component Diagram

```mermaid
graph TB
    subgraph UI["MaxxPro (WinUI 3 / .NET 10)"]
        MainWindow["MainWindow (Mica Backdrop)"]
        BasePage["BasePage (NavigationView)"]
        Views["Views: Items / Places / Categories / Chat"]
        AppXaml["App.xaml.cs (DI Container & Startup)"]
    end

    subgraph Domain["Caiman (.NET 10 Class Library)"]
        Context["CaimanContext (EF Core SQLite)"]
        Models["Models: Item, Place, Categories"]
        ViewModels["ViewModels: ItemsViewModel, etc."]
    end

    subgraph AI["Cougar (.NET 10 Class Library)"]
        Agent["Cougar AIAgent (Microsoft.Agents.AI)"]
        ChatVM["ChatViewModel (CommunityToolkit.Mvvm)"]
        ModelTools["CaimanModelTool (Tool Calling)"]
        Approval["ApprovalRequiredAIFunction (Safety Gate)"]
    end

    subgraph LocalStorage["Local Storage (Windows LocalFolder)"]
        DB[("Caiman.db (SQLite)")]
        VecDB[("chathistory.db (sqlite-vec)")]
    end

    subgraph LocalOllama["Local Inference (Ollama / localhost:11434)"]
        Gemma["gemma4:e2b (LLM)"]
        Emb["embeddinggemma:latest (Embedding)"]
    end

    UI --> Domain
    UI --> AI
    AppXaml --> Context
    AppXaml --> Agent
    Context --> DB
    ModelTools --> Context
    Agent --> LocalOllama
    Agent --> VecDB
```

---

## Project Responsibilities

### 1. MaxxPro (Presentation Layer)
- Built with WinUI 3 and Windows App SDK 1.7.
- Manages top-level windows, Mica backdrop, navigation, and user views.
- Configures dependency injection and triggers database migrations on launch.

### 2. Caiman (Data & Domain Layer)
- Domain entities: `Item`, `Place`, `LargeCategory`, `MediumCategory`, `SmallCategory`.
- EF Core SQLite database context and migrations.
- ViewModels for UI screens.

### 3. Cougar (Local AI Layer)
- Powered by `Microsoft.Agents.AI` and `OllamaSharp`.
- Exposes EF Core operations as AI tools through `CaimanModelTool<TModel>`.
- Enforces user confirmation for destructive actions using `ApprovalRequiredAIFunction`.

### 4. Buffalo (Testing Layer)
- Automated MSTest test suite verifying `Caiman` and `Cougar` components.
