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
                return View(new List<CategoryModel>());
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category categoryModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var httpClient = new HttpClient())
                    {
                        var content = new StringContent(JsonConvert.SerializeObject(categoryModel), System.Text.Encoding.UTF8, "application/json");
                        var response = await httpClient.PostAsync(ProjectConstant.APIEndPoint + "api/Categories/create", content);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to create category.");
                            return View(categoryModel);
                        }
                    }
                }
                return View(categoryModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating category: {ex.Message}");
                return View(categoryModel);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.DeleteAsync(ProjectConstant.APIEndPoint + "api/Categories/" + id);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to remove category.");
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error removing category: {ex.Message}");
                return RedirectToAction("Index");
            }
        }
    }
}
