import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Patient } from '../Models/patient';
import { AutoScheduleRequest, AutoScheduleResponse } from '../Models/Recetionistres';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
private apiUrl = 'http://localhost:5148/api/Reception'; // Update with your backend URL

  constructor(private http: HttpClient) { }

  addPatient(patient:Patient){
    return this.http.post(`${this.apiUrl}/AddPatient`, patient);
  }

  getAllPatients() {
    return this.http.get<Patient[]>(`${this.apiUrl}/GetAllPatients`);
  }
  autoSchedule(request: AutoScheduleRequest) {
    return this.http.post<AutoScheduleResponse>(`${this.apiUrl}/AutoSchedule`, request);
  }
}
