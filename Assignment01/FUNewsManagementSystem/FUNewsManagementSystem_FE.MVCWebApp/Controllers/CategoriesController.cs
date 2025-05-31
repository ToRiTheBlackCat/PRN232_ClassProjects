using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem_FE.MVCWebApp.Constant;
using FUNewsManagementSystem_FE.MVCWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

namespace FUNewsManagementSystem_FE.MVCWebApp.Controllers
{
    public class CategoriesController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + "api/Categories");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var categories = JsonConvert.DeserializeObject<List<CategoryModel>>(jsonString);
                        return View(categories);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to retrieve categories.");
                        return View(new List<CategoryModel>());
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving categories: {ex.Message}");
                return View(new List<Category>());
            }
        }
    }
}
