import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RegisterService } from 'src/app/services/register.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  message = '';
  isSuccess = false;
  roles: string[] = ['Admin', 'Doctor', 'Patient'];

  constructor(private fb: FormBuilder, private registerService: RegisterService) {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      roleName: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      this.registerService.register(this.registerForm.value).subscribe({
        next: (res: any) => {
          console.log('API Response:', res); 
          this.message = res.message;
          this.isSuccess = true;
          this.registerForm.reset();
        },
        error: (err) => {
          console.error('API Error:', err);
          this.message = err.error?.message || 'Registration failed';
          this.isSuccess = false;
        }
      });
    }
  }
}
