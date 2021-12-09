using AppointmentCalendar.Models;
using AppointmentCalendar.Models.ViewModels;
using AppointmentCalendar.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentCalendar.Services
{
    public class AppointmentService : IAppointmentService
    {
       
        private readonly ApplicationDbContext _db;

        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<int> AddUpdate(AppointmentVM appointmentViewModel)
         {
            var startDate = DateTime.ParseExact(appointmentViewModel.StartDate, "MM/d/yyyy h:mm tt", CultureInfo.InvariantCulture);
            var endDate = startDate.AddMinutes(Convert.ToDouble(appointmentViewModel.Duration));
            if (appointmentViewModel != null && appointmentViewModel.Id > 0)
            {
                var appointment = _db.Appointments.FirstOrDefault(x => x.Id == appointmentViewModel.Id);

                appointment.Title = appointmentViewModel.Title;
                appointment.Description = appointmentViewModel.Description;
                appointment.StartDate = startDate;
                appointment.EndDate = endDate;
                appointment.Duration = appointmentViewModel.Duration;
                appointment.DoctorId = appointmentViewModel.DoctorId;
                appointment.PatientId = appointmentViewModel.PatientId;
                appointment.IsDoctorApproved = false;
                appointment.AdminId = appointmentViewModel.AdminId;
                await _db.SaveChangesAsync();
                return 1;
            }
            else {
                // add 
                Appointment appointment = new Appointment()
                {
                    Title = appointmentViewModel.Title,
                    Description = appointmentViewModel.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = appointmentViewModel.Duration,
                    DoctorId = appointmentViewModel.DoctorId,
                    PatientId = appointmentViewModel.PatientId,
                    IsDoctorApproved = false,
                    AdminId = appointmentViewModel.AdminId
                };
                _db.Appointments.Add(appointment);
                await  _db.SaveChangesAsync();
                return 2;
            }
        }

        public async Task<int> ConfirmEvent(int id)
        {
            var appointment = _db.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null) {
                appointment.IsDoctorApproved = true;
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> Delete(int id)
        {
            var appointment = _db.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                _db.Appointments.Remove(appointment);
                return await _db.SaveChangesAsync();
                
            }
            return 0;
        }

        public List<AppointmentVM> DoctorsEventsById(string doctorId)
        {
            return _db.Appointments.Where(x => x.DoctorId == doctorId).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved
            }).ToList();
        }

      public AppointmentVM GetById(int id)
        {
            return _db.Appointments.Where(x => x.Id == id).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved,
                PatientId = c.PatientId,
                DoctorId = c.DoctorId,
                PatientName = _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(),
                DoctorName = _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault(),
            }).SingleOrDefault();
        }

        public List<DoctorVM> GetDoctorList()
        {

            // do poprawy. Póki co zwracam tylko wszystkich userów
            var DoctorsVM = (from user in _db.Users
                             join userRole in _db.UserRoles on user.Id  equals userRole.UserId
                             join role in _db.Roles.Where(x => x.Name == Helper.Doctor) on userRole.RoleId equals role.Id
                             select new DoctorVM
                             {
                                 Id = user.Id,
                                 Name = user.Name
                             }).ToList();
            
            return DoctorsVM;
        }

        public List<PatientVM> GetPatientList()
        {
            var PatientVM = (from user in _db.Users
                             join userRole in _db.UserRoles on user.Id equals userRole.UserId
                             join role in _db.Roles.Where(x => x.Name == Helper.Patient) on userRole.RoleId equals role.Id
                             select new PatientVM
                             {
                                 Id = user.Id,
                                 Name = user.Name
                             }).ToList();
            return PatientVM;
        }

        public List<AppointmentVM> PatientsEventsById(string patientId)
        {
            return _db.Appointments.Where(x => x.PatientId == patientId).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved
            }).ToList();
        }
    }
}
