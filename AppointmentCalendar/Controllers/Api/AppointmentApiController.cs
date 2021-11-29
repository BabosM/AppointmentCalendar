using AppointmentCalendar.Models.ViewModels;
using AppointmentCalendar.Services;
using AppointmentCalendar.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppointmentCalendar.Controllers.Api
{
    [ApiController]
    [Route("api/Appointment")]
    public class AppointmentApiController : Controller
    {
        public readonly IAppointmentService _appointmentService;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly string loginUserId;
        public readonly string role;

        public AppointmentApiController(IAppointmentService appointmentService, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpPost]
        [Route("SaveCalendarData")]
        public IActionResult SaveCalendarData(AppointmentViewModel viewModel)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = _appointmentService.AddUpdate(viewModel).Result;

                if (commonResponse.status == 1)
                {
                    commonResponse.message = Helper.appointmentAdded;
                }
                if (commonResponse.status == 2)
                {
                    commonResponse.message = Helper.appointmentUpdated;
                }

            }
            catch (Exception ex) {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failure_code;
            }
           return View();
        }
        [HttpGet]
        [Route("GetCalendarData")]
        public IActionResult GetCalendarData(string doctorId)
        {
            CommonResponse<List<AppointmentViewModel>> commonResponse = new CommonResponse<List<AppointmentViewModel>>();

            try
            {
                if (role == Helper.Patient)
                {
                    commonResponse.dataEnum = _appointmentService.PatientsEventsById(loginUserId);
                    commonResponse.status = Helper.success_code;
                }
                else if (role == Helper.Doctor)
                {
                    commonResponse.dataEnum = _appointmentService.DoctorEventsById(loginUserId);
                    commonResponse.status = Helper.success_code;
                }
                else {
                    commonResponse.dataEnum = _appointmentService.DoctorEventsById(doctorId);
                    commonResponse.status = Helper.success_code;
                }
                
            }
            catch(Exception ex)
            {
                commonResponse.message = ex.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
        [HttpGet]
        [Route("GetCalendarDataById/{id}")]
        public IActionResult GetCalendarDataById(int id)
        {
            CommonResponse<AppointmentViewModel> commonResponse = new CommonResponse<AppointmentViewModel>();
            try
            {

                commonResponse.dataEnum = _appointmentService.GetById(id);
                commonResponse.status = Helper.success_code;

            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
    }
}
