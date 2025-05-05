# Drinks Application

A comprehensive Windows application for managing and discovering drinks, with features for tracking favorites, voting for drinks of the day, and searching based on various criteria.

## Tech Stack

- **Frontend**: Windows UI Library (WinUI 3)
- **Backend**: ASP.NET Core Web API
- **Database**: SQL Server with Entity Framework Core
- **Testing**: NUnit and xUnit

## Project Structure

The solution consists of four main projects:

1. **WinUIApp** - The client-side Windows application
2. **WinUIApp.Data** - Contains data models and database context
3. **WinUIApp.WebAPI** - Backend API for data operations
4. **WinUIApp.Tests** - Testing suite for the application

## Database Setup

### Initial Setup

1. Ensure you have SQL Server installed on your machine
2. Navigate to the WinUIApp.Data directory:
   ```
   cd WinUIApp.Data
   ```
3. Reset the database if needed:
   ```
   dotnet ef database drop
   ```
4. Apply migrations to create the database:
   ```
   dotnet ef database update
   ```

The default connection string is configured to work with localhost SQL Server.

### Database Schema

The database consists of the following tables:

- **Brands** - Information about drink brands
- **Categories** - Drink categories (e.g., Beer, Wine, Spirits)
- **Drinks** - Core drink information (name, alcohol content, etc.)
- **DrinkCategories** - Junction table for many-to-many relationship between drinks and categories
- **Users** - User information
- **Votes** - User votes for drinks of the day
- **DrinkOfTheDay** - Selected drink of the day
- **UserDrinks** - Junction table for user's personal drink lists
- **Reviews** - User reviews for drinks
- **Ratings** - User ratings for drinks

## Running the Application

### First Run

1. Clone the repository
2. Configure the database as described above
3. Set the startup projects to both `WinUIApp` and `WinUIApp.WebAPI`
4. Run the solution

When running for the first time, the application will automatically create and seed the database with initial data.

### API Configuration

The WinUIApp client connects to the API using settings in `appsettings.json`. Ensure the API URL is correctly set:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7001"
  }
}
```

## Features

### User Interface

The application features a modern Windows UI with several key screens:

- **Main Page** - Dashboard with drink of the day and recommended drinks
- **Search Page** - Advanced search functionality with multiple filters
- **Drink Detail Page** - Detailed information about a specific drink
- **Personal Drink List** - User's saved favorite drinks

### Search Functionality

The application allows searching for drinks based on:

- Name
- Brand
- Category
- Alcohol content range
- Rating

### User Features

- Add/remove drinks to personal list
- Vote for drink of the day
- Add new drinks to the database
- Update existing drink information
- Rate and review drinks

## API Endpoints

The WebAPI exposes the following endpoints:

- `GET /drink/get-all` - Retrieve drinks with optional filters
- `GET /drink/get-one` - Get drink by ID
- `GET /drink/get-drink-brands` - List all drink brands
- `GET /drink/get-drink-categories` - List all drink categories
- `GET /drink/get-drink-of-the-day` - Get the current drink of the day
- `POST /drink/get-user-drink-list` - Get a user's personal drink list
- `POST /drink/add` - Add a new drink
- `POST /drink/add-to-user-drink-list` - Add a drink to user's personal list
- `POST /drink/vote-drink-of-the-day` - Vote for a drink of the day
- `PUT /drink/update` - Update a drink's information
- `DELETE /drink/delete` - Delete a drink
- `DELETE /drink/delete-from-user-drink-list` - Remove a drink from user's personal list

## Testing

The application includes unit tests for critical components:

- Database operations
- Service layer functionality
- View model logic

To run tests:

1. Open Test Explorer in Visual Studio
2. Click "Run All Tests" or select specific tests to run

## Development

### Adding New Features

1. Define data models in `WinUIApp.Data/Data`
2. Add appropriate configuration in `WinUIApp.Data/Configurations`
3. Create repository and service in the WebAPI project
4. Implement controller endpoints
5. Create corresponding view models and views in the WinUI project

### Code Style

This project uses StyleCop for enforcing code style. The rules are defined in `SE.ruleset`.
