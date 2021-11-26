using Microsoft.EntityFrameworkCore.Migrations;

namespace AppointmentCalendar.Migrations
{
    public partial class AddAppointmentToDbUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StarDate",
                table: "Appointments",
                newName: "StartDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Appointments",
                newName: "StarDate");
        }
    }
}
