import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ApiAuthService } from './services/apiauth.service';
import { Router } from '@angular/router';
import { Usuario } from './models/usuario';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  usuario: Usuario | undefined;

  constructor(
    private _title:Title,
    public apiAuthService: ApiAuthService,
    private router:Router
    ){
      this.apiAuthService.usuario?.subscribe(res =>{
        this.usuario = res;
      });
  }

  LogOut(){
    this.apiAuthService.LogOut();
    this.router.navigate(['/login']);
    var matDrawerContent = document.getElementById("matDrawerContent");
    matDrawerContent!.style.marginLeft ="0px";
  }
}
