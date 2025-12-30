import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MiscService } from '../../services/misc.service';
import { Misc, MiscType, InventoryUnit } from '../../models/misc.model';

@Component({
  selector: 'app-misc-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './misc-form.component.html',
  styleUrls: ['./misc-form.component.css']
})
export class MiscFormComponent implements OnInit {
  miscForm!: FormGroup;
  isEditMode = false;
  miscId?: number;
  isSubmitting = false;
  error: string | null = null;
  miscTypes: { value: MiscType; label: string }[] = [];
  inventoryUnits: { value: InventoryUnit; label: string }[] = [];

  constructor(
    private fb: FormBuilder,
    private miscService: MiscService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.initEnums();
    this.initForm();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.miscId = +params['id'];
        this.loadMisc(this.miscId);
      }
    });
  }

  initEnums(): void {
    this.miscTypes = [
      { value: MiscType.Spice, label: 'Spice' },
      { value: MiscType.Herb, label: 'Herb' },
      { value: MiscType.Fruit, label: 'Fruit' },
      { value: MiscType.Flavor, label: 'Flavor' },
      { value: MiscType.WaterAgent, label: 'Water Agent' },
      { value: MiscType.Other, label: 'Other' }
    ];

    this.inventoryUnits = [
      { value: InventoryUnit.Grams, label: 'Grams (g)' },
      { value: InventoryUnit.Kilograms, label: 'Kilograms (kg)' },
      { value: InventoryUnit.Liters, label: 'Liters (L)' },
      { value: InventoryUnit.Milliliters, label: 'Milliliters (ml)' },
      { value: InventoryUnit.Packages, label: 'Packages' },
      { value: InventoryUnit.Tablets, label: 'Tablets' }
    ];
  }

  initForm(): void {
    this.miscForm = this.fb.group({
      name: ['', Validators.required],
      type: [MiscType.Spice, Validators.required],
      unit: [InventoryUnit.Grams, Validators.required],
      amount: [0, [Validators.required, Validators.min(0)]],
      bestBefore: [''],
      brewfatherId: ['']
    });
  }

  loadMisc(id: number): void {
    this.miscService.getMisc(id).subscribe({
      next: (misc) => {
        this.miscForm.patchValue({
          name: misc.name,
          type: misc.type,
          unit: misc.unit,
          amount: misc.amount,
          bestBefore: misc.bestBefore || '',
          brewfatherId: misc.brewfatherId || ''
        });
      },
      error: (err) => {
        this.error = 'Failed to load misc';
        console.error('Error loading misc:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.miscForm.valid) {
      this.isSubmitting = true;
      this.error = null;

      const formValue = this.miscForm.value;
      const misc: Misc = {
        id: this.miscId || 0,
        name: formValue.name,
        type: +formValue.type,
        unit: +formValue.unit,
        amount: formValue.amount,
        bestBefore: formValue.bestBefore || null,
        brewfatherId: formValue.brewfatherId || null
      };

      const request = this.isEditMode
        ? this.miscService.updateMisc(this.miscId!, misc)
        : this.miscService.addMisc(misc);

      request.subscribe({
        next: () => {
          this.router.navigate(['/miscs']);
        },
        error: (err) => {
          this.error = this.isEditMode 
            ? 'Failed to update misc' 
            : 'Failed to add misc';
          this.isSubmitting = false;
          console.error('Error saving misc:', err);
        }
      });
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.miscForm.controls).forEach(key => {
        this.miscForm.get(key)?.markAsTouched();
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/miscs']);
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.miscForm.get(fieldName);
    return !!(field && field.invalid && field.touched);
  }
}
