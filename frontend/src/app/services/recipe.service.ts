import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RecipeListResponse, RecipeDetailsResponse } from '../models/recipe.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private apiUrl = `${environment.apiUrl}/recipes`;

  constructor(private http: HttpClient) { }

  getRecipes(): Observable<RecipeListResponse[]> {
    return this.http.get<RecipeListResponse[]>(this.apiUrl);
  }

  getRecipe(id: number): Observable<RecipeDetailsResponse> {
    return this.http.get<RecipeDetailsResponse>(`${this.apiUrl}/${id}`);
  }
}
