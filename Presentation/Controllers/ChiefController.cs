using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain.ViewModel;
namespace Presentation.Controllers
{
    public class ChiefController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7112/api");
        private readonly HttpClient _client;

        public ChiefController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index(string searchString, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
            IQueryable<Chief> ChiefList = null;

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Chief/Filter?Name=" + searchString).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                ChiefList = JsonConvert.DeserializeObject<List<Chief>>(data).AsQueryable();
            }
            int pageSize = 20;

            return View(PaginatedList<Chief>.Create(ChiefList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(ChiefViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Chief/Post", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Chief created.";
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
                ChiefViewModel Chief = new ChiefViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Chief/GetChief/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Chief = JsonConvert.DeserializeObject<ChiefViewModel>(data);
                }
                return View(Chief);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit(ChiefViewModel Chief)
        {
            string data = JsonConvert.SerializeObject(Chief);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Chief/Put", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Chief updated.";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                ChiefViewModel Chief = new ChiefViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Chief/GetChief/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Chief = JsonConvert.DeserializeObject<ChiefViewModel>(data);
                }
                return View(Chief);
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
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Chief/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Chief deleted.";
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
