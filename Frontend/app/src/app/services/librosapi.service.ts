import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
// Interfaces
import { Libro } from '../models/libro';
import { Response } from '../models/response';

const httpOption = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
  }),
};

@Injectable({
  providedIn: 'root',
})
export class LibrosApiService {
  url: string = 'https://localhost:7091/api/libro/';

  constructor(private http: HttpClient) {}

  GetLibros(): Observable<Response> {
    return this.http.get<Response>(this.url + 'GetLibros');
  }

  GetLibro(_id: number): Observable<Response> {
    return this.http.get<Response>(`${this.url}GetLibro/${_id}`);
  }

  Add(oLibro: Libro): Observable<Response> {
    return this.http.post<Response>(this.url + 'Add', oLibro, httpOption);
  }

  Put(oLibro: Libro): Observable<Response> {
    return this.http.put<Response>(this.url, oLibro, httpOption);
  }
}
