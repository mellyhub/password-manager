# Password Manager

This is a simple console application for managing passwords. It allows users to securely add, view, and save account information locally in an encrypted text file.

## Features
- AES encryption for secure password storage.
- Add, view, and save account information.
- Configuration via `appsettings.json`.

## Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later).
- NuGet package: `Microsoft.Extensions.Configuration.Json` (automatically restored during build).

## How to Run the Application
1. Clone the repository or download the project files.
2. Open a terminal and navigate to the project directory.
3. Configure the `appsettings.json` file:
   - Set the `Encryption:Key` (16, 24, or 32 characters).
   - Set the `Encryption:IV` (16 characters).
4. Build and run the application:
   ```bash
   dotnet build
   dotnet run
   ```