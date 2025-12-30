import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HopService } from '../../services/hop.service';
import { Hop } from '../../models/hop.model';

@Component({
  selector: 'app-hop-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './hop-list.component.html',
  styleUrls: ['./hop-list.component.css']
})
export class HopListComponent implements OnInit {
  hops: Hop[] = [];
  isLoading = false;
  error: string | null = null;

  constructor(
    private hopService: HopService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadHops();
  }

  loadHops(): void {
    this.isLoading = true;
    this.error = null;
    this.hopService.getHops().subscribe({
      next: (data) => {
        this.hops = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load hops';
        this.isLoading = false;
        console.error('Error loading hops:', err);
      }
    });
  }

  addHop(): void {
    this.router.navigate(['/hops/add']);
  }

  editHop(id: number): void {
    this.router.navigate(['/hops/edit', id]);
  }

  deleteHop(id: number): void {
    if (confirm('Are you sure you want to delete this hop?')) {
      this.hopService.deleteHop(id).subscribe({
        next: () => {
          this.loadHops();
        },
        error: (err) => {
          this.error = 'Failed to delete hop';
          console.error('Error deleting hop:', err);
        }
      });
    }
  }
}
