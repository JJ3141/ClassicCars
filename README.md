# 🚀 Project Name

A web application for managing and showcasing cars, allowing users to add, view, and manage car listings.
![.NET Version](https://img.shields.io/badge/.NET-8.0-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 📋 Table of Contents

- [About the Project](#about-the-project)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Features](#features)
- [Usage](#usage)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

## 📖 About the Project

ClassicCars is an ASP.NET Core MVC web application that allows users to create, view, and manage car listings.
It demonstrates core concepts such as MVC architecture, Entity Framework Core, file uploads, and input validation.
Built for car enthusiasts or anyone who wants to manage a car collection online.

---

## 🛠️ Technologies Used

| Technology            | Version  | Purpose                          |
|-----------------------|----------|----------------------------------|
| ASP.NET Core MVC      | 8.0      | Web framework                    |
| Entity Framework Core | 8.0      | ORM / Database access            |
| SQL Server / SQLite   | -        | Database                         |
| Bootstrap             | 5.3      | Frontend styling                 |
| Razor Pages / Views   | -        | Server-side HTML rendering       |
| IFormFile             | 8.0      | Handling file uploads            |

---

## ✅ Prerequisites

Make sure you have the following installed before running the project:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or SQLite (if used)
- [Git](https://git-scm.com/)

---

## 🚀 Getting Started

Follow these steps to get the project running locally.

### 1. Clone the repository

git clone https://github.com/JJ3141/ClassicCars.git
cd ClassicCars

### 2. Restore dependencies

dotnet restore


### 3. Apply database migrations

```bash
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

The app will be available at `https://localhost:5001` or `http://localhost:5000`.

---

## 📁 Project Structure

```
ClassicCars/
│
├── Controllers/          # MVC Controllers
├── Models/               # Domain models and ViewModels
├── Views/                # Razor Views (.cshtml)
├── Data/                 # DbContext and migrations
├── Services/             # Business logic / service layer
├── wwwroot/              # Static files (CSS, JS, images)
├── appsettings.json      # App configuration
└── Program.cs            # App entry point and middleware setup
```

---

## ✨ Features

 User registration and login (ASP.NET Identity)

 CRUD operations for cars

 Upload car images using IFormFile

 Input validation (server-side & client-side)

 Responsive UI with Bootstrap

 Search and filter car listings

---

## 💻 Usage

Navigate to /Register to create an account.

Log in at /Login.

Use the dashboard to add new cars, upload images, and manage listings.

Browse all cars at the home page.

> 💡 **Tip:** You can add screenshots using: `![Screenshot](docs/screenshot.png)`

---

## 🗄️ Database Setup

The project uses **Entity Framework Core** with a Code-First approach.

Connection string is configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ClassicCarsDb;Trusted_Connection=True;"
}
```

To create and seed the database:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## ⚙️ Configuration

Key settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ClassicCarsDb;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

> ⚠️ **Never commit sensitive data** (passwords, API keys) to source control. Use `appsettings.Development.json` or environment variables for local secrets.

---

## 🤝 Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a new branch: `git checkout -b feature/your-feature-name`
3. Commit your changes: `git commit -m "Add some feature"`
4. Push to the branch: `git push origin feature/your-feature-name`
5. Open a Pull Request

---

## 📄 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

## 📬 Contact

**Your Name** – (https://github.com/JJ3141)

Project Link: https://github.com/JJ3141/ClassicCars
---

*Built as part of the **ASP.NET Fundamentals** course.*
