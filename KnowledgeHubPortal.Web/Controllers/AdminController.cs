using KnowledgeHubPortal.Core.Entities;
using KnowledgeHubPortal.Core.Interfaces;
using KnowledgeHubPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using KnowledgeHubPortal.Web.ViewModels;

namespace KnowledgeHubPortal.Web.Controllers
{
    [Authorize(Roles = "A")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Category Management
        public IActionResult ListCategories()
        {
            var categories = _unitOfWork.Categories.GetAll().ToList();
            return View(categories);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    CategoryName = model.CategoryName,
                    Description = model.Description ?? string.Empty
                };
                _unitOfWork.Categories.Add(category);
                _unitOfWork.SaveChanges();
                TempData["SuccessMessage"] = "Category created successfully.";
                return RedirectToAction(nameof(ListCategories));
            }
            return View(model);
        }

        // User Management
        public IActionResult ListUsers()
        {
            var users = _unitOfWork.Users.GetAll().ToList();
            return View(users);
        }
    }
}