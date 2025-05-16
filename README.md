# Payroll Management System

## Overview
The Payroll Management System is an ASP.NET Core MVC application designed to manage employee information, salary processing, and payroll management. It allows different roles such as Admin, HR, and Employees to manage and access the system's functionalities efficiently.

### Features
- **Employee Management**: Employees can register, view, and update their profiles.
- **Salary Management**: HR can manage and update employee salaries.
- **Role-based Access Control**: Admin, HR, and Employee roles with different levels of access.
- **CRUD Operations**: Create, Read, Update, and Delete employee data using Entity Framework Core.
- **Async Operations**: All database operations are asynchronous for better performance.
- **Admin Dashboard**: Admin has full control to manage the system, users, and payroll.
- **Hangfire Integration**: Allows background processing for tasks like email notifications, payroll generation, etc.

## Tech Stack
- **Backend**: ASP.NET Core MVC
- **Frontend**: DevExtreme UI components
- **Database**: SSMS
- **Authentication**: ASP.NET Core Identity
- **Asynchronous Operations**: Entity Framework Core with async programming
- **Background Processing**: Hangfire 

## Setup Instructions

### Prerequisites
1. **.NET 6 or higher**: [Download .NET](https://dotnet.microsoft.com/download)
2. **SSMS**
3. **Visual Studio 2022 or later** 

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/Munali1/Payroll-Management-System.git
