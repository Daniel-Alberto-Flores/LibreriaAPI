import { Injectable } from "@angular/core";
import { MatSnackBarConfig } from "@angular/material/snack-bar";

@Injectable ({
    providedIn: 'root'
})

export class SnackbarConfigComponent{

    constructor(){        
    }

    public GetConfig():MatSnackBarConfig{

        const matsnackbarConfig: MatSnackBarConfig = {
            duration: 5000,
            horizontalPosition: "center",
            panelClass: 'snackbar-dialog',
            verticalPosition: "bottom"
        }
        return matsnackbarConfig;
    }
}