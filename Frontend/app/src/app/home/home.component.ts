import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  title = 'Home';

  constructor(
    private _title:Title
    ){
  }

  ngOnInit(){
    this._title.setTitle(this.title);
  }
}
