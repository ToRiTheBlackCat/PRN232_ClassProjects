using BusinessObjects;
using BusinessObjects.ViewModel.Accounts;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementMVC.Controllers
{
    public class CategoriesController : Controller
    {
        public const string API_ENDPOINT = "https://localhost:58026/api/Categories/";
        private readonly MyStoreDBContext _context;
        public CategoriesController()
        {

        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(API_ENDPOINT))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<List<Category>>(content);

                        if (result != null)
                        {
                            return View(result);
                        }
                    }
                    else
                    {
                        return Ok($"Error: {response.StatusCode}");
                    }
                }
            }

            return View(new List<Category>());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.GetAsync(API_ENDPOINT + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var category = JsonConvert.DeserializeObject<Category>(content);

                        if (category != null)
                        {
                            return View(category);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return Ok($"Error: {response.StatusCode}");
                    }
                }
            }

            return NotFound();
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var httpClient = new HttpClient())
            {
                // Optional: Add headers (e.g., JWT authorization)
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", "your_jwt_token");

                // Serialize the object to JSON
                var json = JsonConvert.SerializeObject(category);
                var body = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(API_ENDPOINT, body))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Category>(content);

                        if (result != null)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        return Ok($"Error: {response.StatusCode}");
                    }
                }
            }

            return Ok("Error: Internal Server Error.");
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(API_ENDPOINT + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var category = JsonConvert.DeserializeObject<Category>(content);

                        if (category != null)
                        {
                            return View(category);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }

            return NotFound();
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryId || !CategoryExists(id).Result)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                // Optional: Add headers (e.g., JWT authorization)
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", "your_jwt_token");

                // Serialize the object to JSON
                var json = JsonConvert.SerializeObject(category);
                var body = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(API_ENDPOINT + id, body))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Category updated successfully.";
                        return View(category);
                    }
                    else
                    {
                        return Ok($"Error: {response.StatusCode}");
                    }
                }
            };
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(API_ENDPOINT + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var category = JsonConvert.DeserializeObject<Category>(content);

                        if (category != null)
                        {
                            return View(category);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }

            return Ok("Error: Internal Server Error.");
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(API_ENDPOINT + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            if (!await CategoryExists(id))
            {
                return NotFound();
            }

            return Ok("Error: Internal Server Error.");
        }

        private async Task<bool> CategoryExists(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(API_ENDPOINT + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var category = JsonConvert.DeserializeObject<Category>(content);

                        if (category != null)
                        {
                            return true;
                        }

                    }
                }
            }

            return false;
        }
    }
}
