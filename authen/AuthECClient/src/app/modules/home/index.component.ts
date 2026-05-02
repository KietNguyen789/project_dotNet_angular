import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'home-component',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})
export class HomeComponent {

    errorMsg = '';
    loading = false;

    constructor(private router: Router) {

    }




}
