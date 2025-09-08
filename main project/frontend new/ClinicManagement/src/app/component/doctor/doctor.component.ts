import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DoctorService } from 'src/app/services/doctor.service';
import { DoctorAppointment, getAvailability, GetDoctorres } from 'src/app/Models/Doctorres';

@Component({
  selector: 'app-doctor',
  templateUrl: './doctor.component.html',
  styleUrls: ['./doctor.component.css']
})
export class DoctorComponent implements OnInit {
  doctor?: GetDoctorres;
  availabilityForm: FormGroup;
  message = '';
  availabilities: getAvailability[] = [];
  selectedDate?: string;
  timeSlots: { time: string, day: string, availability?: getAvailability, isPast: boolean }[] = [];
  currentMonth: number;
  currentYear: number;
  daysInMonth: Date[] = [];
  showCalendar: boolean = true;
  slotClicked?: string;
  appointments: any[] = [];
  activeTab: string = 'availability';

  constructor(private fb: FormBuilder, private doctorService: DoctorService) {
    this.availabilityForm = this.fb.group({
      date: ['', Validators.required],
      startTime: ['', Validators.required],
      endTime: ['', Validators.required]
    });

    const today = new Date();
    this.currentMonth = today.getMonth();
    this.currentYear = today.getFullYear();
  }

  ngOnInit(): void {
    this.loadLoggedDoctor();
    this.loadAvailabilities();
    this.generateDaysInMonth();
    this.loadAppointments();
  }

  setActiveTab(tab: string) {
    this.activeTab = tab;
  }

  loadLoggedDoctor() {
    this.doctorService.getLoggedDoctor().subscribe({
      next: (res) => this.doctor = res,
      error: () => this.message = 'Failed to load doctor info'
    });
  }

  loadAvailabilities() {
  this.doctorService.getAllAvailabilities().subscribe({
    next: (res: getAvailability[]) => {
      this.availabilities = res.filter((a, index, self) =>
        index === self.findIndex(s =>
          s.date === a.date &&
          s.startTime === a.startTime &&
          s.endTime === a.endTime
        )
      );
      this.generateTimeSlots();  // ✅ Refresh UI with booked status
    },
    error: () => console.error('Failed to load availabilities')
  });
}


  generateDaysInMonth() {
    this.daysInMonth = [];
    const totalDays = new Date(this.currentYear, this.currentMonth + 1, 0).getDate();
    for (let i = 1; i <= totalDays; i++) {
      this.daysInMonth.push(new Date(this.currentYear, this.currentMonth, i));
    }
  }

  getMonthName(month: number): string {
    return new Date(this.currentYear, month).toLocaleString('en-US', { month: 'long' });
  }

  isAvailable(date: Date): boolean {
    return this.availabilities.some(a => new Date(a.date).toDateString() === date.toDateString());
  }

  selectDate(date: Date) {
    this.selectedDate = date.toLocaleDateString('en-CA');
    this.generateTimeSlots();
    this.showCalendar = false;
    this.availabilityForm.patchValue({ date: this.selectedDate });
  }

  backToCalendar() {
    this.showCalendar = true;
    this.selectedDate = undefined;
    this.timeSlots = [];
  }

generateTimeSlots() {
  this.timeSlots = [];
  const startHour = 9;
  const endHour = 17;
  const selectedDayName = new Date(this.selectedDate!).toLocaleDateString('en-US', { weekday: 'long' });
  const today = new Date();

  const uniqueAvailabilities = this.availabilities.filter(
    (a, index, self) =>
      index === self.findIndex(
        s =>
          s.date === a.date &&
          s.startTime === a.startTime &&
          s.endTime === a.endTime
      )
  );

  for (let h = startHour; h < endHour; h++) {
    const start = `${h.toString().padStart(2, '0')}:00`;
    const end = `${(h + 1).toString().padStart(2, '0')}:00`;
    const timeStr = `${start} - ${end}`;

    const availability = uniqueAvailabilities.find(a =>
      a.date === this.selectedDate && a.startTime === start
    );

    const slotDateTime = new Date(`${this.selectedDate}T${start}`);
    const isPast = slotDateTime < today;

    this.timeSlots.push({
      time: timeStr,
      day: availability?.day && availability.day.trim() !== '' ? availability.day : selectedDayName,
      availability,
      isPast
    });
  }
}


  selectSlot(slot: { time: string, day: string }) {
    const [startTime, endTime] = slot.time.split(' - ');
    this.availabilityForm.patchValue({
      date: this.selectedDate,
      startTime,
      endTime
    });
    this.slotClicked = slot.time;
  }

onSubmit() {
  if (this.availabilityForm.invalid) {
    this.message = 'Please fill all required fields';
    return;
  }

  const dto = this.availabilityForm.value;
  const dateObj = new Date(dto.date);
  const daysOfWeek = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
  dto.day = daysOfWeek[dateObj.getDay()];

  this.doctorService.addAvailability(dto).subscribe({
    next: (res: any) => {
      this.message = res.message || 'Slot added successfully';
      this.availabilityForm.reset({ date: this.selectedDate });
      this.loadAvailabilities();
      this.generateTimeSlots();
      this.slotClicked = undefined;

      // Auto-hide message
      setTimeout(() => this.message = '', 3000);
    },
    error: (err) => {
      if (err.status === 409) {
        // Instead of showing "Slot already exists" error,
        // we simply mark it as booked
        this.message = 'This slot is already booked!';
        this.loadAvailabilities();   // ✅ Refresh booked slots
        this.generateTimeSlots();
      } else if (err.status === 401) {
        this.message = 'You are unauthorized. Please log in again.';
      } else {
        this.message = 'Failed to add slot. Please try again.';
      }

      setTimeout(() => this.message = '', 3000);
    }
  });
}



  deleteSlot(availabilityId: number) {
    if (confirm('Are you sure you want to delete this slot?')) {
      this.doctorService.deleteAvailability(availabilityId).subscribe({
        next: () => {
          this.availabilities = this.availabilities.filter(a => a.availabilityId !== availabilityId);
          this.generateTimeSlots();
          this.message = 'Slot deleted successfully';
          setTimeout(() => this.message = '', 3000);
        },
        error: () => {
          this.message = 'Failed to delete slot';
          setTimeout(() => this.message = '', 3000);
        }
      });
    }
  }

  editSlot(slot: getAvailability) {
    if (slot.isBooked) {
      alert('Cannot edit a booked slot!');
      return;
    }
    this.availabilityForm.patchValue({
      date: slot.date,
      startTime: slot.startTime,
      endTime: slot.endTime
    });
    this.slotClicked = `${slot.startTime} - ${slot.endTime}`;
  }

  loadAppointments() {
    this.doctorService.getMyAppointments().subscribe({
      next: (res) => {
        this.appointments = res;
        if (res.length === 0) {
          this.message = 'No appointments found';
        }
      },
      error: (err) => {
        console.error(err);
        this.message = err.error || 'Error fetching appointments';
      }
    });
  }

  markCompleted(appointmentId: number) {
    if (confirm('Are you sure you want to mark this appointment as completed?')) {
      this.doctorService.completeAppointment(appointmentId).subscribe({
        next: (res) => {
          alert(res.message);
          this.appointments = this.appointments.filter(a => a.appointmentId !== appointmentId);
        },
        error: () => alert('Error completing appointment')
      });
    }
  }

  logout() {
    localStorage.clear();
    window.location.reload();
  }
}
