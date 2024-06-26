# Store Stock Management System

This is a web application built for managing product stock in stores. It allows users to perform various operations such as adding, updating, and deleting products, managing stock quantities, and viewing product details.

## Features

- **Store Management**: Create, update, and delete stores.
- **Product Management**: Add, update, and delete products.
- **Stock Management**: Manage stock quantities for products in each store.
- **Web Interface**: User-friendly web interface for easy interaction with the system.

## Dependencies

- **ASP.NET Core**: The application is built using ASP.NET Core, a cross-platform, high-performance framework for building modern, cloud-based, Internet-connected applications.
- **Entity Framework Core**: Entity Framework Core is used as the ORM (Object-Relational Mapping) framework to interact with the database.
- **C#**: The primary programming language used for backend development.
- **SQL Server**: SQL Server is used as the database management system for storing application data.
- **dotnet-ef**: The Entity Framework Core Command Line Tools are used for database migrations and schema management.

## Installation

1. Clone the repository: `git clone https://github.com/Saladin-99/StoreStock.git`
2. Navigate to the project directory: `cd StoreStock`
3. Install SQL Server: Follow the installation instructions for SQL Server based on your operating system.
4. Install dotnet-ef: Run `dotnet tool install --global dotnet-ef` to install the Entity Framework Core Command Line Tools globally.
5. Install dependencies: `dotnet restore`
6. Run the application: `dotnet run`

