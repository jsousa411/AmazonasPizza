using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterProject.Context.Contexts.AppContext;
using StarterProject.Crosscutting;

namespace StarterProject.WebAdmin.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexGrid(string search)
        {
            var query = Context.User.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Email.Contains(search) || c.Name.Contains(search));
            }

            return PartialView(query);
        }

        public IActionResult Add()
        {
            var user = new User() {  Active = true };
            return View("Edit", user);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(User user)
        {
            if (user.Id != 0)
            {
                throw new InvalidOperationException();
            }

            if (user.Password?.Length < 6)
            {
                ModelState.AddModelError("Password", "Password must have at least 6 characters");
            }

            if (ModelState.IsValid)
            {
                user.Password = Tools.EncryptPassword(user.Password, out byte[] salt);
                user.Salt = salt;
                Context.User.Add(user);
                Context.SaveChanges();

                TempData["Success"] = "Record saved successfully.";

                return JavaScript($"views.app.navTo('/Users/Edit/{user.Id}');");
            }
            else
            {
                TempData["Warning"] = "Check the data entered.";
                return View("EditForm", user);
            }
        }

        public IActionResult Edit(int id)
        {
            var user = Context.User.First(c => c.Id == id);
            return View(user);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (user.Id == 0)
            {
                throw new InvalidOperationException();
            }

            if (ModelState.IsValid)
            {
                var dbCopy = Context.User.First(c => c.Id == user.Id);

                Context.Entry(dbCopy).CurrentValues.SetValues(user);
                Context.Entry(dbCopy).Property(c => c.Password).IsModified = false;
                Context.Entry(dbCopy).Property(c => c.Salt).IsModified = false;
                Context.SaveChanges();

                TempData["Success"] = "Record saved successfully.";

                return JavaScript($"views.app.reloadContentShell();");
            }
            else
            {
                TempData["Warning"] = "Check the data entered.";
                return View("EditForm", user);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var user = Context.User.Find(id);
            Context.Remove(user);
            Context.SaveChanges();

            return Ok();
        }
    }
}