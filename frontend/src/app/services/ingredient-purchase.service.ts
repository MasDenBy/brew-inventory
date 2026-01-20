import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IngredientPurchaseRequest, IngredientPurchaseResponse } from '../models/ingredient-purchase.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class IngredientPurchaseService {
  private apiUrl = `${environment.apiUrl}/ingredient-purchase`;

  constructor(private http: HttpClient) { }

  calculatePurchaseNeeds(recipeIds: number[]): Observable<IngredientPurchaseResponse> {
    const request: IngredientPurchaseRequest = { recipeIds };
    return this.http.post<IngredientPurchaseResponse>(`${this.apiUrl}/calculate`, request);
  }

  exportPurchaseNeeds(recipeIds: number[]): Observable<Blob> {
    const request: IngredientPurchaseRequest = { recipeIds };
    // Assume backend now returns Excel file (e.g., .xlsx)
    return this.http.post(`${this.apiUrl}/export`, request, { responseType: 'blob' });
  }
}