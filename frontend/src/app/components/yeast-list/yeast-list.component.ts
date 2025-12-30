import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { YeastService } from '../../services/yeast.service';
import { Yeast } from '../../models/yeast.model';

@Component({
  selector: 'app-yeast-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './yeast-list.component.html',
  styleUrls: ['./yeast-list.component.css']
})
export class YeastListComponent implements OnInit {
  yeasts: Yeast[] = [];
  isLoading = false;
  error: string | null = null;

  constructor(
    private yeastService: YeastService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadYeasts();
  }

  loadYeasts(): void {
    this.isLoading = true;
    this.error = null;
    this.yeastService.getYeasts().subscribe({
      next: (data) => {
        this.yeasts = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load yeasts';
        this.isLoading = false;
        console.error('Error loading yeasts:', err);
      }
    });
  }

  addYeast(): void {
    this.router.navigate(['/yeasts/add']);
  }

  editYeast(id: number): void {
    this.router.navigate(['/yeasts/edit', id]);
  }

  deleteYeast(id: number): void {
    if (confirm('Are you sure you want to delete this yeast?')) {
      this.yeastService.deleteYeast(id).subscribe({
        next: () => {
          this.loadYeasts();
        },
        error: (err) => {
          this.error = 'Failed to delete yeast';
          console.error('Error deleting yeast:', err);
        }
      });
    }
  }
}
