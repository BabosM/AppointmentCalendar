using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentCalendar.Utility
{
    public static class Helper
    {
        public static string Admin = "Admin";
        public static string Doctor = "Doctor";
        public static string Patient = "Patient";
        public static string appointmentAdded = "Appointment added successfully.";
        public static string appointmentUpdated = "Appointment updated successfully.";
        public static string appointmentDeleted = "Appointment deleted successfully.";
        public static string appointmentExists = "Appointment for selected date and time already exists.";
        public static string appointmentNotExists = "Appointment not exists.";
        public static string meetingConfirm = "Meeting confirm successfully.";
        public static string meetingConfirmError = "Error while confirming meeting.";
        public static string appointmentAddError = "Something went wront, Please try again.";
        public static string appointmentUpdatError = "Something went wront, Please try again.";
        public static string somethingWentWrong = "Something went wront, Please try again.";
        public static int success_code = 1;
        public static int failure_code = 0;
        public static List<SelectListItem> GetRolesForDropDown( bool isAdmin)
        {
            if (isAdmin)
            {
                return new List<SelectListItem>{
                new SelectListItem{Value=Helper.Admin, Text=Helper.Admin }
                };
            }
            else { 
            
            }
          return new List<SelectListItem>{
                new SelectListItem{Value=Helper.Doctor, Text=Helper.Doctor},
                new SelectListItem{Value=Helper.Patient, Text=Helper.Patient}
            };      
        }
        public static List<SelectListItem> getTimeDopDown()
        {
            var minutes = 60;
            List<SelectListItem> duration = new List<SelectListItem>();

            for (int i = 0; i < 4; i++) {
                if (i == 0) {
                    duration.Add(new SelectListItem { Value = "30", Text = "30min" });
                }else
                {
                    duration.Add(new SelectListItem { Value = minutes.ToString(), Text = i + "h" });
                    minutes = +30;
                    duration.Add(new SelectListItem { Value = minutes.ToString(), Text = i + "h30min" });
                    minutes = +30;
                }
               
            }

            return duration;
        }
    }
}
