import { Title } from '@angular/platform-browser';
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
// Componentes
import { DialogLibroComponent } from './dialog/dialoglibrocomponent';
import { SnackbarConfigComponent } from '../common/snackbarconfig.component';
import { ExportaXlsx } from '../common/archivos/exportaxlsx.component';
// Interfaces
import { Libro } from '../models/libro';
// Servicios
import { ArchivosApiService } from '../services/archivosapi.service';
import { LibrosApiService } from '../services/librosapi.service';

@Component({
  selector: 'app-libros',
  templateUrl: './libros.component.html',
  styleUrls: ['./libros.component.scss']
})
export class LibrosComponent {
  title = 'Listado de libros';
  //readonly dialogWidth: string  = '480px';
  //readonly dialogHeigth: string  = '350px';
  //readonly dialogWidth: string  = '480px';
  //readonly dialogHeigth: string  = '650px';
  readonly dialogWidth: string  = '480px';
  readonly dialogHeigth: string  = '680px';
  public listLibros: any[] = [];  
  public columnas: string[] = ['IdLibro', 'Nombre', 'Publicacion', 'AutorId', 'Actions'];  

  constructor(
    private _title:Title,
    private librosApi: LibrosApiService,
    private archivosApi: ArchivosApiService,
    public dialog: MatDialog,
    public snackBar: MatSnackBar,
    public SnackbarConfigComponent: SnackbarConfigComponent,
    private exportaxlsx: ExportaXlsx
  ){
  }

  ngOnInit(): void {
    this._title.setTitle(this.title);
    this.GetLibros();
  }

  GetLibros() {
    this.librosApi.GetLibros().subscribe(response => {
      this.listLibros = response.data;
    });
  }  

  ExportarListadoLibros(){

    this.archivosApi.GeneraListadoLibros().subscribe(response =>{
      if (response.success === 1){
        this.exportaxlsx.ExportaXlsx(response.data);
      };
      this.snackBar.open(response.message!,'', this.SnackbarConfigComponent.GetConfig()); 
    });
  }
  
  OpenAdd(){
    const dialogRef = this.dialog.open(DialogLibroComponent, {
      width: this.dialogWidth,
      height: this.dialogHeigth,
      panelClass: 'libros-component-dialog-ref'
    });
    dialogRef.afterClosed().subscribe(result => {
      this.GetLibros();
    });
  }

  OpenEdit(oLibro:Libro){
    const dialogRef = this.dialog.open(DialogLibroComponent, {
      width: this.dialogWidth,
      height: this.dialogHeigth,
      panelClass: 'libros-component-dialog-ref',
      data: oLibro
    });
    dialogRef.afterClosed().subscribe(result => {
      this.GetLibros();
    });
  } 

}
function saveAs(file: File) {
  throw new Error('Function not implemented.');
}

