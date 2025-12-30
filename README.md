# Brew Inventory Application

## Overview
The Brew Inventory Application is a home brewing inventory management system designed to help users track various brewing ingredients such as malts, hops, yeasts, and other components. This application features a backend built with ASP.NET Core and a frontend developed using Angular, providing a seamless user experience for managing brewing supplies.

## Features
- **Ingredient Management**: Add, modify, and delete ingredients from the inventory.
- **Persistent Storage**: Utilizes SQLite with Entity Framework for data storage.
- **User-Friendly Interface**: Angular frontend with a responsive design for easy navigation and management of inventory items.

## Project Structure
The project is organized into two main parts: the backend and the frontend.

### Backend
- **BrewInventory.App**: The ASP.NET Core application that handles API requests and manages the database.
  - **Controllers**: Contains the logic for handling HTTP requests.
    - `IngredientsController.cs`: Manages ingredient-related requests.
    - `RecipesController.cs`: Manages recipe-related requests.
  - **Models**: Defines the data structures used in the application.
    - `Ingredient.cs`: Base class for all ingredients.
    - `Malt.cs`, `Hop.cs`, `Yeast.cs`, `OtherIngredient.cs`: Specific ingredient types.
  - **Data**: Contains the database context and migrations.
    - `BrewInventoryContext.cs`: Manages database connections and entity sets.
  - **appsettings.json**: Configuration settings for the application.

### Frontend
- **brew-inventory-ui**: The Angular application that provides the user interface.
  - **Components**: Contains reusable components for the application.
    - `inventory-table`: Displays the list of inventory items.
    - `ingredient-form`: Provides a form for adding and modifying ingredients.
  - **Services**: Handles HTTP requests to the backend API.
    - `inventory.service.ts`: Manages inventory-related API calls.
  - **Assets**: Contains static assets for the application.
  - **Environments**: Configuration files for different environments (development and production).

## Getting Started
1. **Clone the Repository**: 
   ```
   git clone <repository-url>
   cd brew-inventory-app
   ```

2. **Set Up the Backend**:
   - Navigate to the `backend/BrewInventory.App` directory.
   - Restore the NuGet packages:
     ```
     dotnet restore
     ```
   - Run the migrations to set up the database:
     ```
     dotnet ef database update
     ```
   - Start the backend application:
     ```
     dotnet run
     ```

3. **Set Up the Frontend**:
   - Navigate to the `frontend/brew-inventory-ui` directory.
   - Install the dependencies:
     ```
     npm install
     ```
   - Start the Angular application:
     ```
     ng serve
     ```

4. **Access the Application**: Open your browser and navigate to `http://localhost:4200` to access the frontend.

## Contributing
Contributions are welcome! Please submit a pull request or open an issue for any enhancements or bug fixes.

## License
This project is licensed under the MIT License. See the LICENSE file for details.