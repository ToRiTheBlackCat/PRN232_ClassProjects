using BusinessObjects;
using BusinessObjects.ViewModel.Accounts;
using DataAccessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using ProductManagementMVC.Constant;


//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductManagementMVC.Controllers
{
    [Authorize(Roles = "1")]
    public class AccountMembersController : Controller
    {
        public static string API_ENDPOINT = ApiUrlConstant.APIEndPoint;

        public AccountMembersController()
        {
        }

        // GET: AccountMembers
        public async Task<IActionResult> Index(string fullName = "", string email = "", int roleId = 0, int pageNum = 1)
        {
            ViewBag.FullName = fullName;
            ViewBag.Email = email;
            ViewBag.RoleId = roleId;

            using (var httpClient = new HttpClient())
            {
                var queryParams = $"?fullName={Uri.EscapeDataString(fullName)}&email={Uri.EscapeDataString(email)}&roleId={roleId}&pageNum={pageNum}";
                using (var response = await httpClient.GetAsync(API_ENDPOINT + "Accounts" + queryParams))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<PaginatedAccountMembers>(content);

                        if (result != null)
                        {
                            ViewBag.RoleList = new SelectList(AccountRoles(), "Value", "Text");
                            return View(result);
                        }
                    }
                }
            }

            return View(new PaginatedAccountMembers());
        }

        // GET: AccountMembers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(API_ENDPOINT + "Accounts/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var accountMember = JsonConvert.DeserializeObject<AccountMember>(content);

                        if (accountMember != null)
                        {
                            var viewModel = AccountMemberFormModel.ToFormModel(accountMember);
                            return View(viewModel);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        return Ok($"Error: {response.StatusCode}");
                    }
                }
            }

            return NotFound("Internal Server Error.");
        }

        // GET: AccountMembers/Create
        public IActionResult Create()
        {
            ViewBag.RoleList = new SelectList(AccountRoles(), "Value", "Text");
            return View();
        }

        // POST: AccountMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<AccountMemberFormModel>> Create([Bind("MemberId,Password,ConfirmPassword,FullName,EmailAddress,MemberRole")] AccountMemberFormModel accountMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (AccountMemberExists(accountMember.MemberId).Result)
            {
                ViewBag.Message = "Account member already exists.";
                ViewBag.RoleList = new SelectList(AccountRoles(), "Value", "Text");
                ModelState.AddModelError("MemberId", "Account member already exists.");
                return View(accountMember);
            }

            using (HttpClient client = new HttpClient())
            {
                // Optional: Add headers (e.g., JWT authorization)
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", "your_jwt_token");

                // Serialize the object to JSON
                var json = JsonConvert.SerializeObject(accountMember.ToAccountMember());
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send POST request
                HttpResponseMessage response = await client.PostAsync(API_ENDPOINT + "Accounts/", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var addedMember = JsonConvert.DeserializeObject<AccountMember>(responseBody);
                    TempData["Message"] = "Created account successfully.";

                    //return View(AccountMemberFormModel.ToFormModel(addedMember!));
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return Ok($"Error: {response.StatusCode}");
                }
            }

            return Ok("Internal Server Error");
        }

        // GET: AccountMembers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(API_ENDPOINT + "Accounts/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var accountMember = JsonConvert.DeserializeObject<AccountMember>(content);

                        if (accountMember != null)
                        {
                            var viewModel = AccountMemberFormModel.ToFormModel(accountMember);
                            ViewBag.RoleList = new SelectList(AccountRoles(), "Value", "Text", accountMember.MemberRole);
                            return View(viewModel);
                        }
                    }
                }
            }

            return NotFound();
        }

        // POST: AccountMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MemberId,Password,ConfirmPassword,FullName,EmailAddress,MemberRole")] AccountMemberFormModel accountMember)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            if (id != accountMember.MemberId)
            {
                return NotFound();
            }

            using (HttpClient client = new HttpClient())
            {
                // Optional: Add headers (e.g., JWT authorization)
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", "your_jwt_token");

                // Serialize the object to JSON
                var json = JsonConvert.SerializeObject(accountMember.ToAccountMember());
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send POST request
                HttpResponseMessage response = await client.PutAsync(API_ENDPOINT + "Accounts/" + id, content);

                ViewBag.RoleList = new SelectList(AccountRoles(), "Value", "Text");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Update successful: " + responseBody);
                    TempData["Message"] = "Edit account successfully.";

                    return View(accountMember);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    ModelState.AddModelError("", $"Error: {response.StatusCode}");
                    return View(accountMember);
                }
            }
        }

        // GET: AccountMembers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(API_ENDPOINT + "Accounts/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var accountMember = JsonConvert.DeserializeObject<AccountMember>(content);

                        if (accountMember != null)
                        {
                            var viewModel = AccountMemberFormModel.ToFormModel(accountMember);
                            return View(accountMember);
                        }
                    }
                }
            }

            return NotFound();
        }

        // POST: AccountMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(API_ENDPOINT + "Accounts/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return NotFound();
        }

        private async Task<bool> AccountMemberExists(string id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(API_ENDPOINT + "Accounts/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var accountMember = JsonConvert.DeserializeObject<AccountMember>(content);

                        if (accountMember != null)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private List<SelectListItem> AccountRoles()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "Admin" },
                new SelectListItem { Value = "2", Text = "Manager" },
                new SelectListItem { Value = "3", Text = "Member" },
            };

            return list;
        }
    }
}
