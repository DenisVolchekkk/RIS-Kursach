using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using StudyProj.Repositories.Implementations;
using StudyProj.Repositories.Interfaces;
using System.Text.RegularExpressions;

namespace StudyProj.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private AttendanceRepository Attendances { get; set; }


        public AttendanceController(AttendanceRepository Attendance)
        {
            Attendances = Attendance;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()

        {
            return new JsonResult(await Attendances.GetAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] Attendance attendance)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return new JsonResult(await Attendances.GetAllAsync(attendance));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAttendance(int id)
        {
            var Attendance = await Attendances.GetAsync(id);

            if (Attendance == null)
            {
                return NotFound();
            }

            return new JsonResult(Attendance);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Attendance Attendance)
        {
            bool success = true;
            Attendance att = null;

            try
            {
                att = await Attendances.CreateAsync(Attendance);
            }
            catch (Exception)
            {
                success = false;
            }

            return success ? new JsonResult($"Created successfully with ID: {att.Id}") : new JsonResult("Creation failed");
        }
        [HttpPut]
        public async Task<IActionResult> Put(Attendance Attendance)
        {
            bool success = true;
            var att = await Attendances.GetAsync(Attendance.Id);
            try
            {
                if (att != null)
                {
                    att = await Attendances.UpdateAsync(Attendance);
                }
                else
                {
                    success = false;
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success ? new JsonResult($"Update successful for Attendance with ID: {att.Id}") : new JsonResult("Update was not successful");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = true;
            var Attendance = await Attendances.GetAsync(id);

            try
            {
                if (Attendance != null)
                {
                    await Attendances.DeleteAsync(Attendance.Id);
                }
                else
                {
                    success = false;
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success ? new JsonResult("Delete successful") : new JsonResult("Delete was not successful");
        }
    }
}
