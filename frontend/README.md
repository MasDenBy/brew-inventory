# Brew Inventory UI

This is the frontend application for the Brew Inventory project, built using Angular. The application allows users to manage their home brewing inventory, including tracking malt, hops, yeasts, and other ingredients.

## Features

- Add, modify, and delete inventory items.
- Display inventory items in a user-friendly table format.
- Responsive design for optimal viewing on various devices.

## Getting Started

To get started with the Brew Inventory UI, follow these steps:

1. **Clone the repository:**
   ```
   git clone <repository-url>
   cd brew-inventory-app/frontend/brew-inventory-ui
   ```

2. **Install dependencies:**
   ```
   npm install
   ```

3. **Run the application:**
   ```
   ng serve
   ```

4. **Open your browser and navigate to:**
   ```
   http://localhost:4200
   ```

## Project Structure

- `src/app/components`: Contains the Angular components for the application.
  - `inventory-table`: Displays the list of inventory items.
  - `ingredient-form`: Provides a form for adding and modifying inventory items.
- `src/app/services`: Contains services for handling HTTP requests to the backend API.
- `src/environments`: Contains environment-specific settings for development and production.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License. See the LICENSE file for details.