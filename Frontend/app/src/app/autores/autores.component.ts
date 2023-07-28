import { Component } from '@angular/core';
// Servicios
import { AutoresApiService } from '../services/autoresapi.service';

@Component({
  selector: 'app-autores',
  templateUrl: './autores.component.html',
  styleUrls: ['./autores.component.scss']
})
export class AutoresComponent {
  public listAutores: any[] = [];

  constructor(
    private autoresApi: AutoresApiService
  ){
  }

  public GetAutores() {
    this.autoresApi.GetAutores().subscribe(response => {
      this.listAutores = response.data;      
    });    
  }

}
