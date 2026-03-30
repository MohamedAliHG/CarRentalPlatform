# 🚗 CarRentalPlatform — Car Rental Platform (Tunisia)

A full-featured **ASP.NET Core MVC** web application for managing car rental agencies in Tunisia. The platform connects rental agencies with clients, enabling car listings, rental offers, booking requests, and user reviews — all under a secure, role-based system.

---

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Database Setup](#database-setup)
- [Usage](#usage)
- [Contributing](#contributing)

---

## Overview

**CarRentalPlatform** is a web-based car rental management system designed for the Tunisian market. It allows:

- **Agencies** to register, manage their fleet, and publish rental offers.
- **Clients** to browse available cars, submit rental requests, and leave reviews.
- **Admins** to oversee the platform, manage users, and handle operations.

---

## Features

- 🔐 **Authentication & Authorization** — Role-based access control (Admin, Agency, Client) via ASP.NET Core Identity
- 🚘 **Vehicle Management** — Add, edit, and manage cars per agency
- 📋 **Rental Offers** — Agencies can publish and manage rental offers
- 📩 **Rental Requests** — Clients can submit and track booking requests 
- ⭐ **Reviews System** — Clients can leave reviews on agencies or cars
- 📧 **Email Notifications** — Integrated email service for alerts and confirmations
- 📁 **File Upload** — Support for uploading car images and documents
- 🔒 **Security** — Account lockout, strong password policy, HTTPS enforcement, HSTS
- 🗂️ **Session Management** — Server-side session with 30-minute idle timeout

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 9) |
| Language | C# |
| ORM | Entity Framework Core 9 |
| Database | Microsoft SQL Server |
| Auth | ASP.NET Core Identity |
| Views | Razor Pages + MVC Views |
| Frontend | HTML, CSS, JavaScript |
| Logging | Console + Debug providers |

---

## Architecture

The project follows a clean **layered architecture** with clear separation of concerns:

```
Presentation   →   Controllers / Views / ViewModels
Business Logic →   Services (with interfaces / contracts)
Data Access    →   Repositories + Unit of Work pattern
Database       →   Entity Framework Core + SQL Server
```

Key design patterns used:
- **Repository Pattern** — abstracts data access per entity
- **Unit of Work** — coordinates multiple repository operations in a single transaction
- **Service Layer** — business logic decoupled from controllers
- **Dependency Injection** — all services and repositories registered via DI container

---

## Project Structure

```
AgenceLocationVoiture/
│
├── Areas/
│   └── Identity/Pages/         # Scaffolded Identity UI (login, register, etc.)
│
├── Controllers/                # MVC Controllers
├── Data/                       # DbContext (ApplicationDbContext)
├── Helpers/                    # Utility/helper classes
├── Models/                     # Domain models (Voiture, Agence, Client, Avis, etc.)
├── Repositories/               # Repository interfaces and implementations + UnitOfWork
├── Services/
│   ├── ServiceContracts/       # Service interfaces
│   └── Services/               # Service implementations
│       # (AgenceService, ClientService, VoitureService,
│       #  OffreLocService, DemandeLocService, AvisService,
│       #  AuthenticationService, EmailService, FileUploadService)
│
├── ViewModels/                 # ViewModels for Razor views
├── Views/                      # Razor view templates
├── wwwroot/                    # Static assets (CSS, JS, images)
│
├── Program.cs                  # App entry point & service registration
└── AgenceLocationVoiture.csproj
```

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (LocalDB, Express, or full)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/MohamedAliHG/CarRentalPlatform.git
   cd CarRentalPlatform
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Configure your connection string** (see [Configuration](#configuration))

4. **Apply migrations:**
   ```bash
   dotnet ef database update
   ```

5. **Run the application:**
   ```bash
   dotnet run
   ```

6. Open your browser at `https://localhost:5001`

---

## Configuration

Edit `appsettings.json` to set your database connection string and email settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AgenceLocationVoiture;Trusted_Connection=True;"
  },
  "EmailSettings": {
    "SmtpHost": "smtp.example.com",
    "SmtpPort": 587,
    "SenderEmail": "noreply@example.com",
    "SenderName": "AgenceLocationVoiture"
  }
}
```

> **Note:** Never commit secrets to source control. Use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) in development:
> ```bash
> dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your_connection_string"
> ```

---

## Database Setup

The project uses **Entity Framework Core** with SQL Server. Run the following commands to set up the database:

```bash
# Create a new migration (if needed)
dotnet ef migrations add InitialCreate

# Apply migrations to the database
dotnet ef database update
```

---

## Usage

### Roles

| Role | Capabilities |
|---|---|
| **Admin** | Manage all users, agencies, and platform data |
| **Agence** | Manage vehicles, publish rental offers, respond to requests |
| **Client** | Browse cars, submit rental requests, leave reviews |

### Password Policy

All accounts must have a password that:
- Is at least **8 characters** long
- Contains **uppercase** and **lowercase** letters
- Contains at least one **digit**
- Contains at least one **non-alphanumeric** character

Accounts are **locked for 15 minutes** after 5 failed login attempts.

---

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Commit your changes: `git commit -m "Add your feature"`
4. Push to the branch: `git push origin feature/your-feature-name`
5. Open a Pull Request

---

## Author

**Mohamed Ali HG** — [@MohamedAliHG](https://github.com/MohamedAliHG)

---

*Built with ❤️ using ASP.NET Core 9 and Entity Framework Core.*
