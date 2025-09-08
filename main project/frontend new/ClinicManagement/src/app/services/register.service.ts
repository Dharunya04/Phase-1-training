import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterRequest, RegisterResponse } from '../Models/RegisterResponse';

@Injectable({
  providedIn: 'root'
})

export class RegisterService {
  private apiUrl = "http://localhost:5148/user";
    constructor(private http: HttpClient) { }

register(userData:RegisterRequest){
     
    const user_add_url=`${this.apiUrl}/register`
    return this.http.post<RegisterResponse>(user_add_url,userData)
    
  }
 
}
