# VocTrainer

A vocabulary trainer web app built with Blazor Server and PostgreSQL. Database running on my RaspberryPi 4

## Features

- List all the vocabulary from the database and filter it by date
- Quiz mode (German &rarr; English and English &rarr; German)
- Database management tab for adding, editing and deleting words from the database (PIN protected)

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

   VocTrainer/appsettings.Example.json &rarr; VocTrainer/appsettings.json

2. Fill in your PostgreSQL connection string and PIN hash.

The PIN hash is a SHA-256 hex digest of your desired PIN

### Run

The app locally starts at `http://localhost:5222`.

### Docker

1. Copy and fill in the environment file:
   compose.example.yaml &rarr; compose.yaml

   .env.example &rarr; .env

2. Start the container:

## License

MIT
