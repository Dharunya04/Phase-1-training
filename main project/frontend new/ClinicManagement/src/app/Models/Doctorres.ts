export interface Doctorres {
userName: string;
  password: string;
  roleName: string;
  doctorName: string;
  age: number;
  specialization: string;
  email: string;
  phoneNumber: string;
  consultantFee: number;
  
}

export interface GetDoctorres {
  doctorID: number;
  doctorName: string;
  age: number;
  specialization: string;
  email: string;
  phoneNumber: string;
  consultantFee:number;
  
  
}
export interface Availability {
  day: string;
  date:string;
  startTime: string;  
  endTime: string;    
 
}

export interface getAvailability {
  availabilityId: number;
  doctorId: number;
  doctorName: string;
  day: string;
  date: string; 
  startTime: string;
  endTime: string;
  isBooked:boolean;
}

export interface DoctorAppointment {
  appointmentId: number;
  patientName: string;
  appointmentDate: string;
  appointmentStatus: string;
  reasonForVisit: string;
}