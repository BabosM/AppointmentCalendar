using AppointmentCalendar.Models;
using AppointmentCalendar.Models.ViewModels;
using AppointmentCalendar.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
