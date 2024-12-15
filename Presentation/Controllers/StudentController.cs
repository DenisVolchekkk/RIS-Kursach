using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.ViewModel;

namespace Presentation.Controllers
{
    public class StudentController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7112/api");
        private readonly HttpClient _client;

        public StudentController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index(string SearchStudentName, string SearchGroupName, int? pageNumber)
        {
            ViewData["SearchStudentName"] = SearchStudentName;
            ViewData["SearchGroupName"] = SearchGroupName;
            IQueryable<Student> StudentList = null;

            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Student/Filter?Name={SearchStudentName}&Group.Name={SearchGroupName}").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                StudentList = JsonConvert.DeserializeObject<List<Student>>(data).AsQueryable();
            }
            int pageSize = 20;

            return View(PaginatedList<Student>.Create(StudentList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            SetViewDataAsync();
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Student/Post", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Student created.";
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
                StudentViewModel Student = new StudentViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Student/GetStudent/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Student = JsonConvert.DeserializeObject<StudentViewModel>(data);
                    SetViewDataAsync(Student.GroupId);
                }
                return View(Student);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit(Student Student)
        {
            string data = JsonConvert.SerializeObject(Student);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Student/Put", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Student updated.";

                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                StudentViewModel Student = new StudentViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Student/GetStudent/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Student = JsonConvert.DeserializeObject<StudentViewModel>(data);
                    SetViewDataAsync(Student.GroupId);
                }
                return View(Student);
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
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Student/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Student deleted.";
                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }
        private void SetViewDataAsync(int? groupId = null)
        {
            List<Group> groupList = new List<Group>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Group/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                groupList = JsonConvert.DeserializeObject<List<Group>>(data);
            }
            ViewData["GroupId"] = new SelectList(groupList, "Id", "Name", groupId);
        }
    }
}
