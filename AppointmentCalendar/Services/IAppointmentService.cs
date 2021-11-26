using AppointmentCalendar.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentCalendar.Services
{
    public interface IAppointmentService
    {
        List<DoctorVM> GetDoctorList();

        List<PatientVM> GetPatientList();
        public Task<int> AddUpdate(AppointmentViewModel appointmentViewModel);
    }
}