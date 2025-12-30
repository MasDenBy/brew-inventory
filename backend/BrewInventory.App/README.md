# Brew Inventory Application

## Overview
The Brew Inventory Application is a home brewing inventory management system that allows users to track various brewing ingredients such as malts, hops, yeasts, and other miscellaneous ingredients. The application is built using ASP.NET Core for the backend and Angular for the frontend, with persistent storage provided by SQLite and Entity Framework.

## Features
- **Ingredient Management**: Add, modify, and delete ingredients from the inventory.
- **Recipe Management**: Manage recipes that utilize the ingredients in the inventory.
- **User-Friendly Interface**: An intuitive Angular frontend that displays inventory items in a table format.
- **Persistent Storage**: Utilizes SQLite for data storage, ensuring that all inventory data is saved and retrievable.

## Project Structure
```
brew-inventory-app
├── backend
│   ├── BrewInventory.App
│   │   ├── Program.cs
│   │   ├── BrewInventory.App.csproj
│   │   ├── Controllers
│   │   │   ├── IngredientsController.cs
│   │   │   └── RecipesController.cs
│   │   ├── Models
│   │   │   ├── Ingredient.cs
│   │   │   ├── Malt.cs
│   │   │   ├── Hop.cs
│   │   │   ├── Yeast.cs
│   │   │   └── OtherIngredient.cs
│   │   ├── Data
│   │   │   ├── BrewInventoryContext.cs
│   │   │   └── Migrations
│   │   ├── appsettings.json
│   │   └── README.md
│   └── BrewInventory.sln
├── frontend
│   ├── brew-inventory-ui
│   │   ├── src
│   │   │   ├── app
│   │   │   │   ├── components
│   │   │   │   │   ├── inventory-table
│   │   │   │   │   │   ├── inventory-table.component.ts
│   │   │   │   │   │   ├── inventory-table.component.html
│   │   │   │   │   │   └── inventory-table.component.css
│   │   │   │   │   ├── ingredient-form
│   │   │   │   │   │   ├── ingredient-form.component.ts
│   │   │   │   │   │   ├── ingredient-form.component.html
│   │   │   │   │   │   └── ingredient-form.component.css
│   │   │   │   ├── services
│   │   │   │   │   └── inventory.service.ts
│   │   │   │   ├── app.module.ts
│   │   │   │   └── app.component.ts
│   │   │   ├── assets
│   │   │   ├── environments
│   │   │   │   ├── environment.ts
│   │   │   │   └── environment.prod.ts
│   │   │   └── index.html
│   │   ├── angular.json
│   │   ├── package.json
│   │   └── README.md
├── README.md
```

## Getting Started
1. **Clone the Repository**: Clone this repository to your local machine.
2. **Backend Setup**:
   - Navigate to the `backend/BrewInventory.App` directory.
   - Restore the dependencies using `dotnet restore`.
   - Run the application using `dotnet run`.
3. **Frontend Setup**:
   - Navigate to the `frontend/brew-inventory-ui` directory.
   - Install the Angular dependencies using `npm install`.
   - Start the Angular application using `ng serve`.

## Technologies Used
- **Backend**: ASP.NET Core, Entity Framework, SQLite
- **Frontend**: Angular, TypeScript, HTML, CSS

## Contributing
Contributions are welcome! Please feel free to submit a pull request or open an issue for any enhancements or bug fixes.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.