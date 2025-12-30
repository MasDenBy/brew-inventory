import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { InventoryService } from '../../services/inventory.service';
import { Ingredient } from '../../models/ingredient.model';
import { IngredientType } from '../../models/ingredient.model';

@Component({
  selector: 'app-inventory-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './inventory-table.component.html',
  styleUrls: ['./inventory-table.component.css']
})
export class InventoryTableComponent implements OnInit {
  ingredients: Ingredient[] = [];
  constructor(private inventoryService: InventoryService, private router: Router) {}

  ngOnInit(): void {
    this.loadIngredients();
  }

  loadIngredients(): void {
    this.inventoryService.getIngredients().subscribe((data: Ingredient[]) => {
      this.ingredients = data;
    });
  }

  deleteIngredient(id: number): void {
    this.inventoryService.deleteIngredient(id).subscribe(() => {
      this.loadIngredients();
    });
  }

  editIngredient(ingredient: Ingredient): void {
    this.inventoryService.startEdit(ingredient);
    this.router.navigate(['/edit', ingredient.id]);
  }

  addNewIngredient(): void {
    this.inventoryService.startEdit({ id: 0, name: '', supplier: '', amount: 0, expirationDate: new Date(), type: IngredientType.Misc });

    this.router.navigate(['/add']);
  }
}