import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MiscService } from '../../services/misc.service';
import { Misc } from '../../models/misc.model';

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
  miscTypes: { value: string; label: string }[] = [];
  inventoryUnits: { value: string; label: string }[] = [];

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
      { value: 'Spice', label: 'Spice' },
      { value: 'Fining', label: 'Fining' },
      { value: 'Herb', label: 'Herb' },
      { value: 'Flavor', label: 'Flavor' },
      { value: 'Water Agent', label: 'Water Agent' },
      { value: 'Other', label: 'Other' }
    ];

    this.inventoryUnits = [
      { value: 'g', label: 'Grams (g)' },
      { value: 'kg', label: 'Kilograms (kg)' },
      { value: 'l', label: 'Liters (L)' },
      { value: 'ml', label: 'Milliliters (ml)' },
      { value: 'pkg', label: 'Packages' },
      { value: 'items', label: 'Tablets' }
    ];
  }

  initForm(): void {
    this.miscForm = this.fb.group({
      name: ['', Validators.required],
      type: ['Spice', Validators.required],
      unit: ['g', Validators.required],
      amount: [0, [Validators.required, Validators.min(0)]],
      use: ['', Validators.required],
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
          use: misc.use || '',
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
        type: formValue.type,
        unit: formValue.unit,
        amount: formValue.amount,
        use: formValue.use || '',
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
