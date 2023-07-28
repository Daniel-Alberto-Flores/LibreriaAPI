import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ExportaXlsx {
  constructor(public datepipe: DatePipe) {}

  ExportaXlsx(_data: any) {
    let oDate: string = this.datepipe.transform(new Date(), 'dd_MM_yyyy HH.mm')!;
    let filename = 'Listado de libros ' + oDate;

    const byteCharacters = atob(_data);
    const byteNumbers = new Array(byteCharacters.length);

    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }

    const byteArray = new Uint8Array(byteNumbers);
    let blob = new Blob([byteArray], {
      type: 'application / vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    });

    const url = window.URL.createObjectURL(blob);
    const anchor = document.createElement('a');
    anchor.download = `${filename}.xlsx`;
    anchor.href = url;
    anchor.click();
  }
}
