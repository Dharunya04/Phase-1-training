import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Availability, DoctorAppointment, getAvailability, GetDoctorres } from '../Models/Doctorres';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private apiUrl = "http://localhost:5148/api/Doctor";

  constructor(private http: HttpClient) {}

  // ✅ Centralized headers with JWT
  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  // ✅ Get logged-in doctor
  getLoggedDoctor() {
    return this.http.get<GetDoctorres>(
      `${this.apiUrl}/GetLoggedDoctor`,
      { headers: this.getAuthHeaders() }
    );
  }

  // ✅ Add availability
  addAvailability(dto: Availability) {
    return this.http.post(
      `${this.apiUrl}/availability`,
      dto,
      { headers: this.getAuthHeaders() }
    );
  }

  // ✅ Get all availabilities (fixed — added headers)
  getAllAvailabilities() {
    return this.http.get<getAvailability[]>(
      `${this.apiUrl}/GetAllAvailabilities`,
      { headers: this.getAuthHeaders() }
    );
  }

  // ✅ Update availability
  updateAvailability(id: number, dto: Availability) {
    return this.http.put(
      `${this.apiUrl}/UpdateAvailability/${id}`,
      dto,
      { headers: this.getAuthHeaders() }
    );
  }

  // ✅ Delete availability
  deleteAvailability(id: number) {
    return this.http.delete(
      `${this.apiUrl}/DeleteAvailability/${id}`,
      { headers: this.getAuthHeaders() }
    );
  }

  // ✅ Get doctor's appointments
  getMyAppointments() {
    return this.http.get<DoctorAppointment[]>(
      `${this.apiUrl}/my-appointments`,
      { headers: this.getAuthHeaders() }
    );
  }

 
  completeAppointment(appointmentId: number) {
  return this.http.delete<{ message: string }>(
    `${this.apiUrl}/complete-appointment/${appointmentId}`,
    {
      headers: this.getAuthHeaders()
    }
  );
}

}
