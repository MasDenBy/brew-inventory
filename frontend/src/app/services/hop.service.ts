import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Hop } from '../models/hop.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HopService {
  private apiUrl = `${environment.apiUrl}/hops`;

  constructor(private http: HttpClient) { }

  getHops(): Observable<Hop[]> {
    return this.http.get<Hop[]>(this.apiUrl);
  }

  getHop(id: number): Observable<Hop> {
    return this.http.get<Hop>(`${this.apiUrl}/${id}`);
  }

  addHop(hop: Hop): Observable<Hop> {
    return this.http.post<Hop>(this.apiUrl, hop);
  }

  updateHop(id: number, hop: Hop): Observable<Hop> {
    return this.http.put<Hop>(`${this.apiUrl}/${id}`, hop);
  }

  deleteHop(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
