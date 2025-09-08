export interface ReceptionistRequest {
  receptionistID: number;
  receptionistName: string;
  email: string;
  phoneNumber: string;
  userName: string;
  password: string;
  roleName: string;
}


export interface ReceptionRes {
  receptionistID: number;
  receptionistName: string;
  phoneNumber: string;
  email: string;
  userName: string;
}

export interface AutoScheduleRequest {
  patientID: number;
  doctorID: number; 
  reasonForVisit: string;
  availabilityId?: number;
  date:string;
   startTime:string;
   endTime:string;

}
export interface AutoScheduleResponse {
  message: string;
  
}
