using FUNewsManagementSystem_FE.MVCWebApp.Models;
using FUNewsManagementSystem_FE.MVCWebApp.Models.FormModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FEMVC.Controllers
{
    public class AccountsManagementController : Controller
    {
        public AccountsManagementController()
        {
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("https://localhost:50013/api/" + "SystemAccounts");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<List<SystemAccountView>>(jsonString);
                        return View(list);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving Accounts: {ex.Message}");
            }
            return NotFound();
        }

        public async Task<IActionResult> Details(short id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("https://localhost:50013/api/" + "SystemAccounts/id/" + id);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<SystemAccountView>(jsonString);
                        return View(list);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving Accounts: {ex.Message}");
            }
            return NotFound();
        }

        public async Task<IActionResult> Create(CreateAccountForm acc)
        {
            try
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage + " " + v.Exception).ToList();
                if (ModelState.IsValid)
                {
                    using (var httpClient = new HttpClient())
                    {
                        var content = new StringContent(JsonConvert.SerializeObject(acc), System.Text.Encoding.UTF8, "application/json");
                        var response = await httpClient.PostAsync("https://localhost:50013/api/" + "SystemAccounts", content);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                return View(acc);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving Accounts: {ex.Message}");
            }
            return NotFound();
        }

        public async Task<IActionResult> Delete(short id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.DeleteAsync("https://localhost:50013/api/" + $"SystemAccounts/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["DeleteError"] = "Failed to delete the account. Server returned: " + response.StatusCode;
                        return RedirectToAction(nameof(Details), new { deletedId = id });
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving Accounts: {ex.Message}");
            }
            return NotFound();
        }

        [HttpGet]
        // GET: AccountsManagement/Edit/5
        public async Task<IActionResult> Edit(short id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("https://localhost:50013/api/" +"SystemAccounts/id/" + id);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var account = JsonConvert.DeserializeObject<UpdateAccountForm>(jsonString);
                        return View(account);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving account: {ex.Message}");
            }
            return NotFound();
        }

        [HttpPost]
        // POST: AccountsManagement/Edit/5
        public async Task<IActionResult> Edit(short? id, UpdateAccountForm acc)
        {
            if (id != acc.AccountId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        var content = new StringContent(JsonConvert.SerializeObject(acc), System.Text.Encoding.UTF8, "application/json");
                        var response = await httpClient.PatchAsync("https://localhost:50013/api/" + "SystemAccounts", content);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        ModelState.AddModelError("", "Failed to update account");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating account: {ex.Message}");
                }
            }
            return View(acc);
        }
    }
}