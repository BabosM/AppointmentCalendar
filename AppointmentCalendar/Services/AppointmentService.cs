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
            var startDate = DateTime.ParseExact(appointmentViewModel.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
           // var startDate = DateTime.Parse(appointmentViewModel.StartDate);
            var endDate = DateTime.Parse(appointmentViewModel.EndDate).AddMinutes(Convert.ToDouble(appointmentViewModel.Duration));
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
    }
}
