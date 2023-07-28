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

export class UploadsApiService {
    url: string = 'https://localhost:7091/api/Upload/';
  
    constructor(private http: HttpClient) {}

    UploadImage(_formData: FormData):Observable<Response> {
      return this.http.post<Response>(this.url + 'UploadImage', _formData);
    }
}