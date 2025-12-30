import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FermentableService } from '../../services/fermentable.service';
import { Fermentable } from '../../models/fermentable.model';

@Component({
  selector: 'app-fermentable-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './fermentable-list.component.html',
  styleUrls: ['./fermentable-list.component.css']
})
export class FermentableListComponent implements OnInit {
  fermentables: Fermentable[] = [];
  isLoading = false;
  error: string | null = null;

  constructor(
    private fermentableService: FermentableService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadFermentables();
  }

  loadFermentables(): void {
    this.isLoading = true;
    this.error = null;
    this.fermentableService.getFermentables().subscribe({
      next: (data) => {
        this.fermentables = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load fermentables';
        this.isLoading = false;
        console.error('Error loading fermentables:', err);
      }
    });
  }

  addFermentable(): void {
    this.router.navigate(['/fermentables/add']);
  }

  editFermentable(id: number): void {
    this.router.navigate(['/fermentables/edit', id]);
  }

  deleteFermentable(id: number): void {
    if (confirm('Are you sure you want to delete this fermentable?')) {
      this.fermentableService.deleteFermentable(id).subscribe({
        next: () => {
          this.loadFermentables();
        },
        error: (err) => {
          this.error = 'Failed to delete fermentable';
          console.error('Error deleting fermentable:', err);
        }
      });
    }
  }
}
