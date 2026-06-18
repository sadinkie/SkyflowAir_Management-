# SkyflowAir_Management-

Skyflow Air Management is a .NET console application for managing core airline workflows such as users, flights, bookings, and passengers.

## Tech Stack

- .NET (`net10.0`)
- C#
- SQL Server access via `Microsoft.Data.SqlClient`

## Project Structure

- `Program.cs` — Application entry point
- `Core/Entities` — Domain entities (`Flight`, `Booking`, `Passenger`, `User`, etc.)
- `Core/Interfaces` — Repository contracts
- `Data` — SQL repository implementation
- `Config` — Connection-string and configuration helpers

## Prerequisites

- .NET SDK compatible with `net10.0`
- Access to a SQL Server instance (if database-backed features are used)

## Run Locally

1. Clone the repository:

	```bash
	git clone https://github.com/sadinkie/SkyflowAir_Management-.git
	cd SkyflowAir_Management-
	```

2. Restore dependencies:

	```bash
	dotnet restore
	```

3. Build and run:

	```bash
	dotnet run
	```

## Notes

- Ensure connection settings are configured appropriately before running features that require database access.
