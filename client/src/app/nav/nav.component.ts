import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  currentUser$: Observable<User> = this.accountService.currentUser$;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
  }

  Login() {
    this.accountService.login(this.model).subscribe(response => {
      console.log(response)
    }, error => {
      console.log(error);
    })
  }

  LogOut() {
    this.accountService.logout();
  }
}
