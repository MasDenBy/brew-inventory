import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { YeastService } from '../../services/yeast.service';
import { Yeast, YeastType, YeastForm } from '../../models/yeast.model';

@Component({
  selector: 'app-yeast-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './yeast-form.component.html',
  styleUrls: ['./yeast-form.component.css']
})
export class YeastFormComponent implements OnInit {
  yeastForm!: FormGroup;
  isEditMode = false;
  yeastId?: number;
  isSubmitting = false;
  error: string | null = null;
  yeastTypes = Object.values(YeastType);
  yeastForms = Object.values(YeastForm);

  constructor(
    private fb: FormBuilder,
    private yeastService: YeastService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.initForm();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.yeastId = +params['id'];
        this.loadYeast(this.yeastId);
      }
    });
  }

  initForm(): void {
    this.yeastForm = this.fb.group({
      name: ['', Validators.required],
      laboratory: ['', Validators.required],
      type: [YeastType.Ale, Validators.required],
      form: [YeastForm.Dry, Validators.required],
      amount: [0, [Validators.required, Validators.min(0)]],
      bestBefore: [''],
      brewfatherId: ['']
    });
  }

  loadYeast(id: number): void {
    this.yeastService.getYeast(id).subscribe({
      next: (yeast) => {
        this.yeastForm.patchValue({
          name: yeast.name,
          laboratory: yeast.laboratory,
          type: yeast.type,
          form: yeast.form,
          amount: yeast.amount,
          bestBefore: yeast.bestBefore || '',
          brewfatherId: yeast.brewfatherId || ''
        });
      },
      error: (err) => {
        this.error = 'Failed to load yeast';
        console.error('Error loading yeast:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.yeastForm.valid) {
      this.isSubmitting = true;
      this.error = null;

      const formValue = this.yeastForm.value;
      const yeast: Yeast = {
        id: this.yeastId || 0,
        name: formValue.name,
        laboratory: formValue.laboratory,
        type: formValue.type,
        form: formValue.form,
        amount: formValue.amount,
        bestBefore: formValue.bestBefore || null,
        brewfatherId: formValue.brewfatherId || null
      };

      const request = this.isEditMode
        ? this.yeastService.updateYeast(this.yeastId!, yeast)
        : this.yeastService.addYeast(yeast);

      request.subscribe({
        next: () => {
          this.router.navigate(['/yeasts']);
        },
        error: (err) => {
          this.error = this.isEditMode 
            ? 'Failed to update yeast' 
            : 'Failed to add yeast';
          this.isSubmitting = false;
          console.error('Error saving yeast:', err);
        }
      });
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.yeastForm.controls).forEach(key => {
        this.yeastForm.get(key)?.markAsTouched();
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/yeasts']);
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.yeastForm.get(fieldName);
    return !!(field && field.invalid && field.touched);
  }
}
