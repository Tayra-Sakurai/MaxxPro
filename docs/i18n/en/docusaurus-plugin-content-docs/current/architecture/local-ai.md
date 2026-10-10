---
sidebar_position: 3
---

# Local AI & Tool Architecture

Cougar connects local large language models to MaxxPro's database using function calling and safety wrappers.

---

## Architecture Flow

```mermaid
flowchart LR
    subgraph Host["MaxxPro Application"]
        UI["ChatViewModel"]
        Agent["AIAgent Cougar"]
        ToolWrapper["CaimanModelTool"]
        Safety["ApprovalRequiredAIFunction"]
    end

    subgraph LocalOllama["Ollama Localhost"]
        ChatLLM["gemma4:e2b"]
        Embedder["embeddinggemma:latest"]
    end

    subgraph Store["Local Vector Storage"]
        VecStore["SqliteVectorStore chathistory.db"]
    end

    UI <--> Agent
    Agent <--> ChatLLM
    Agent <--> Embedder
    Agent <--> VecStore
    Agent --> Safety --> ToolWrapper
```

---

## Automatic Tool Registration

`CaimanModelTool<TModel>` generates AI functions for each domain entity:
- `GetAll<TModel>`: Read-only, executes immediately.
- `AddModelAsync<TModel>`: Adds an entity, wrapped with `ApprovalRequiredAIFunction`.
- `UpdateModelAsync<TModel>`: Updates an entity, wrapped with `ApprovalRequiredAIFunction`.
- `RemoveModelAsync<TModel>`: Deletes an entity, wrapped with `ApprovalRequiredAIFunction`.
