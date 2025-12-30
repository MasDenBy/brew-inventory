import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { Ingredient } from '../models/ingredient.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private apiUrl = environment.apiUrl;
  // Subject used to broadcast the ingredient that should be edited
  private editIngredientSource = new BehaviorSubject<Ingredient | null>(null);
  public editIngredient$ = this.editIngredientSource.asObservable();

  constructor(private http: HttpClient) { }

  // Start editing: emit the ingredient to any subscribers (e.g. the form)
  startEdit(ingredient: Ingredient): void {
    this.editIngredientSource.next(ingredient);
  }

  // Clear the current editing ingredient
  clearEdit(): void {
    this.editIngredientSource.next(null);
  }

  getIngredients(): Observable<Ingredient[]> {
    return this.http.get<Ingredient[]>(`${this.apiUrl}/ingredients`);
  }

  getIngredient(id: number): Observable<Ingredient> {
    return this.http.get<Ingredient>(`${this.apiUrl}/ingredients/${id}`);
  }

  addIngredient(ingredient: Ingredient): Observable<Ingredient> {
    return this.http.post<Ingredient>(`${this.apiUrl}/ingredients`, ingredient);
  }

  updateIngredient(id: number, ingredient: Ingredient): Observable<Ingredient> {
    return this.http.put<Ingredient>(`${this.apiUrl}/ingredients/${id}`, ingredient);
  }

  deleteIngredient(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/ingredients/${id}`);
  }
}