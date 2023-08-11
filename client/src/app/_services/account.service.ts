import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';

  //Storing user data here. So, all components can use it.
  //using special observable called behavior
  private currentSourceUser = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentSourceUser.asObservable();


  constructor(private http:HttpClient) { }

  login(model: any)
  {
    return this.http.post<User>(this.baseUrl + 'account/login',model).pipe(
      map((response:User) => {
        const user = response;
        //storing user data in browser.In inspectElement>application>localstorage.
        //This will remember username and token when user close the browser and reopen it.
        if(user){
          localStorage.setItem('user',JSON.stringify(user)); 
          this.currentSourceUser.next(user);
        }

      })
    )
  }
  register(model: any)
  {
    return this.http.post<User>(this.baseUrl + 'account/register',model).pipe(
      map(user => {
        if(user){
          localStorage.setItem('user',JSON.stringify(user)); 
          this.currentSourceUser.next(user);
        }
      })
    )
  }

  setCurrentUser( user:User){
    this.currentSourceUser.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentSourceUser.next(null);
  }
}