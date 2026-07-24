# Trade Finance Lite

A simplified Letter of Credit (LC) management API built to demonstrate core banking/trade-finance workflows — Maker-Checker approval, AML-style screening, and audit logging — using ASP.NET Core.

## Overview

This project simulates a real-world trade finance system where:
- **Makers** create Letters of Credit
- **Checkers** independently review and approve/reject them (dual-control, a standard banking practice)
- Every LC is automatically screened against a sanctions/watchlist
- Every action is logged in an audit trail

## Tech Stack

- **Backend:** ASP.NET Core 9 Web API (C#)
- **Database:** SQL Server + Entity Framework Core (Code-First)
- **Auth:** JWT Bearer authentication with role-based authorization
- **Testing:** xUnit
- **API Docs:** Swagger / OpenAPI

## Features

- **JWT Authentication** — Register/Login with role-based claims (Maker, Checker, Admin)
- **LC Lifecycle Management** — Create, view, approve, reject Letters of Credit
- **Maker-Checker Workflow** — A Maker cannot approve their own LC; only a Checker can approve/reject
- **AML-style Screening** — Beneficiary names are automatically checked against a watchlist using Levenshtein-distance similarity matching
- **Audit Trail** — Every create/approve/reject action is logged with timestamp and user
- **Unit Tested** — Password hashing and screening logic covered by xUnit tests

## Project Structure

TradeFinanceLite/
├── TradeFinanceLite.Api/ # Main Web API project
│ ├── Controllers/ # Auth & LC controllers
│ ├── Models/ # EF Core entities
│ ├── DTOs/ # Request/response contracts
│ ├── Data/ # DbContext
│ └── Helpers/ # Password hashing, screening logic
└── TradeFinanceLite.Tests/ # xUnit test project

## Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server (LocalDB is fine)

### Setup

```bash
git clone https://github.com/<your-username>/TradeFinanceLite.git
cd TradeFinanceLite/TradeFinanceLite.Api
dotnet restore
dotnet ef database update
dotnet run
```

Open `https://localhost:<port>/swagger` to explore the API.

### Running Tests

```bash
cd TradeFinanceLite.Tests
dotnet test
```

## Sample Workflow

1. Register a Maker and a Checker via `/api/Auth/register`
2. Log in as the Maker, create an LC via `POST /api/Lc`
3. Log in as the Checker, approve/reject via `POST /api/Lc/{id}/approve`

## Author

**Danish Hassan**
[LinkedIn](https://www.linkedin.com/in/danish-hassan-94a6a8129/)