import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MiscService } from '../../services/misc.service';
import { Misc } from '../../models/misc.model';

@Component({
  selector: 'app-misc-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './misc-list.component.html',
  styleUrls: ['./misc-list.component.css']
})
export class MiscListComponent implements OnInit {
  miscs: Misc[] = [];
  isLoading = false;
  error: string | null = null;

  constructor(
    private miscService: MiscService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadMiscs();
  }

  loadMiscs(): void {
    this.isLoading = true;
    this.error = null;
    this.miscService.getMiscs().subscribe({
      next: (data) => {
        this.miscs = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load miscs';
        this.isLoading = false;
        console.error('Error loading miscs:', err);
      }
    });
  }

  addMisc(): void {
    this.router.navigate(['/miscs/add']);
  }

  editMisc(id: number): void {
    this.router.navigate(['/miscs/edit', id]);
  }

  deleteMisc(id: number): void {
    if (confirm('Are you sure you want to delete this misc?')) {
      this.miscService.deleteMisc(id).subscribe({
        next: () => {
          this.loadMiscs();
        },
        error: (err) => {
          this.error = 'Failed to delete misc';
          console.error('Error deleting misc:', err);
        }
      });
    }
  }

}
