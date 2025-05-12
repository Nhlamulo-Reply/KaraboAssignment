Agricultural Management System Documentation
Overview
A comprehensive system for managing farmers and their agricultural products with role-based access control.
Features

User Roles
• Farmers: Add and manage their products
• Employees: Manage farmer profiles and search/filters farmers products
• Admin: Full system access (pre-configured)
Product Management
• Add products with: Name, Category, Production Date
• View and filter product listings

User Management
• Role-based authentication
• Pre-seeded admin account for immediate access

System Requirements
• .NET 6 SDK
• SQL Server 2019+ (or SQL Server Express)
• Visual Studio 2022+ or Visual Studio Code

Setup Instructions
1. Database Configuration
1. Create Database:
CREATE DATABASE AgriEnergyConnectDb;
2. Update Connection String in appsettings.json:
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AgriEnergyConnectDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }

2. Project Setup

Restore Packages:
dotnet restore
Apply Database Migrations:
dotnet ef database update
The system will automatically:
• Create roles (Admin, Employee, Farmer)
• Create default admin user:
Email: admin@example.com
Password: Admin@123
Role: Employee

3. Running the Application
dotnet run or ctr + f5

Default Accounts
Role	Email	Password
Employee	admin@example.com	Admin@123
Farmer	Register manually	Custom

Role Configuration
• Admin: Full system access, manage all users and roles
• Employee: Add/remove farmers, view and filter all products
• Farmer: Add and manage own products, access personal dashboard
Troubleshooting
Migration Issues
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
run dotnet build(To get more advance troubleshooting)

Connection Problems
• Ensure SQL Server is running
• Check firewall settings
• Verify the connection string matches your SQL Server instance
Development Notes
• Use the seeded admin account for testing
• Register farmer accounts for role-specific testing
• Configuration:
  - Server settings: appsettings.json (Check DefaultConnection string if it exist ) if not manually add the following.
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AgriEnergyConnectDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}

To check if (localdb)\MSSQLLocalDB is installed and running on your machine (Windows), follow these steps:

 Step 1: Check if LocalDB is Installed
Via Command Prompt or PowerShell or package manager

sqllocaldb info
This will list all installed LocalDB instances. You should see something like:

MSSQLLocalDB
Step 2: Check if MSSQLLocalDB is Running
Run:

sqllocaldb i MSSQLLocalDB
This shows the status and instance details. Look for:

State: Running
If it's not running, you can start it with:

sqllocaldb start MSSQLLocalDB
 Step 3: Connect to (localdb)\MSSQLLocalDB
You can use SQL Server Management Studio (SSMS) or Azure Data Studio:

Server name: (localdb)\MSSQLLocalDB

Authentication: Windows Authentication

Then click Connect.
 
Confirm if the Database is connected and check if default users are added and roles in the database

