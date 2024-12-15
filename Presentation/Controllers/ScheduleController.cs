using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Web;
using Domain.ViewModel;

namespace Presentation.Controllers
{
    public class ScheduleController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7112/api");
        private readonly HttpClient _client;

        public ScheduleController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index(string SearchStartTime, string SearchDayOfWeek, string SearchTeacherName, string SearchDisciplineName, int? pageNumber)
        {
            ViewData["SearchStartTime"] = SearchStartTime;
            ViewData["SearchDayOfWeek"] = SearchDayOfWeek;
            ViewData["SearchTeacherName"] = SearchTeacherName;
            ViewData["SearchDisciplineName"] = SearchDisciplineName;
            HttpResponseMessage response;
            TimeSpan.TryParse(SearchStartTime, out var result);
            string formattedTime = result.ToString("hh\\:mm\\:ss");
            string encodedTime = HttpUtility.UrlEncode(formattedTime).ToUpper();
            Enum.TryParse(typeof(DayOfWeek), SearchDayOfWeek, out var res);
            //response = _client.GetAsync($"{_client.BaseAddress}/Schedule/Filter?hour={result.Hour}&minute={result.Minute}&DayOfWeek={SearchDayOfWeek}&Teacher.Name={SearchTeacherName}&Discipline.Name={SearchDisciplineName}").Result;
            if (result.TotalMinutes != 0)
            {
                response = _client.GetAsync($"{_client.BaseAddress}/Schedule/Filter?StartTime={encodedTime}&DayOfWeek={res}&Teacher.Name={SearchTeacherName}&Discipline.Name={SearchDisciplineName}").Result;
            }
            else 
            {
                response = _client.GetAsync($"{_client.BaseAddress}/Schedule/Filter?DayOfWeek={res}&Teacher.Name={SearchTeacherName}&Discipline.Name={SearchDisciplineName}").Result;

            }
            IQueryable<Schedule> ScheduleList = null;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                ScheduleList = JsonConvert.DeserializeObject<List<Schedule>>(data).AsQueryable();
            }
            int pageSize = 20;

            return View(PaginatedList<Schedule>.Create(ScheduleList.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ScheduleViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Schedule/Post", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Schedule created.";
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
                ScheduleViewModel Schedule = new ScheduleViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Schedule/GetSchedule/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Schedule = JsonConvert.DeserializeObject<ScheduleViewModel>(data);
                    SetViewData(Schedule.DisciplineId, Schedule.TeacherId);
                }
                return View(Schedule);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit(ScheduleViewModel Schedule)
        {
            string data = JsonConvert.SerializeObject(Schedule);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Schedule/Put", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["successMessage"] = "Schedule updated.";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                ScheduleViewModel Schedule = new ScheduleViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Schedule/GetSchedule/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    Schedule = JsonConvert.DeserializeObject<ScheduleViewModel>(data);
                    SetViewData(Schedule.DisciplineId, Schedule.TeacherId);
                }
                return View(Schedule);
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
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Schedule/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Schedule deleted.";
                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

        }
        private void SetViewData(int? disciplineId = null, int ? teacherId = null)
        {
            List<Discipline> disciplineList = new List<Discipline>();
            HttpResponseMessage response1 = _client.GetAsync(_client.BaseAddress + "/Discipline/GetAll").Result;
            if (response1.IsSuccessStatusCode)
            {
                string data = response1.Content.ReadAsStringAsync().Result;
                disciplineList = JsonConvert.DeserializeObject<List<Discipline>>(data);
            }
            ViewData["DisciplineId"] = new SelectList(disciplineList, "Id", "Name", disciplineId);

            List<Teacher> teacherList = new List<Teacher>();
            HttpResponseMessage response2 = _client.GetAsync(_client.BaseAddress + "/Teacher/GetAll").Result;
            if (response2.IsSuccessStatusCode)
            {
                string data = response2.Content.ReadAsStringAsync().Result;
                teacherList = JsonConvert.DeserializeObject<List<Teacher>>(data);
            }
            ViewData["TeacherId"] = new SelectList(teacherList, "Id", "Name", teacherId);
        }
    }
}
