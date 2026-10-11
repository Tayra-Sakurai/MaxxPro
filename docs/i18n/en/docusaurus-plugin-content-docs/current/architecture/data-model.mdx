---
sidebar_position: 2
---

# Data Model

MaxxPro persists all stockpile entities locally in SQLite (`Caiman.db`).

---

## Entity-Relationship Diagram

```mermaid
erDiagram
    LargeCategory ||--o{ MediumCategory : "has children"
    MediumCategory ||--o{ SmallCategory : "has children"
    SmallCategory ||--o{ Item : "classifies"
    Place ||--o{ Item : "stores"

    LargeCategory {
        int Id PK
        string Name
    }

    MediumCategory {
        int Id PK
        string Name
        int ParentId FK
    }

    SmallCategory {
        int Id PK
        string Name
        int ParentId FK
    }

    Place {
        int Id PK
        string Name
    }

    Item {
        int Id PK
        string Name
        string Description
        DateTimeOffset Life "Expiring Date"
        int CategoryId FK
        int PlaceId FK
    }
```

---

## Entity Details

### Item
Represents a physical stockpile unit.
- `Id`: Unique integer identity.
- `Name`: Title / product label.
- `Description`: Additional notes, quantities, specifications.
- `Life`: Expiration timestamp (`DateTimeOffset`).
- `CategoryId`: Foreign key referencing `SmallCategory`.
- `PlaceId`: Foreign key referencing `Place`.

### Place
Represents a storage room, shelf, or bag where items are kept. Tracks the total item count via `ItemsCount`.

### Category Hierarchy
Structured across three tiers:
- `LargeCategory`: Root category.
- `MediumCategory`: Subcategory under `LargeCategory`.
- `SmallCategory`: Leaf category containing items.
