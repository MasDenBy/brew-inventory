import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SyncService } from '../../services/sync.service';

interface SyncItem {
  name: string;
  displayName: string;
  icon: string;
  status: 'idle' | 'syncing' | 'success' | 'error';
  message?: string;
  syncMethod: () => void;
}

@Component({
  selector: 'app-sync',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sync.component.html',
  styleUrls: ['./sync.component.css']
})
export class SyncComponent {
  syncItems: SyncItem[] = [
    {
      name: 'fermentables',
      displayName: 'Fermentables',
      icon: '🌾',
      status: 'idle',
      syncMethod: () => this.syncFermentables()
    },
    {
      name: 'hops',
      displayName: 'Hops',
      icon: '🌿',
      status: 'idle',
      syncMethod: () => this.syncHops()
    },
    {
      name: 'yeasts',
      displayName: 'Yeasts',
      icon: '🧫',
      status: 'idle',
      syncMethod: () => this.syncYeasts()
    },
    {
      name: 'miscs',
      displayName: 'Miscs',
      icon: '🧪',
      status: 'idle',
      syncMethod: () => this.syncMiscs()
    },
    {
      name: 'recipes',
      displayName: 'Recipes',
      icon: '📖',
      status: 'idle',
      syncMethod: () => this.syncRecipes()
    }
  ];

  isSyncingAll = false;

  constructor(private syncService: SyncService) {}

  syncAll(): void {
    this.isSyncingAll = true;
    let completedSyncs = 0;
    const totalSyncs = 4; // fermentables, hops, yeasts, and miscs

    const checkAllComplete = () => {
      completedSyncs++;
      if (completedSyncs >= totalSyncs) {
        this.isSyncingAll = false;
      }
    };

    // Sync fermentables
    const fermentableItem = this.syncItems.find(i => i.name === 'fermentables');
    if (fermentableItem) {
      fermentableItem.status = 'syncing';
      fermentableItem.message = undefined;
      this.syncService.syncFermentables().subscribe({
        next: (response) => {
          fermentableItem.status = 'success';
          fermentableItem.message = response.message || 'Fermentables synced successfully!';
          checkAllComplete();
        },
        error: (error) => {
          fermentableItem.status = 'error';
          fermentableItem.message = error.error?.error || 'Failed to sync fermentables. Please try again.';
          checkAllComplete();
        }
      });
    }

    // Sync hops
    const hopItem = this.syncItems.find(i => i.name === 'hops');
    if (hopItem) {
      hopItem.status = 'syncing';
      hopItem.message = undefined;
      this.syncService.syncHops().subscribe({
        next: (response) => {
          hopItem.status = 'success';
          hopItem.message = response.message || 'Hops synced successfully!';
          checkAllComplete();
        },
        error: (error) => {
          hopItem.status = 'error';
          hopItem.message = error.error?.error || 'Failed to sync hops. Please try again.';
          checkAllComplete();
        }
      });
    }

    // Sync yeasts
    const yeastItem = this.syncItems.find(i => i.name === 'yeasts');
    if (yeastItem) {
      yeastItem.status = 'syncing';
      yeastItem.message = undefined;
      this.syncService.syncYeasts().subscribe({
        next: (response) => {
          yeastItem.status = 'success';
          yeastItem.message = response.message || 'Yeasts synced successfully!';
          checkAllComplete();
        },
        error: (error) => {
          yeastItem.status = 'error';
          yeastItem.message = error.error?.error || 'Failed to sync yeasts. Please try again.';
          checkAllComplete();
        }
      });
    }

    // Sync miscs
    const miscItem = this.syncItems.find(i => i.name === 'miscs');
    if (miscItem) {
      miscItem.status = 'syncing';
      miscItem.message = undefined;
      this.syncService.syncMiscs().subscribe({
        next: (response) => {
          miscItem.status = 'success';
          miscItem.message = response.message || 'Miscs synced successfully!';
          checkAllComplete();
        },
        error: (error) => {
          miscItem.status = 'error';
          miscItem.message = error.error?.error || 'Failed to sync miscs. Please try again.';
          checkAllComplete();
        }
      });
    }
  }

  syncFermentables(): void {
    const item = this.syncItems.find(i => i.name === 'fermentables');
    if (!item) return;

    item.status = 'syncing';
    item.message = undefined;

    this.syncService.syncFermentables().subscribe({
      next: (response) => {
        item.status = 'success';
        item.message = response.message || 'Fermentables synced successfully!';
        this.isSyncingAll = false;
      },
      error: (error) => {
        item.status = 'error';
        item.message = error.error?.error || 'Failed to sync fermentables. Please try again.';
        this.isSyncingAll = false;
      }
    });
  }

  syncHops(): void {
    const item = this.syncItems.find(i => i.name === 'hops');
    if (!item) return;

    item.status = 'syncing';
    item.message = undefined;

    this.syncService.syncHops().subscribe({
      next: (response) => {
        item.status = 'success';
        item.message = response.message || 'Hops synced successfully!';
        this.isSyncingAll = false;
      },
      error: (error) => {
        item.status = 'error';
        item.message = error.error?.error || 'Failed to sync hops. Please try again.';
        this.isSyncingAll = false;
      }
    });
  }

  syncYeasts(): void {
    const item = this.syncItems.find(i => i.name === 'yeasts');
    if (!item) return;

    item.status = 'syncing';
    item.message = undefined;

    this.syncService.syncYeasts().subscribe({
      next: (response) => {
        item.status = 'success';
        item.message = response.message || 'Yeasts synced successfully!';
        this.isSyncingAll = false;
      },
      error: (error) => {
        item.status = 'error';
        item.message = error.error?.error || 'Failed to sync yeasts. Please try again.';
        this.isSyncingAll = false;
      }
    });
  }

  syncMiscs(): void {
    const item = this.syncItems.find(i => i.name === 'miscs');
    if (!item) return;

    item.status = 'syncing';
    item.message = undefined;

    this.syncService.syncMiscs().subscribe({
      next: (response) => {
        item.status = 'success';
        item.message = response.message || 'Miscs synced successfully!';
        this.isSyncingAll = false;
      },
      error: (error) => {
        item.status = 'error';
        item.message = error.error?.error || 'Failed to sync miscs. Please try again.';
        this.isSyncingAll = false;
      }
    });
  }

  syncRecipes(): void {
    const item = this.syncItems.find(i => i.name === 'recipes');
    if (!item) return;

    item.status = 'syncing';
    item.message = 'Coming soon...';
    setTimeout(() => {
      item.status = 'idle';
    }, 2000);
  }

  getStatusIcon(status: string): string {
    switch (status) {
      case 'syncing': return '⏳';
      case 'success': return '✅';
      case 'error': return '❌';
      default: return '';
    }
  }
}
