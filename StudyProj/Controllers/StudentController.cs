using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using StudyProj.Repositories.Implementations;
namespace StudyProj.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private StudentRepository Students { get; set; }


        public StudentController(StudentRepository Student)
        {
            Students = Student;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()

        {
            return new JsonResult(await Students.GetAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] Student student)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return new JsonResult(await Students.GetAllAsync(student));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetStudent(int id)
        {
            var Student = await Students.GetAsync(id);

            if (Student == null)
            {
                return NotFound();
            }

            return new JsonResult(Student);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Student Student)
        {
            bool success = true;
            Student st = null;

            try
            {
                st = await Students.CreateAsync(Student);
            }
            catch (Exception)
            {
                success = false;
            }

            return success ? new JsonResult($"Created successfully with ID: {st.Id}") : new JsonResult("Creation failed");
        }
        [HttpPut]
        public async Task<IActionResult> Put(Student Student)
        {
            bool success = true;
            var st = await Students.GetAsync(Student.Id);
            try
            {
                if (st != null)
                {
                    st = await Students.UpdateAsync(Student);
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

            return success ? new JsonResult($"Update successful for Student with ID: {st.Id}") : new JsonResult("Update was not successful");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = true;
            var Student = await Students.GetAsync(id);

            try
            {
                if (Student != null)
                {
                    await Students.DeleteAsync(Student.Id);
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
