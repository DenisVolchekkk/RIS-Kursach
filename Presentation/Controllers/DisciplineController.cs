using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain.ViewModel;

namespace Presentation.Controllers
{
    public class DisciplineController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7112/api");
        private readonly HttpClient _client;

        public DisciplineController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index(string searchString, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
            IQueryable<Discipline> DisciplineList = null;

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Discipline/Filter?Name=" + searchString).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                DisciplineList = JsonConvert.DeserializeObject<List<Discipline>>(data).AsQueryable();
            }
            int pageSize = 20;

            return View(PaginatedList<Discipline>.Create(DisciplineList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(DisciplineViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Discipline/Post", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Discipline created.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                DisciplineViewModel Discipline = new DisciplineViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Discipline/GetDiscipline/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Discipline = JsonConvert.DeserializeObject<DisciplineViewModel>(data);
                }
                return View(Discipline);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit(DisciplineViewModel Discipline)
        {
            string data = JsonConvert.SerializeObject(Discipline);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Discipline/Put", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Discipline updated.";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                DisciplineViewModel Discipline = new DisciplineViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Discipline/GetDiscipline/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Discipline = JsonConvert.DeserializeObject<DisciplineViewModel>(data);
                }
                return View(Discipline);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Discipline/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Discipline deleted.";
                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }

    }
}
