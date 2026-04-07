# 🌸 Selfcare Meets Beautify – Admin Panel








# 📌 Overview

Selfcare Meets Beautify – Admin Panel is a backend management system built using ASP.NET Core MVC.

This application is designed only for administrators to manage beauty and self-care service content, ensuring full control over data, services, and system operations.

⚠️ This project does not include a public user interface. It is strictly an admin control system.

# ✨ Features
# 🔐 Admin Authentication
Secure login system
Role-based authorization (Admin only access)
Protected routes for all operations
# 🛠️ Service Management
Add new services
Edit existing services
Delete services
View all services in a structured dashboard
# 🗂️ Content Control
Manage website content dynamically
Maintain structured data for frontend consumption
⚙️ CRUD Operations
Full Create, Read, Update, Delete functionality
Clean and maintainable controller logic
# 🏗️ Architecture

The project follows the MVC (Model-View-Controller) pattern:

Model → Data structure and business logic
View → Admin dashboard UI
Controller → Handles admin requests and operations

✔ Clean separation of concerns
✔ Scalable backend design
✔ Maintainable codebase

🛠️ Technologies Used
ASP.NET Core MVC
C#
Entity Framework Core (Code First)
SQL Server
HTML5
CSS3
Bootstrap
# 📂 Project Structure
/Controllers
    ├── AdminController.cs
    ├── ServiceController.cs
    └── AuthController.cs

/Models
    ├── Service.cs
    └── AdminUser.cs

/Views
    ├── Admin/
    ├── Services/
    └── Shared/

/wwwroot
    ├── css/
    ├── js/
    └── images/
# 🚀 Getting Started
# 🔧 Prerequisites
Visual Studio 2022+
.NET SDK
SQL Server
⚙️ Setup Steps
Clone the repository:
git clone https://github.com/mohshenaa/Selfcare-meets-Beautify_ASP.NET_CORE_MVC.git
Open the project in Visual Studio
Configure appsettings.json with your SQL Server connection string
Run migrations:
Update-Database
Run the project:
Ctrl + F5
# 📸 Screenshots

Admin Dashboard
Service Management Page
Login Page
🎯 Purpose

This project demonstrates:

Building a secure Admin Panel in ASP.NET Core MVC
Implementing authentication & authorization
Managing application data through CRUD operations
Designing a structured backend system

# 👩‍💻 Author

Mohshena Akter Meem

# GitHub: https://github.com/mohshenaa
⭐ Support

If you like this project:

⭐ Star this repo
🍴 Fork it
