using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem_FE.MVCWebApp.Controllers
{
    public class AccountController : Controller
    {
        // GET: AccountController1cs
        public ActionResult Index()
        {
            return View();
        }

        // GET: AccountController1cs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController1cs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController1cs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController1cs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController1cs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController1cs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController1cs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
