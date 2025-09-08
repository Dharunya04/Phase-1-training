import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Doctorres, GetDoctorres } from '../Models/Doctorres';
import { ReceptionistRequest, ReceptionRes } from '../Models/Recetionistres';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = "http://localhost:5148/api/Admin";

  constructor(private http: HttpClient) { }

  registerDoctor(userData: Doctorres) {
    const user_add_url = `${this.apiUrl}/Doctor`;
    return this.http.post<Doctorres>(user_add_url, userData);
  }

  registerReceptionist(userData: ReceptionistRequest) {
    const user_add_url = `${this.apiUrl}/Reception`;
    return this.http.post<ReceptionistRequest>(user_add_url, userData);
  }

  getDoctors() {
    const get_doctors_url = `${this.apiUrl}/GetAllDoctor`;
    return this.http.get<GetDoctorres[]>(get_doctors_url);
  }
  getReception() {
    const get_url = `${this.apiUrl}/GetAllReceptionist`;
    return this.http.get<ReceptionRes[]>(get_url);
  }

  updateDoctor(doctorId: number, doctorData: Doctorres) {
    const update_url = `${this.apiUrl}/UpdateDoctor/${doctorId}`;
    return this.http.put(update_url, doctorData);
  }

  deleteDoctor(doctorId: number) {
    const delete_url = `${this.apiUrl}/DeleteDoctor/${doctorId}`;
    return this.http.delete(delete_url);
  }
  updateReceptionist(receptionistId: number, receptionistData: ReceptionistRequest) {
    const update_url = `${this.apiUrl}/UpdateReceptionist/${receptionistId}`;
    return this.http.put(update_url, receptionistData);
  }

  deleteReceptionist(receptionistId: number) {
    const delete_url = `${this.apiUrl}/DeleteReceptionist/${receptionistId}`;
    return this.http.delete(delete_url);
  }
}
