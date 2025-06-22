using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Repository.Models.FormModels;
using FUNewsManagementSystem_FE.MVCWebApp.Constant;
using Newtonsoft.Json;
using AutoMapper;

namespace FUNewsManagementSystem_FE.RazorPageWebApp.Pages.Accounts
{
    public class EditModel : PageModel
    {
        private readonly IMapper _mapper;

        public EditModel(IMapper mapper)
        {
            _mapper = mapper;
        }

        [BindProperty]
        public UpdateAccountForm SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(ProjectConstant.APIEndPoint + $"odata/SystemAccounts({id})");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var item = JsonConvert.DeserializeObject<SystemAccountView>(jsonString);
                        if (item == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            SystemAccount = _mapper.Map<UpdateAccountForm>(item);
                        }
                        return Page();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving Accounts: {ex.Message}");
            }
            return NotFound();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                using (var httpClient = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(SystemAccount), System.Text.Encoding.UTF8, "application/json");
                    var response = await httpClient.PatchAsync(ProjectConstant.APIEndPoint + $"odata/SystemAccounts", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("./Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error retrieving Accounts: {ex.Message}");
            }
            return NotFound();
        }
    }
}
