import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Yeast } from '../models/yeast.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class YeastService {
  private apiUrl = `${environment.apiUrl}/yeasts`;

  constructor(private http: HttpClient) { }

  getYeasts(): Observable<Yeast[]> {
    return this.http.get<Yeast[]>(this.apiUrl);
  }

  getYeast(id: number): Observable<Yeast> {
    return this.http.get<Yeast>(`${this.apiUrl}/${id}`);
  }

  addYeast(yeast: Yeast): Observable<Yeast> {
    return this.http.post<Yeast>(this.apiUrl, yeast);
  }

  updateYeast(id: number, yeast: Yeast): Observable<Yeast> {
    return this.http.put<Yeast>(`${this.apiUrl}/${id}`, yeast);
  }

  deleteYeast(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
