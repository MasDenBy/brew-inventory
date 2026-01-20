import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RecipeService } from '../../services/recipe.service';
import { IngredientPurchaseService } from '../../services/ingredient-purchase.service';
import { RecipeListResponse } from '../../models/recipe.model';
import { IngredientPurchaseResponse, IngredientNeedDetail } from '../../models/ingredient-purchase.model';

@Component({
  selector: 'app-ingredient-purchase',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ingredient-purchase.component.html',
  styleUrls: ['./ingredient-purchase.component.css']
})
export class IngredientPurchaseComponent implements OnInit {
  recipes: RecipeListResponse[] = [];
  selectedRecipeIds: number[] = [];
  purchaseData: IngredientPurchaseResponse | null = null;
  isLoadingRecipes = false;
  isCalculating = false;
  error: string | null = null;
  isExporting = false;

  constructor(
    private recipeService: RecipeService,
    private ingredientPurchaseService: IngredientPurchaseService
  ) { }

  ngOnInit(): void {
    this.loadRecipes();
  }

  exportPurchaseNeeds(): void {
    if (!this.purchaseData || this.selectedRecipeIds.length === 0) {
      return;
    }
    this.isExporting = true;
    this.ingredientPurchaseService.exportPurchaseNeeds(this.selectedRecipeIds).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'ingredient-purchase.xlsx';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
        this.isExporting = false;
      },
      error: (err) => {
        this.error = 'Failed to export Excel file';
        this.isExporting = false;
        console.error('Error exporting Excel:', err);
      }
    });
  }

  loadRecipes(): void {
    this.isLoadingRecipes = true;
    this.error = null;
    this.recipeService.getRecipes().subscribe({
      next: (data) => {
        this.recipes = data;
        this.isLoadingRecipes = false;
      },
      error: (err) => {
        this.error = 'Failed to load recipes';
        this.isLoadingRecipes = false;
        console.error('Error loading recipes:', err);
      }
    });
  }

  toggleRecipeSelection(recipeId: number): void {
    const index = this.selectedRecipeIds.indexOf(recipeId);
    if (index > -1) {
      this.selectedRecipeIds.splice(index, 1);
    } else {
      this.selectedRecipeIds.push(recipeId);
    }
  }

  isRecipeSelected(recipeId: number): boolean {
    return this.selectedRecipeIds.includes(recipeId);
  }

  calculatePurchaseNeeds(): void {
    if (this.selectedRecipeIds.length === 0) {
      this.error = 'Please select at least one recipe';
      return;
    }

    this.isCalculating = true;
    this.error = null;
    this.ingredientPurchaseService.calculatePurchaseNeeds(this.selectedRecipeIds).subscribe({
      next: (data) => {
        this.purchaseData = data;
        this.isCalculating = false;
      },
      error: (err) => {
        this.error = 'Failed to calculate purchase needs';
        this.isCalculating = false;
        console.error('Error calculating purchase needs:', err);
      }
    });
  }

  hasAnyIngredients(): boolean {
    if (!this.purchaseData) return false;
    return this.purchaseData.fermentables.length > 0 ||
           this.purchaseData.hops.length > 0 ||
           this.purchaseData.yeasts.length > 0 ||
           this.purchaseData.miscs.length > 0;
  }

  resetCalculation(): void {
    this.purchaseData = null;
    this.selectedRecipeIds = [];
    this.error = null;
  }
}
