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
using X.PagedList;
using X.PagedList.Extensions;


namespace ProductManagementMVC.Controllers
{
    [Authorize(Roles = "1 , 2")]
    public class ProductsController : Controller
    {
        public ProductsController()
        {

        }
        // GET: Products
        [Authorize]
        public async Task<IActionResult> Index(string? productName, string? categoryName, int page)
        {
            page = page == 0 ? 1 : page;
            var listProduct = new List<Product>();

            using (var httpClient = new HttpClient())
            {
                if (string.IsNullOrEmpty(productName) && string.IsNullOrEmpty(categoryName))
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
                else
                {
                    using (var response = await httpClient.GetAsync(ApiUrlConstant.APIEndPoint + "Products/search?productName=" + productName + "&categoryName=" + categoryName + "&currentPage=" + page + "&pageSize=" + "6"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseBody = await response.Content.ReadAsStringAsync();
                            listProduct = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                        }
                    }
                }
            }


            var pagedList = listProduct?.ToPagedList(page, 6);
            return View(pagedList);
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
                if (await ProductExists(product.ProductId))
                {
                    ModelState.AddModelError("ProductId", "ProductId already exists. Please use another ProductId.");
                    return View(product);
                }

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

        private async Task<bool> ProductExists(int id)
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
            return foundProduct != null ? true : false;
        }

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
