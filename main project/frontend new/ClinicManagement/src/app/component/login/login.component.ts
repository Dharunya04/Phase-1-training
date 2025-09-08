import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from 'src/app/services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  message = '';

  constructor(private fb: FormBuilder, private loginservice: LoginService,private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }
onSubmit() {
  if (this.loginForm.valid) {
    const { username, password } = this.loginForm.value;

    this.loginservice.login(username, password).subscribe({
      next: (res: any) => { 
        this.message = "User logged in successfully";

       if (res.role?.trim().toLowerCase() === 'admin') {
  this.router.navigate(['/admin']);
}
if (res.role?.trim().toLowerCase() === 'doctor') {
  this.router.navigate(['/doctor']);
}
if (res.role?.trim().toLowerCase() === 'reception') {
  this.router.navigate(['/reception']);
}
      },
      error: (err) => {
        console.error('API Error:', err);
        this.message = 'Invalid username or password';
      }
    });
  }
}
}

