import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Fermentable } from '../models/fermentable.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FermentableService {
  private apiUrl = `${environment.apiUrl}/fermentables`;

  constructor(private http: HttpClient) { }

  getFermentables(): Observable<Fermentable[]> {
    return this.http.get<Fermentable[]>(this.apiUrl);
  }

  getFermentable(id: number): Observable<Fermentable> {
    return this.http.get<Fermentable>(`${this.apiUrl}/${id}`);
  }

  addFermentable(fermentable: Fermentable): Observable<Fermentable> {
    return this.http.post<Fermentable>(this.apiUrl, fermentable);
  }

  updateFermentable(id: number, fermentable: Fermentable): Observable<Fermentable> {
    return this.http.put<Fermentable>(`${this.apiUrl}/${id}`, fermentable);
  }

  deleteFermentable(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
