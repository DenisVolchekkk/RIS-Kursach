using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using StudyProj.Repositories.Implementations;
namespace StudyProj.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DisciplineController : ControllerBase
    {
        private DisciplineRepository Disciplines { get; set; }


        public DisciplineController(DisciplineRepository Discipline)
        {
            Disciplines = Discipline;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()

        {
            return new JsonResult(await Disciplines.GetAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] Discipline dis)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return new JsonResult(await Disciplines.GetAllAsync(dis));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetDiscipline(int id)
        {
            var Discipline = await Disciplines.GetAsync(id);

            if (Discipline == null)
            {
                return NotFound();
            }

            return new JsonResult(Discipline);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Discipline Discipline)
        {
            bool success = true;
            Discipline dis = null;

            try
            {
                dis = await Disciplines.CreateAsync(Discipline);
            }
            catch (Exception)
            {
                success = false;
            }

            return success ? new JsonResult($"Created successfully with ID: {dis.Id}") : new JsonResult("Creation failed");
        }
        [HttpPut]
        public async Task<IActionResult> Put(Discipline Discipline)
        {
            bool success = true;
            var dis = await Disciplines.GetAsync(Discipline.Id);
            try
            {
                if (dis != null)
                {
                    dis = await Disciplines.UpdateAsync(Discipline);
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

            return success ? new JsonResult($"Update successful for Discipline with ID: {dis.Id}") : new JsonResult("Update was not successful");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = true;
            var Discipline = await Disciplines.GetAsync(id);

            try
            {
                if (Discipline != null)
                {
                    await Disciplines.DeleteAsync(Discipline.Id);
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
