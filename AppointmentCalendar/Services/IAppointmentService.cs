using AppointmentCalendar.Models.ViewModels;
using System.Collections.Generic;

namespace AppointmentCalendar.Services
{
    public interface IAppointmentService
    {
        List<DoctorVM> GetDoctorList();

        List<PatientVM> GetPatientList();

    }
}