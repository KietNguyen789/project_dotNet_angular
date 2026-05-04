import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserItem } from '../../models/auth.models';

@Component({
    selector: 'home-component',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})
export class HomeComponent implements OnInit {
    users: UserItem[] = [];
    loading = false;
    errorMsg = '';

    constructor(private router: Router, private auth: AuthService) { }

    ngOnInit(): void {
        this.loadUsers();
    }

    loadUsers(): void {
        this.loading = true;
        this.auth.getListUser().subscribe({
            next: (data) => {
                debugger
                this.users = data;
                this.loading = false;
            },
            error: (err) => {
                this.errorMsg = err?.error?.message || 'Failed to load users.';
                this.loading = false;
                if (err.status === 401) {
                    this.router.navigate(['/login']);
                }
            }
        });
    }
}
