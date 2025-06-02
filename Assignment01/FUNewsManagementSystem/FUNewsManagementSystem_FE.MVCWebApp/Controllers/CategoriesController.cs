using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem_FE.MVCWebApp.Constant;
using FUNewsManagementSystem_FE.MVCWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text.Json;

namespace FUNewsManagementSystem_FE.MVCWebApp.Controllers
{
    public class CategoriesController : Controller
    {
        //DONE
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Index()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + "Categories");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var categories = JsonConvert.DeserializeObject<List<Category>>(jsonString);
                        return View(categories);
                    }
                    //else
                    //{
                    //    ModelState.AddModelError("", "Failed to retrieve categories.");
                    //    return View(new List<Category>());
                    //}
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving categories: {ex.Message}");
            }
            return NotFound();
        }

        //DONE
        public async Task<IActionResult> Details(int? id)
        {

            var foundCategory = new Category();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + "Categories/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        foundCategory = JsonConvert.DeserializeObject<Category>(responseBody);
                    }
                }
            }
            return View(foundCategory);
        }

        public async Task<IActionResult> Create()
        {
            var parentCategoryList = await GetCategoryList();

            ViewData["ParentCategoryId"] = new SelectList(parentCategoryList, "ParentCategoryId", "CategoryName");
            return View();
        }

        private async Task<List<Category>?> GetCategoryList()
        {
            var categoryList = new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + "Categories"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        categoryList = JsonConvert.DeserializeObject<List<Category>>(responseBody);
                    }
                }
            }
            return categoryList;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(category), System.Text.Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(ProjectConstant.APIEndPoint + "Categories", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to create category.");
                        return View(category);
                    }
                }
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id) //DONE GET
        {
            if (id == null)
            {
                return NotFound();
            }
            var foundCategory = new Category();


            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + "Categories/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        foundCategory = JsonConvert.DeserializeObject<Category>(responseBody);

                        if (foundCategory == null)
                        {
                            return NotFound();
                        }

                        var parentCategoryList = await GetCategoryList();

                        ViewData["ParentCategoryId"] = new SelectList(parentCategoryList, "CategoryId", "CategoryName", foundCategory.ParentCategoryId);
                    }
                }
            }
            return View(foundCategory);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("CategoryId,CategoryName,CategoryDesciption,ParentCategoryId,IsActive")] Category cate)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {

                    var content = new StringContent(JsonConvert.SerializeObject(cate), System.Text.Encoding.UTF8, "application/json");
                    var response = await httpClient.PutAsync(ProjectConstant.APIEndPoint + "Categories/" + id, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update category.");
                        return View(cate);
                    }
                }
            }
            return View(cate);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var foundCategory = new Category();

            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + "Categories/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        foundCategory = JsonConvert.DeserializeObject<Category>(responseBody);

                        if (foundCategory == null)
                        {
                            return NotFound();
                        }
                    }
                }
                return View(foundCategory);
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short? id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(ProjectConstant.APIEndPoint + "Categories/" + id))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var message = await response.Content.ReadAsStringAsync();

                        TempData["AlertMessage"] = message;
                        

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
