import { Component, OnInit, OnDestroy } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InventoryService } from '../../services/inventory.service';
import { Ingredient } from '../../models/ingredient.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-ingredient-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './ingredient-form.component.html',
  styleUrls: ['./ingredient-form.component.css']
})
export class IngredientFormComponent implements OnInit, OnDestroy {
  ingredientForm: FormGroup;
  private editSub: Subscription | null = null;
  // When not null, the form is editing an existing ingredient
  editingIngredient: Ingredient | null = null;

  constructor(private fb: FormBuilder, private inventoryService: InventoryService) {
    this.ingredientForm = this.fb.group({
      id: [null],
      name: ['', Validators.required],
      supplier: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(0)]],
      expirationDate: ['', Validators.required],
      type: ['', Validators.required]
    });
  }


  get isEditMode(): boolean {
    return this.editingIngredient !== null;
  }

  onSubmit(): void {
    if (this.ingredientForm.valid) {
      const formValue = this.ingredientForm.value as Ingredient;
      if (this.editingIngredient && this.editingIngredient.id != null && this.editingIngredient.id !== 0) {
        // Update existing ingredient
        this.inventoryService.updateIngredient(this.editingIngredient.id, formValue).subscribe(() => {
          this.inventoryService.clearEdit();
          this.editingIngredient = null;
          this.ingredientForm.reset();
        });
      } else {
        // Add new ingredient
        this.inventoryService.addIngredient(formValue).subscribe(() => {
          this.ingredientForm.reset();
        });
      }
    }
  }

  onCancel(): void {
    this.inventoryService.clearEdit();
    this.editingIngredient = null;
    this.ingredientForm.reset();
  }


  ngOnDestroy(): void {
    if (this.editSub) {
      this.editSub.unsubscribe();
    }
  }

  ngOnInit(): void {
    // Subscribe to edit events from the service
    this.editSub = this.inventoryService.editIngredient$.subscribe((ingredient) => {
      if (ingredient) {
        this.editingIngredient = ingredient;
        // Patch form values (ensure date is formatted for the input)
        const patch = {
          id: ingredient.id,
          name: ingredient.name,
          supplier: ingredient.supplier,
          amount: ingredient.amount,
          expirationDate: ingredient.expirationDate ? new Date(ingredient.expirationDate).toISOString().slice(0,10) : '',
          type: ingredient.type
        };
        this.ingredientForm.patchValue(patch);
      } else {
        // Clear editing state
        this.editingIngredient = null;
        this.ingredientForm.reset();
      }
    });
  }
}