using BusinessObjects;
using DataAccessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductManagementMVC.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementMVC.Controllers
{
    public class ProductsController : Controller
    {
        //private readonly IProductService _context;

        //public ProductsController(MyStoreDBContext context)
        //{
        //    _context = context;
        //}

        public ProductsController()
        {

        }
        // GET: Products
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var listProduct = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(ApiUrlConstant.APIEndPoint + "Products"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        listProduct = JsonConvert.DeserializeObject<List<Product>>(responseBody);


                    }
                }
            }
            return View(listProduct);
        }

        //Products/search?productName=a&categoryName=a&currentPage=1&pageSize=1
        public async Task<IActionResult> Search(string? productName, string? categoryName, int currentPage)
        {
            currentPage = currentPage == 0 ? 1 : currentPage;

            var listProduct = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(ApiUrlConstant.APIEndPoint + "Products/search?productName=" + productName + "&categoryName=" + categoryName + "&currentPage=" + currentPage + "&pageSize=" + "6"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        listProduct = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                    }
                }
            }
            return View("Index", listProduct);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var foundProduct = new Product();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(ApiUrlConstant.APIEndPoint + "Products/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        foundProduct = JsonConvert.DeserializeObject<Product>(responseBody);
                    }
                }
            }
            return View(foundProduct);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var categoryList = await GetCategoryList();

            ViewData["CategoryId"] = new SelectList(categoryList, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,CategoryId,UnitsInStock,UnitPrice")] Product product)
        {
            var categoryList = await GetCategoryList();

            ViewData["CategoryId"] = new SelectList(categoryList, "CategoryId", "CategoryName", product.CategoryId);

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var jsonContent = JsonConvert.SerializeObject(product);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


                    using (var response = await httpClient.PostAsync(ApiUrlConstant.APIEndPoint + "Products", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseBody = await response.Content.ReadAsStringAsync();
                            var createdProduct = JsonConvert.DeserializeObject<Product>(responseBody);

                            if (createdProduct != null)
                            {
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var foundProduct = new Product();

            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.GetAsync(ApiUrlConstant.APIEndPoint + "Products/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        foundProduct = JsonConvert.DeserializeObject<Product>(responseBody);

                        if (foundProduct == null)
                        {
                            return NotFound();
                        }

                        var categoryList = await GetCategoryList();
                        ViewData["CategoryId"] = new SelectList(categoryList, "CategoryId", "CategoryName", foundProduct.CategoryId);
                    }
                }
            }
            return View(foundProduct);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,CategoryId,UnitsInStock,UnitPrice")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest();


            using (var httpClient = new HttpClient())
            {
                var jsonContent = JsonConvert.SerializeObject(product);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


                using (var response = await httpClient.PutAsync(ApiUrlConstant.APIEndPoint + "Products/" + id, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return NotFound();
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var foundProduct = new Product();

            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.GetAsync(ApiUrlConstant.APIEndPoint + "Products/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        foundProduct = JsonConvert.DeserializeObject<Product>(responseBody);

                        if (foundProduct == null)
                        {
                            return NotFound();
                        }
                    }
                }
                return View(foundProduct);
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var foundProduct = new Product();

                using (var response = await httpClient.GetAsync(ApiUrlConstant.APIEndPoint + "Products/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        foundProduct = JsonConvert.DeserializeObject<Product>(responseBody);

                        if (foundProduct == null)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }

                using (var response = await httpClient.DeleteAsync(ApiUrlConstant.APIEndPoint + "Products/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));

                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.ProductId == id);
        //}

        private async Task<List<Category>?> GetCategoryList()
        {
            var categoryList = new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(ApiUrlConstant.APIEndPoint + "Categories"))
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
    }
}
