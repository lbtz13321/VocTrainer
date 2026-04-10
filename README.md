# VocTrainer

A vocabulary trainer web app built with Blazor Server and PostgreSQL. Add, manage and quiz yourself on German-English vocabulary.

## Features

- Browse and search your vocabulary list with date filtering
- Interactive quiz mode (German &rarr; English and English &rarr; German)
- PIN-protected management area for adding, editing and deleting entries
- Dark theme UI
- Docker support

## Tech Stack

- ASP.NET Core Blazor Server (.NET 10)
- PostgreSQL with Dapper
- Bootstrap 5

## Setup

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL server

### Database

Create a `voctrainer` database and the vocabulary table:

```sql
CREATE TABLE vocabulary (
    id SERIAL PRIMARY KEY,
    date DATE NOT NULL,
    german TEXT NOT NULL,
    english TEXT NOT NULL
);
```

### Configuration

1. Copy the example config:
   ```bash
   cp VocTrainer/appsettings.Example.json VocTrainer/appsettings.json
   ```
2. Fill in your PostgreSQL connection string and PIN hash.

The PIN hash is a SHA-256 hex digest of your desired PIN, e.g.:
```bash
echo -n "1234" | sha256sum
```

### Run

```bash
cd VocTrainer
dotnet run
```

The app starts at `http://localhost:5222`.

### Docker

1. Copy and fill in the environment file:
   ```bash
   cp compose.example.yaml compose.yaml
   cp .env.example .env
   # edit .env with your values
   ```
2. Start the container:
   ```bash
   docker compose up -d
   ```

## License

MIT
