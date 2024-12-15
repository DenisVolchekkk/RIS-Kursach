using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.ViewModel;

namespace Presentation.Controllers
{
    public class GroupController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7112/api");
        private readonly HttpClient _client;

        public GroupController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index(string SearchGroupName, string SearchChiefName, string SearchFacilityName, int? pageNumber)
        {
            ViewData["SearchGroupName"] = SearchGroupName;
            ViewData["SearchChiefName"] = SearchChiefName;
            ViewData["SearchFacilityName"] = SearchFacilityName;
            IQueryable<Group> GroupList = null;

            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Group/Filter?Name={SearchGroupName}&Chief.Name={SearchChiefName}&Facility.Name={SearchFacilityName}").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                GroupList = JsonConvert.DeserializeObject<List<Group>>(data).AsQueryable();
            }
            int pageSize = 20;

            return View(PaginatedList<Group>.Create(GroupList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            SetViewDataAsync();
            return View();
        }

        [HttpPost]
        public IActionResult Create(GroupViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Group/Post", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Group created.";
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
                GroupViewModel Group = new GroupViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Group/GetGroup/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Group =  JsonConvert.DeserializeObject<GroupViewModel>(data);
                    SetViewDataAsync(Group.FacilityId, Group.ChiefId);
                }
                return View(Group);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit(GroupViewModel Group)
        {
            string data = JsonConvert.SerializeObject(Group);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Group/Put", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Group updated.";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                GroupViewModel Group = new GroupViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Group/GetGroup/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Group = JsonConvert.DeserializeObject<GroupViewModel>(data);
                    SetViewDataAsync(Group.FacilityId, Group.ChiefId);
                }
                return View(Group);
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
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Group/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Group deleted.";
                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }
        private void SetViewDataAsync(int? facilityId = null, int? chiefId = null)
        {
            List<Facility> facilityList = new List<Facility>();
            HttpResponseMessage response1 = _client.GetAsync(_client.BaseAddress + "/Facility/GetAll").Result;
            if (response1.IsSuccessStatusCode)
            {
                string data = response1.Content.ReadAsStringAsync().Result;
                facilityList = JsonConvert.DeserializeObject<List<Facility>>(data);
            }
            ViewData["FacilityId"] = new SelectList(facilityList, "Id", "Name", facilityId);

            List<Chief> chiefList = new List<Chief>();
            HttpResponseMessage response2 = _client.GetAsync(_client.BaseAddress + "/Chief/GetAll").Result;
            if (response2.IsSuccessStatusCode)
            {
                string data = response2.Content.ReadAsStringAsync().Result;
                chiefList = JsonConvert.DeserializeObject<List<Chief>>(data);
            }
            ViewData["ChiefId"] = new SelectList(chiefList, "Id", "Name", chiefId);
        }
    }
}
