import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Login } from '../Models/Login.interface';
import { tap } from 'rxjs';
interface Loginresponse{
  token: string;
  role: string;
}
@Injectable({
  providedIn: 'root'
})

export class LoginService {
private apiUrl = "http://localhost:5148/user";
  constructor(private http: HttpClient) { }

  
  
  login(username: string, password: string) {
  const loginData = {
    name: username,
    password: password
  };

  return this.http.post<Loginresponse>(`${this.apiUrl}/login`, loginData)
    .pipe(
      tap((res: Loginresponse) => {
      console.log(`Token received: ${res.token}`);
      localStorage.setItem('token', res.token);
      })
    );
}

}
  

