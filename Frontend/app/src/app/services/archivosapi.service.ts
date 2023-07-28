import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
// Interfaces
import { Response } from '../models/response';

const httpOption = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

@Injectable({
    providedIn: 'root'
})
export class ArchivosApiService{

    url: string = 'https://localhost:7091/api/Archivo/';

    constructor(private http: HttpClient) {}

    GeneraListadoLibros(): Observable<Response> {
        return this.http.get<Response>(this.url + 'GeneraListadoLibros');
      }
}