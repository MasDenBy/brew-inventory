import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface SyncResponse {
  message?: string;
  error?: string;
}

@Injectable({
  providedIn: 'root'
})
export class SyncService {
  private apiUrl = `${environment.apiUrl}/sync`;

  constructor(private http: HttpClient) { }

  syncFermentables(): Observable<SyncResponse> {
    return this.http.post<SyncResponse>(`${this.apiUrl}/fermentables`, {});
  }

  // Placeholder for future sync methods
  syncHops(): Observable<SyncResponse> {
    return this.http.post<SyncResponse>(`${this.apiUrl}/hops`, {});
  }

  syncYeasts(): Observable<SyncResponse> {
    return this.http.post<SyncResponse>(`${this.apiUrl}/yeasts`, {});
  }

  syncMiscs(): Observable<SyncResponse> {
    return this.http.post<SyncResponse>(`${this.apiUrl}/miscs`, {});
  }

  syncRecipes(): Observable<SyncResponse> {
    return this.http.post<SyncResponse>(`${this.apiUrl}/recipes`, {});
  }
}
