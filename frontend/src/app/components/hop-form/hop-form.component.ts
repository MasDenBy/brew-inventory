import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HopService } from '../../services/hop.service';
import { Hop, HopType } from '../../models/hop.model';

@Component({
  selector: 'app-hop-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './hop-form.component.html',
  styleUrls: ['./hop-form.component.css']
})
export class HopFormComponent implements OnInit {
  hopForm!: FormGroup;
  isEditMode = false;
  hopId?: number;
  isSubmitting = false;
  error: string | null = null;
  hopTypes = Object.values(HopType);

  constructor(
    private fb: FormBuilder,
    private hopService: HopService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.initForm();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.hopId = +params['id'];
        this.loadHop(this.hopId);
      }
    });
  }

  initForm(): void {
    this.hopForm = this.fb.group({
      name: ['', Validators.required],
      origin: [''],
      type: [HopType.Pellet, Validators.required],
      alphaAcid: [0, [Validators.required, Validators.min(0), Validators.max(100)]],
      harvestYear: [''],
      amount: [0, [Validators.required, Validators.min(0)]],
      bestBefore: [''],
      brewfatherId: ['']
    });
  }

  loadHop(id: number): void {
    this.hopService.getHop(id).subscribe({
      next: (hop) => {
        this.hopForm.patchValue({
          name: hop.name,
          origin: hop.origin || '',
          type: hop.type,
          alphaAcid: hop.alphaAcid,
          harvestYear: hop.harvestYear || '',
          amount: hop.amount,
          bestBefore: hop.bestBefore || '',
          brewfatherId: hop.brewfatherId || ''
        });
      },
      error: (err) => {
        this.error = 'Failed to load hop';
        console.error('Error loading hop:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.hopForm.valid) {
      this.isSubmitting = true;
      this.error = null;

      const formValue = this.hopForm.value;
      const hop: Hop = {
        id: this.hopId || 0,
        name: formValue.name,
        origin: formValue.origin || null,
        type: formValue.type,
        alphaAcid: formValue.alphaAcid,
        harvestYear: formValue.harvestYear || null,
        amount: formValue.amount,
        bestBefore: formValue.bestBefore || null,
        brewfatherId: formValue.brewfatherId || null
      };

      const request = this.isEditMode
        ? this.hopService.updateHop(this.hopId!, hop)
        : this.hopService.addHop(hop);

      request.subscribe({
        next: () => {
          this.router.navigate(['/hops']);
        },
        error: (err) => {
          this.error = this.isEditMode 
            ? 'Failed to update hop' 
            : 'Failed to add hop';
          this.isSubmitting = false;
          console.error('Error saving hop:', err);
        }
      });
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.hopForm.controls).forEach(key => {
        this.hopForm.get(key)?.markAsTouched();
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/hops']);
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.hopForm.get(fieldName);
    return !!(field && field.invalid && field.touched);
  }
}
