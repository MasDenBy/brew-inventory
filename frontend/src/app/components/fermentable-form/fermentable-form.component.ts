import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FermentableService } from '../../services/fermentable.service';
import { Fermentable, FermentableType } from '../../models/fermentable.model';

@Component({
  selector: 'app-fermentable-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './fermentable-form.component.html',
  styleUrls: ['./fermentable-form.component.css']
})
export class FermentableFormComponent implements OnInit {
  fermentableForm!: FormGroup;
  isEditMode = false;
  fermentableId?: number;
  isSubmitting = false;
  error: string | null = null;
  fermentableTypes = Object.values(FermentableType);

  constructor(
    private fb: FormBuilder,
    private fermentableService: FermentableService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.initForm();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.fermentableId = +params['id'];
        this.loadFermentable(this.fermentableId);
      }
    });
  }

  initForm(): void {
    this.fermentableForm = this.fb.group({
      name: ['', Validators.required],
      supplier: [''],
      origin: [''],
      type: [FermentableType.Grain, Validators.required],
      color: [0, [Validators.required, Validators.min(0)]],
      amount: [0, [Validators.required, Validators.min(0)]],
      bestBefore: [''],
      brewfatherId: ['']
    });
  }

  loadFermentable(id: number): void {
    this.fermentableService.getFermentable(id).subscribe({
      next: (fermentable) => {
        this.fermentableForm.patchValue({
          name: fermentable.name,
          supplier: fermentable.supplier || '',
          origin: fermentable.origin || '',
          type: fermentable.type,
          color: fermentable.color,
          amount: fermentable.amount,
          bestBefore: fermentable.bestBefore || '',
          brewfatherId: fermentable.brewfatherId || ''
        });
      },
      error: (err) => {
        this.error = 'Failed to load fermentable';
        console.error('Error loading fermentable:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.fermentableForm.valid) {
      this.isSubmitting = true;
      this.error = null;

      const formValue = this.fermentableForm.value;
      const fermentable: Fermentable = {
        id: this.fermentableId || 0,
        name: formValue.name,
        supplier: formValue.supplier || null,
        origin: formValue.origin || null,
        type: formValue.type,
        color: formValue.color,
        amount: formValue.amount,
        bestBefore: formValue.bestBefore || null,
        brewfatherId: formValue.brewfatherId || null
      };

      const request = this.isEditMode
        ? this.fermentableService.updateFermentable(this.fermentableId!, fermentable)
        : this.fermentableService.addFermentable(fermentable);

      request.subscribe({
        next: () => {
          this.router.navigate(['/fermentables']);
        },
        error: (err) => {
          this.error = this.isEditMode 
            ? 'Failed to update fermentable' 
            : 'Failed to add fermentable';
          this.isSubmitting = false;
          console.error('Error saving fermentable:', err);
        }
      });
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.fermentableForm.controls).forEach(key => {
        this.fermentableForm.get(key)?.markAsTouched();
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/fermentables']);
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.fermentableForm.get(fieldName);
    return !!(field && field.invalid && field.touched);
  }
}
