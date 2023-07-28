import { Component, Inject, OnDestroy, OnInit} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpClient } from '@angular/common/http';
// Componentes
import { SnackbarConfigComponent } from '../../common/snackbarconfig.component';
// Interfaces
import { Libro } from 'src/app/models/libro';
import { Response } from '../../models/response';
// Servicios
import { AutoresApiService } from 'src/app/services/autoresapi.service';
import { LibrosApiService } from 'src/app/services/librosapi.service';
import { Observable, Subject, catchError, map, of, take, takeUntil} from 'rxjs';
import { UploadsApiService } from 'src/app/services/uploadsapiservice';

@Component({
  templateUrl: 'dialoglibro.component.html',
  styleUrls: ['dialoglibro.component.scss'],
})
export class DialogLibroComponent implements OnInit, OnDestroy{
  private _onDestroy = new Subject<void>();
  public listAutores: any[] = [];
  public selectedFile: any = null;
  //public pathPortada: string | undefined;
  public pathPortada: string | ArrayBuffer | null | undefined;
  public filename: string = '';
  public mFileUploaded = false;
  private idLibro?: string;
  private librosFolder:string = '../../assets/images/libros/';  

  public libroForm = this.formBuilder.group({
    idLibro: ['', [Validators.pattern('^[0-9]*$')]],
    nombre: ['', Validators.required],
    publicacion: [
      '',
      [
        Validators.required,
        Validators.pattern('^[0-9]*$'),
        Validators.minLength(4),
        Validators.maxLength(4),
      ],
    ],
    autorId: ['', [Validators.required, Validators.pattern('^[0-9]*$')]],
  });

  constructor(
    @Inject(MAT_DIALOG_DATA) public oLibro: Libro,
    public dialogRef: MatDialogRef<DialogLibroComponent>,
    public librosApi: LibrosApiService,
    private autoresApi: AutoresApiService,
    private uploadsApi: UploadsApiService,
    public snackBar: MatSnackBar,
    public SnackbarConfigComponent: SnackbarConfigComponent,
    private formBuilder: FormBuilder,
    private http: HttpClient
  ) {
    if (this.oLibro !== null) {
      this.idLibro = oLibro.idLibro;
    }
  }  

  ngOnInit() {
    this.autoresApi.GetAutores().pipe(takeUntil(this._onDestroy)).subscribe((response) => {
      this.listAutores = response.data;
    });
    this.getImagePath();
  }

  ngOnDestroy(): void {
    this._onDestroy.next();
    this._onDestroy.complete();
  }

  Close() {
    this.dialogRef.close();
  }

  AddLibro() {
    let response: Response;
    let message: string | undefined;
    this.libroForm.value.idLibro = '0'; // esto es necesario por la validación del servicio
    this.librosApi.Add(this.libroForm.value as Libro).pipe(takeUntil(this._onDestroy)).subscribe((rPost) => {      
      this.dialogRef.close();
      response = rPost;
      message = response.message;

      if (response.success == 1){
        this.idLibro = response.data.idLibro;
        this.updatePortada(message);
      }
      else{
        this.showSnackBar(message);
      }
    });
  }

  EditLibro() {
    let response: Response;
    let message: string | undefined;
    this.libroForm.value.idLibro = this.idLibro;
    this.librosApi.Put(this.libroForm.value as Libro).pipe(takeUntil(this._onDestroy)).subscribe((rPut) => {
      this.dialogRef.close();
      response = rPut;
      message = response.message;
      
      if (response.success == 1){
        this.updatePortada(message);
      }
      else{
        this.showSnackBar(message);
      }           
    });
  }

  updatePortada(message?: string){
    console.log(message);
    if (this.mFileUploaded)
    {
      // Si se pudo añadir el libro intentamos copiar la portada al server
      let fd = new FormData();
      fd.append('_image', this.selectedFile, this.selectedFile.name);
      fd.append('_id', this.idLibro!);

      this.uploadsApi.UploadImage(fd).pipe().subscribe((rUploadImage) =>{
        message = message + " \n " + rUploadImage.message!;
        this.showSnackBar(message);       
      });
    }
    else{
      this.showSnackBar(message);  
    }
  }

  onFileSelected(event: any){

    // Validamos el archivo cargado
    let message: string | undefined;

    message = this.validaImage(event);

    if (message == ''){
      // Actualizamos el archivo seleccionado para luego enviarlo en el post,
      // también el filename y la marca de archivo subido    
      this.selectedFile = <File>event?.target.files[0];
      this.filename = event?.target.files[0].name;
      this.mFileUploaded = true;

      // Actualizamos la vista previa
      this.updateImagePreview();
    }
    else{
      this.showSnackBar(message);  
    }    
  }

  updateImagePreview()
  {
    // Si se carga un archivo desde el input, actualizamos la vista previa
    if (this.selectedFile) {
      const fReader = new FileReader();
      fReader.readAsDataURL(this.selectedFile);
      fReader.onloadend = (event) => {
        this.pathPortada = event.target!.result;
      }
    }
  }

  getImagePath(){
    // Formamos el nombre del archivo y la ruta
    this.filename = this.idLibro + '.jpg';
    let path = this.librosFolder + this.filename;

    return this.fileExists(path).pipe(take(1)).subscribe(rFileExists =>{      
      if (!rFileExists){
        // Si el archivo no existe, cargamos el archivo predeterminado
        this.filename = '0.jpg';
        path = this.librosFolder + this.filename;
      }
      else{
        // Si el archivo existe seteamos la marca
        this.mFileUploaded = true;        
      }
      this.pathPortada = path;
      if (this.selectedFile == null && this.mFileUploaded){
        // Si el archivo existe y se subió anteriormente (porque estamos editando)
        // Lo buscamos en la ruta y lo cargamos al input
        this.getImageFile().pipe(takeUntil(this._onDestroy)).subscribe(rGetImageFile =>{
          let oFile = new File([rGetImageFile], this.filename, {type: "image/jpeg"});
          this.selectedFile = oFile;
        });
      }
    });
  }

  fileExists(path: string): Observable<boolean> {
    return this.http
      .get(path, { observe: 'response', responseType: 'blob' })
      .pipe(
        map(rFileExists => {
          return true;
        }),
        catchError(error => {
          return of(false);
        })
      );
  }

  showSnackBar(_message?:string){
    this.snackBar.open(
      _message!,
      '',
      this.SnackbarConfigComponent.GetConfig()
    );
  }

  getImageFile(){
    return this.http.get(this.pathPortada!.toString(), { responseType: 'blob' }).pipe(takeUntil(this._onDestroy));
  }

  validaImage(_file: any): string{

    let file = <File>_file?.target.files[0];
    if (file.size == 0){
      return 'Archivo cargado sin datos.';
    }
    if (file.size > 400000){
      return 'El archivo cargado no puede superar los 400 kb.';
    }
    if (file.type != 'image/jpeg'){
      return 'Extensión de imagen inválida. Solo se permiten las extensiones ".jpg".';
    }

    return '';
  }
}