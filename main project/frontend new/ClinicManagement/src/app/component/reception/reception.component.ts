import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Patient } from 'src/app/Models/patient';
import { AutoScheduleRequest, AutoScheduleResponse } from 'src/app/Models/Recetionistres';
import { DoctorService } from 'src/app/services/doctor.service';
import { PatientService } from 'src/app/services/patient.service';

@Component({
  selector: 'app-reception',
  templateUrl: './reception.component.html',
  styleUrls: ['./reception.component.css']
})
export class ReceptionComponent implements OnInit {
  patientForm: FormGroup;
  appointmentForm: FormGroup;
  successMessage: string = '';
  errorMessage: string = '';
  message: string = '';
  patients: Patient[] = [];
  activeForm: 'addPatient' | 'viewPatients' = 'addPatient';

  selectedPatientId!: number | null;
  doctorAvailabilities: any[] = [];
  uniqueDoctors: any[] = [];
  selectedDoctorId: number | null = null;
  filteredSlots: any[] = [];
  selectedSlot: any = null;

  constructor(
    private fb: FormBuilder,
    private patientService: PatientService,
    private doctorService: DoctorService
  ) {
    this.patientForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      gender: ['', Validators.required],
      phoneNumber: [''],
      email: ['', [Validators.email]],
      address: ['']
    });

    this.appointmentForm = this.fb.group({
      reasonForVisit: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadPatients();
    this.loadDoctorAvailabilities();
  }

  private getUniqueDoctors(data: any[]): any[] {
    const map = new Map();
    data.forEach(item => {
      if (!map.has(item.doctorId)) {
        map.set(item.doctorId, {
          doctorId: item.doctorId,
          doctorName: item.doctorName,
          specialization: item.specialization
        });
      }
    });
    return Array.from(map.values());
  }

  showForm(formType: 'addPatient' | 'viewPatients') {
    this.activeForm = formType;
    this.message = '';
    this.selectedPatientId = null;
    this.selectedDoctorId = null;
    this.selectedSlot = null;
    if (formType === 'viewPatients') this.loadPatients();
  }

  onSubmit() {
    if (this.patientForm.invalid) {
      this.errorMessage = 'Please fill out all required fields correctly.';
      return;
    }

    this.patientService.addPatient(this.patientForm.value).subscribe({
      next: () => {
        this.successMessage = 'Patient added successfully!';
        this.errorMessage = '';
        this.patientForm.reset();
        this.loadPatients();
      },
      error: (err) => {
        console.error('Error adding patient:', err);
        this.errorMessage = 'Failed to add patient. Please try again.';
      }
    });
  }

  loadPatients() {
    this.patientService.getAllPatients().subscribe({
      next: (data) => this.patients = data,
      error: (err) => {
        console.error('Error fetching patients:', err);
        this.message = 'Failed to fetch patients';
      }
    });
  }

  loadDoctorAvailabilities() {
    this.doctorService.getAllAvailabilities().subscribe({
      next: (data) => {
        this.doctorAvailabilities = data;
        this.uniqueDoctors = this.getUniqueDoctors(data);
      },
      error: (err) => console.error('Error fetching doctor availability:', err)
    });
  }

  showDoctorAvailability(patientID: number) {
    this.selectedPatientId = patientID;
    this.selectedDoctorId = null;
    this.selectedSlot = null;
    this.message = '';
    this.appointmentForm.reset();
  }

  selectDoctor(doctorId: number) {
    this.selectedDoctorId = doctorId;
    this.selectedSlot = null;

    this.filteredSlots = this.doctorAvailabilities
      .filter(d => d.doctorId === doctorId && new Date(d.date).getFullYear() > 1)
      .filter((value, index, self) =>
        index === self.findIndex(s => s.date === value.date && s.startTime === value.startTime)
      )
      .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
  }

  selectSlot(slot: any) {
    if (!slot.isBooked) {
      this.selectedSlot = slot;
    }
  }

  bookAppointment() {
    if (!this.selectedPatientId || !this.selectedDoctorId || !this.selectedSlot || this.appointmentForm.invalid) {
      return;
    }

    const request: AutoScheduleRequest = {
      patientID: this.selectedPatientId,
      doctorID: this.selectedDoctorId,
      reasonForVisit: this.appointmentForm.value.reasonForVisit,
      date: this.selectedSlot.date,
      startTime: this.selectedSlot.startTime,
      endTime: this.selectedSlot.endTime
    };

    this.patientService.autoSchedule(request).subscribe({
      next: (res: AutoScheduleResponse) => {
        this.message = res.message || 'Appointment booked successfully!';
        this.selectedPatientId = null;
        this.selectedDoctorId = null;
        this.selectedSlot = null;
        this.appointmentForm.reset();
        this.loadDoctorAvailabilities();
      },
      error: (err) => {
        console.error('Error booking appointment:', err);
        this.message = err.error?.message || 'Failed to book appointment.';
      }
    });
  }
}
