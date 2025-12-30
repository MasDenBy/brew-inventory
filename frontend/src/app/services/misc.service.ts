import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Misc } from '../models/misc.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MiscService {
  private apiUrl = `${environment.apiUrl}/miscs`;

  constructor(private http: HttpClient) { }

  getMiscs(): Observable<Misc[]> {
    return this.http.get<Misc[]>(this.apiUrl);
  }

  getMisc(id: number): Observable<Misc> {
    return this.http.get<Misc>(`${this.apiUrl}/${id}`);
  }

  addMisc(misc: Misc): Observable<Misc> {
    return this.http.post<Misc>(this.apiUrl, misc);
  }

  updateMisc(id: number, misc: Misc): Observable<Misc> {
    return this.http.put<Misc>(`${this.apiUrl}/${id}`, misc);
  }

  deleteMisc(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
