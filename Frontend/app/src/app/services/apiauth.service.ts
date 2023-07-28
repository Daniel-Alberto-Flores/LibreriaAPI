import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {BehaviorSubject, Observable} from 'rxjs';
import {map} from 'rxjs/operators';
// Interfaces
import {Response} from '../models/response';
import {Usuario} from "../models/usuario";
import {Login} from "../models/login";

const httpOption = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

@Injectable({
    providedIn: 'root'
})

export class ApiAuthService{
    url: string = "https://localhost:7091/api/usuario/login";

    private usuarioSubject: BehaviorSubject<Usuario>;
    public usuario: Observable<Usuario> | undefined;

    public get usuarioData(): Usuario{
        return this.usuarioSubject.value;
    }

    constructor(
        private _http: HttpClient){
            this.usuarioSubject = new BehaviorSubject<Usuario>(JSON.parse(localStorage.getItem('usuario')!));
            this.usuario = this.usuarioSubject.asObservable();
    }

    Login (login?: Login): Observable<Response>{
        return this._http.post<Response>(this.url, login, httpOption).pipe(
            map(res => {
                if (res.success === 1){
                    const usuario: Usuario = res.data;
                    localStorage.setItem('usuario', JSON.stringify(usuario));
                    this.usuarioSubject?.next(usuario);
                }
                return res;
            })
        );
    }

    LogOut(){
        localStorage.removeItem('usuario');
        this.usuarioSubject.next(null!);        
    }
}