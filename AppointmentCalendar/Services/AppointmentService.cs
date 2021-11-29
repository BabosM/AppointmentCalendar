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

        public async Task<int> AddUpdate(AppointmentViewModel appointmentViewModel)
        {
            var startDate = DateTime.ParseExact(appointmentViewModel.StartDate, "MM/d/yyyy h:mm tt", CultureInfo.InvariantCulture);
            var endDate = startDate.AddMinutes(Convert.ToDouble(appointmentViewModel.Duration));
            if (appointmentViewModel != null && appointmentViewModel.Id > 0)
            {
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

        public List<AppointmentViewModel> DoctorEventsById(string doctorId)
        {
            return _db.Appointments.Where(x => x.DoctorId == doctorId).ToList().Select(c => new AppointmentViewModel()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("MM/d/yyyy h:mm tt"),
                EndDate = c.EndDate.ToString("MM/d/yyyy h:mm tt"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved
            }).ToList();
        }

        public AppointmentViewModel GetById(int id)
        {
           
                return _db.Appointments.Where(x => x.Id == id).ToList().Select(c => new AppointmentViewModel()
                {
                    Id = c.Id,
                    Description = c.Description,
                    StartDate = c.StartDate.ToString("MM/d/yyyy h:mm tt"),
                    EndDate = c.EndDate.ToString("MM/d/yyyy h:mm tt"),
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

        public List<AppointmentViewModel> PatientsEventsById(string patientId)
        {
            return _db.Appointments.Where(x => x.PatientId == patientId).ToList().Select(c => new AppointmentViewModel()
            {
                Id = c.Id,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy,MM,dd h:mm::ss tt"),
                EndDate = c.EndDate.ToString("yyyy,MM,dd h:mm::ss tt"),
                Title = c.Title,
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved
            }).ToList();
        }
    }
}
