import { Routes } from '@angular/router';
import { FermentableListComponent } from './components/fermentable-list/fermentable-list.component';
import { FermentableFormComponent } from './components/fermentable-form/fermentable-form.component';
import { HopListComponent } from './components/hop-list/hop-list.component';
import { HopFormComponent } from './components/hop-form/hop-form.component';
import { YeastListComponent } from './components/yeast-list/yeast-list.component';
import { YeastFormComponent } from './components/yeast-form/yeast-form.component';
import { MiscListComponent } from './components/misc-list/misc-list.component';
import { MiscFormComponent } from './components/misc-form/misc-form.component';
import { SyncComponent } from './components/sync/sync.component';

export const routes: Routes = [
  { path: '', component: FermentableListComponent },
  { path: 'fermentables', component: FermentableListComponent },
  { path: 'fermentables/add', component: FermentableFormComponent },
  { path: 'fermentables/edit/:id', component: FermentableFormComponent },
  { path: 'hops', component: HopListComponent },
  { path: 'hops/add', component: HopFormComponent },
  { path: 'hops/edit/:id', component: HopFormComponent },
  { path: 'yeasts', component: YeastListComponent },
  { path: 'yeasts/add', component: YeastFormComponent },
  { path: 'yeasts/edit/:id', component: YeastFormComponent },
  { path: 'miscs', component: MiscListComponent },
  { path: 'miscs/add', component: MiscFormComponent },
  { path: 'miscs/edit/:id', component: MiscFormComponent },
  { path: 'sync', component: SyncComponent }
];
