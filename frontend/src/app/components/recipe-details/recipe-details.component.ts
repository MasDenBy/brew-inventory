import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { RecipeService } from '../../services/recipe.service';
import { RecipeDetailsResponse } from '../../models/recipe.model';

@Component({
  selector: 'app-recipe-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './recipe-details.component.html',
  styleUrls: ['./recipe-details.component.css']
})
export class RecipeDetailsComponent implements OnInit {
  recipe: RecipeDetailsResponse | null = null;
  isLoading = false;
  error: string | null = null;
  isSyncing = false;
  syncSuccess: string | null = null;
  syncError: string | null = null;

  constructor(
    private recipeService: RecipeService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadRecipe(+id);
    }
  }

  loadRecipe(id: number): void {
    this.isLoading = true;
    this.error = null;
    this.recipeService.getRecipe(id).subscribe({
      next: (data) => {
        this.recipe = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load recipe details';
        this.isLoading = false;
        console.error('Error loading recipe:', err);
      }
    });
  }

  syncToBrewfather(): void {
    if (!this.recipe) return;

    this.isSyncing = true;
    this.syncSuccess = null;
    this.syncError = null;

    this.recipeService.syncRecipe(this.recipe.id).subscribe({
      next: (data) => {
        this.isSyncing = false;
        this.syncSuccess = 'Recipe created in Brewfather.';
        this.recipe!.brewfatherId = data.brewfatherId;
      },
      error: (err) => {
        this.isSyncing = false;
        this.syncError = err.error?.message || 'Failed to create recipe in Brewfather.';
        console.error('Error syncing recipe to Brewfather:', err);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/recipes']);
  }
}
