using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using StudyProj.Repositories.Implementations;
using StudyProj.Repositories.Interfaces;

namespace StudyProj.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChiefController : ControllerBase
    {
        private ChiefRepository Chiefs { get; set; }


        public ChiefController(ChiefRepository Chief)
        {
            Chiefs = Chief;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()

        {
            return new JsonResult(await Chiefs.GetAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] Chief chief)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return new JsonResult(await Chiefs.GetAllAsync(chief));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetChief(int id)
        {
            var Chief = await Chiefs.GetAsync(id);

            if (Chief == null)
            {
                return NotFound();
            }

            return new JsonResult(Chief);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Chief Chief)
        {
            bool success = true;
            Chief chief = null;

            try
            {
                chief = await Chiefs.CreateAsync(Chief);
            }
            catch (Exception)
            {
                success = false;
            }

            return success ? new JsonResult($"Created successfully with ID: {chief.Id}") : new JsonResult("Creation failed");
        }
        [HttpPut]
        public async Task<IActionResult> Put(Chief Chief)
        {
            bool success = true;
            var chief = await Chiefs.GetAsync(Chief.Id);
            try
            {
                if (chief != null)
                {
                    chief = await Chiefs.UpdateAsync(Chief);
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

            return success ? new JsonResult($"Update successful for Chief with ID: {Chief.Id}") : new JsonResult("Update was not successful");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = true;
            var chief = await Chiefs.GetAsync(id);

            try
            {
                if (chief != null)
                {
                    await Chiefs.DeleteAsync(chief.Id);
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
