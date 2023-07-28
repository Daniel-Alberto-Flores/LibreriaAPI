import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
// Componentes
import { SnackbarConfigComponent } from '../common/snackbarconfig.component';
// Interfaces
import { Login } from '../models/login';
// Servicios
import { ApiAuthService } from '../services/apiauth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  title = 'Login';

  public loginForm = this.formBuilder.group({
    email: ['', Validators.required],
    password: ['', Validators.required]
  });

  constructor(
    private _title:Title,
    public apiAuthService: ApiAuthService,
    private router: Router,
    private formBuilder: FormBuilder,
    public snackBar: MatSnackBar,
    public SnackbarConfigComponent: SnackbarConfigComponent,
    ){      
      if (this.apiAuthService.usuarioData){
        this.router.navigate(['/'])
    }    
  }

  ngOnInit() {
    this._title.setTitle(this.title);
  }

  Login(){
      this.apiAuthService.Login(this.loginForm.value as Login).subscribe(response =>{          
          if (response.success === 1){
              sessionStorage.setItem('user', this.loginForm.value.email!);
              this.router.navigate(['/home']);
          }
          else
          {
            if (response.success === 0){
              this.snackBar.open(response.message!,'', this.SnackbarConfigComponent.GetConfig()); 
            }
          }
      });
  }
}
