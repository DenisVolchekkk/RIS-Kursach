﻿using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.ViewModel;
namespace Presentation.Controllers
{
    public class TeacherController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7112/api");
        private readonly HttpClient _client;

        public TeacherController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        [HttpGet]
        public IActionResult Index(string SearchTeacherName, string SearchFacilityName, int? pageNumber)
        {
            ViewData["SearchTeacherName"] = SearchTeacherName;
            ViewData["SearchFacilityName"] = SearchFacilityName;
            IQueryable<Teacher> TeacherList = null;

            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Teacher/Filter?Name={SearchTeacherName}&Facility.Name={SearchFacilityName}").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                TeacherList = JsonConvert.DeserializeObject<List<Teacher>>(data).AsQueryable();
            }
            int pageSize = 20;

            return View(PaginatedList<Teacher>.Create(TeacherList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            SetViewDataAsync();
            return View();
        }

        [HttpPost]
        public IActionResult Create(TeacherViewModel model)
        {
            try
            {

                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Teacher/Post", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Teacher created.";
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
                TeacherViewModel Teacher = new TeacherViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Teacher/GetTeacher/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Teacher = JsonConvert.DeserializeObject<TeacherViewModel>(data);
                }
                SetViewDataAsync(Teacher.Id);
                return View(Teacher);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit(Teacher Teacher)
        {
            string data = JsonConvert.SerializeObject(Teacher);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Teacher/Put", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Teacher updated.";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                TeacherViewModel Teacher = new TeacherViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Teacher/GetTeacher/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Teacher = JsonConvert.DeserializeObject<TeacherViewModel>(data);
                }
                SetViewDataAsync(Teacher.Id);
                return View(Teacher);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult  DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Teacher/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Teacher deleted.";
                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }
        private void SetViewDataAsync(int? facilityId = null)
        {
            List<Facility> facilityList = new List<Facility>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Facility/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                facilityList = JsonConvert.DeserializeObject<List<Facility>>(data);
            }
            ViewData["FacilityId"] = new SelectList(facilityList, "Id", "Name", facilityId);
        }
    }
}
