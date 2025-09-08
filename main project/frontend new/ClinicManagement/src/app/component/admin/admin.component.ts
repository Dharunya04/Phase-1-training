import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Doctorres, GetDoctorres } from 'src/app/Models/Doctorres';
import { ReceptionistRequest, ReceptionRes } from 'src/app/Models/Recetionistres';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  doctorForm: FormGroup;
  receptionistForm: FormGroup;
  editDoctorForm: FormGroup;
  editReceptionistForm: FormGroup;

  message: string = '';
  activeForm: 'admin' | 'doctor' | 'receptionist' | 'viewDoctors' | 'viewReceptionist' | 'editDoctor' | 'editReceptionist' = 'admin';

  doctors: GetDoctorres[] = [];
  receptionist: ReceptionRes[] = [];
  selectedDoctorId: number | null = null;
  selectedReceptionistId: number | null = null;

  constructor(private fb: FormBuilder, private adminService: AdminService) {
    // Doctor Registration Form
    this.doctorForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required],
      roleName: ['Doctor', Validators.required],
      doctorName: ['', Validators.required],
      age: [null, [Validators.required, Validators.min(25)]],
      specialization: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      consultantFee: [0, [Validators.required, Validators.min(0)]]
    });

    // Receptionist Registration Form
    this.receptionistForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      roleName: ['Reception', Validators.required],
      receptionistName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]]
    });

    // Edit Doctor Form
    this.editDoctorForm = this.fb.group({
      doctorName: ['', Validators.required],
      age: [null, [Validators.required, Validators.min(25)]],
      specialization: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      consultantFee: [0, [Validators.required, Validators.min(0)]]
    });

    this.editReceptionistForm = this.fb.group({
  receptionistID: [null, Validators.required],
  userName: ['', Validators.required], 
  password: [''],  
  receptionistName: ['', Validators.required],
  email: ['', [Validators.required, Validators.email]],
  phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
  roleName: ['Reception', Validators.required]
});


  }

  ngOnInit(): void {
    this.loadDoctors();
    this.loadReceptionist();
  }

  // Show specific form
  showForm(formType: any) {
    this.activeForm = formType;
    this.message = '';
  }

  // -------------------- Doctor CRUD ------------------------
  onSubmit() {
    if (this.doctorForm.valid) {
      const doctor: Doctorres = this.doctorForm.value;
      this.adminService.registerDoctor(doctor).subscribe({
        next: () => {
          this.message = 'Doctor registered successfully!';
          this.doctorForm.reset({ roleName: 'Doctor' });
          this.loadDoctors();
        },
        error: () => {
          this.message = 'Error registering doctor.';
        }
      });
    } else {
      this.message = 'Please fill all required fields correctly.';
    }
  }

  loadDoctors() {
    this.adminService.getDoctors().subscribe({
      next: (data) => (this.doctors = data),
      error: (err) => console.error('Error fetching doctors:', err)
    });
  }

  editDoctor(doc: GetDoctorres) {
    this.selectedDoctorId = doc.doctorID;
    this.editDoctorForm.patchValue(doc);
    this.showForm('editDoctor');
  }

  updateDoctor() {
    if (this.editDoctorForm.valid && this.selectedDoctorId) {
      this.adminService.updateDoctor(this.selectedDoctorId, this.editDoctorForm.value).subscribe({
        next: () => {
          alert('Doctor updated successfully!');
          this.showForm('viewDoctors');
          this.loadDoctors();
        },
        error: (err) => alert('Error updating doctor: ' + err.message)
      });
    }
  }

  deleteDoctor(id: number) {
    if (confirm('Are you sure you want to delete this doctor?')) {
      this.adminService.deleteDoctor(id).subscribe({
        next: () => {
          alert('Doctor deleted successfully!');
          this.loadDoctors();
        },
        error: (err) => alert('Error deleting doctor: ' + err.message)
      });
    }
  }

  // -------------------- Receptionist CRUD ------------------------
  onClick() {
    if (this.receptionistForm.invalid) {
      alert('Please fill all fields correctly.');
      return;
    }

    const formData: ReceptionistRequest = this.receptionistForm.value;
    this.adminService.registerReceptionist(formData).subscribe({
      next: () => {
        alert('Receptionist registered successfully!');
        this.receptionistForm.reset({ roleName: 'Reception' });
        this.loadReceptionist();
      },
      error: () => alert('Failed to register receptionist.')
    });
  }

  loadReceptionist() {
    this.adminService.getReception().subscribe({
      next: (data) => (this.receptionist = data),
      error: (err) => console.error('Error fetching receptionist:', err)
    });
  }

  editReceptionist(rec: ReceptionRes) {
  this.selectedReceptionistId = rec.receptionistID;

  this.editReceptionistForm.patchValue({
    receptionistID: rec.receptionistID,
    userName: rec.userName || '',
    receptionistName: rec.receptionistName,
    email: rec.email,
    phoneNumber: rec.phoneNumber,
    roleName: 'Reception'
  });

  this.showForm('editReceptionist');
}

updateReceptionist() {
  if (this.editReceptionistForm.valid && this.selectedReceptionistId) {
    const formValue = { ...this.editReceptionistForm.value };

    // If password is empty, remove it from payload
    if (!formValue.password) {
      delete formValue.password;
    }

    this.adminService.updateReceptionist(this.selectedReceptionistId, formValue).subscribe({
      next: () => {
        alert('Receptionist updated successfully!');
        this.showForm('viewReceptionist');
        this.loadReceptionist();
      },
      error: (err) => alert('Error updating receptionist: ' + err.message)
    });
  }
}


  deleteReceptionist(id: number) {
    if (confirm('Are you sure you want to delete this receptionist?')) {
      this.adminService.deleteReceptionist(id).subscribe({
        next: () => {
          alert('Receptionist deleted successfully!');
          this.loadReceptionist();
        },
        error: (err) => alert('Error deleting receptionist: ' + err.message)
      });
    }
  }
}
