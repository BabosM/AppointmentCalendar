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

        public static List<SelectListItem> GetRolesForDropDown()
        {
          return new List<SelectListItem>{
                new SelectListItem{Value=Helper.Admin, Text=Helper.Admin },
                new SelectListItem{Value=Helper.Doctor, Text=Helper.Doctor},
                new SelectListItem{Value=Helper.Patient, Text=Helper.Patient}
            };      
        }
        public static List<SelectListItem> getTimeDopDown()
        {
            var minutes = 60;
            List<SelectListItem> duration = new List<SelectListItem>();

            for (int i = 0; i < 12; i++) {
                duration.Add(new SelectListItem { Value = minutes.ToString(), Text = i + " Hr" });
                minutes = +30;
                duration.Add(new SelectListItem{ Value = minutes.ToString(), Text = i + "Hr 30min"});
                minutes = +30;
            }

            return duration;
        }
    }
}
