import { Routes } from '@angular/router';
import { IngredientFormComponent } from './components/ingredient-form/ingredient-form.component';
import { InventoryTableComponent } from './components/inventory-table/inventory-table.component';
import { FermentableListComponent } from './components/fermentable-list/fermentable-list.component';
import { FermentableFormComponent } from './components/fermentable-form/fermentable-form.component';

export const routes: Routes = [
  { path: '', component: InventoryTableComponent },
  { path: 'add', component: IngredientFormComponent },
  { path: 'edit/:id', component: IngredientFormComponent },
  { path: 'fermentables', component: FermentableListComponent },
  { path: 'fermentables/add', component: FermentableFormComponent },
  { path: 'fermentables/edit/:id', component: FermentableFormComponent }
];
