using AppointmentCalendar.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentCalendar.Controllers
{
    public class AppointmentController : Controller
    {
        IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
           _appointmentService = appointmentService;
        }

        public IActionResult Index()
        {
            ViewBag.DoctorList = _appointmentService.GetDoctorList();
            return View();
        }
    }
}
