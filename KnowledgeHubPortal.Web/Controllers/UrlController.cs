
using KnowledgeHubPortal.Core.Entities;
using KnowledgeHubPortal.Core.Interfaces;
using KnowledgeHubPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHubPortal.Web.Controllers
{
    public class UrlController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UrlController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public IActionResult Submit()
        {
            var viewModel = new SubmitUrlFormViewModel
            {
                Categories = GetCategoriesSelectList()
            };
            return View(viewModel);
        }


        /*[HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(SubmitUrlViewModel model)
        {
            try
            {
                model.Categories = GetCategoriesSelectList();
                Console.WriteLine($"Received Link: {model.Link}");
                Console.WriteLine($"Received Title: {model.Title}");
                Console.WriteLine($"Received Description: {model.Description}");
                Console.WriteLine($"Received CategoryId: {model.CategoryId}");
                Console.WriteLine("Submit action started");
                if (ModelState.IsValid)
                {
                    Console.WriteLine("Model is valid");
                    var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    Console.WriteLine($"User ID Claim: {userIdClaim}");

                    if (string.IsNullOrEmpty(userIdClaim))
                    {
                        Console.WriteLine("User ID not found");
                        ModelState.AddModelError("", "User ID not found. Please try logging in again.");
                        model.Categories = GetCategoriesSelectList();
                        return View(model);
                    }

                    if (!int.TryParse(userIdClaim, out int userId))
                    {
                        Console.WriteLine("Invalid User ID format");
                        ModelState.AddModelError("", "Invalid User ID format.");
                        model.Categories = GetCategoriesSelectList();
                        return View(model);
                    }

                    Console.WriteLine($"Parsed User ID: {userId}");

                    var url = new Url
                    {
                        Link = model.Link,
                        Title = model.Title,
                        Description = model.Description,
                        CategoryId = model.CategoryId,
                        UserId = userId,
                        IsApproved = false,
                        SubmittedAt = DateTime.UtcNow
                    };

                    Console.WriteLine("URL object created");

                    _unitOfWork.Urls.Add(url);
                    _unitOfWork.SaveChanges();

                    Console.WriteLine("URL saved to database");

                    TempData["SuccessMessage"] = "URL submitted successfully. It will be reviewed by an admin.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Console.WriteLine("Model is invalid");
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine($"Model Error: {error.ErrorMessage}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Submit action: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                ModelState.AddModelError("", "An error occurred while submitting the URL. Please try again.");
            }

           
            return View(model);
        }*/

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(UrlSubmissionViewModel model)
        {
            Console.WriteLine($"Received Link: {model.Link}");
            Console.WriteLine($"Received Title: {model.Title}");
            Console.WriteLine($"Received Description: {model.Description}");
            Console.WriteLine($"Received CategoryId: {model.CategoryId}");

            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var url = new Url
                {
                    Link = model.Link,
                    Title = model.Title,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    UserId = userId,
                    IsApproved = false,
                    SubmittedAt = DateTime.UtcNow
                };

                _unitOfWork.Urls.Add(url);
                _unitOfWork.SaveChanges();

                TempData["SuccessMessage"] = "URL submitted successfully. It will be reviewed by an admin.";
                return RedirectToAction("Index", "Home");
            }

            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                if (state.Errors.Count > 0)
                {
                    Console.WriteLine($"Error for {key}: {state.Errors[0].ErrorMessage}");
                }
            }

            // If we get here, something failed; redisplay form
            var viewModel = new SubmitUrlFormViewModel
            {
                Link = model.Link,
                Title = model.Title,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Categories = GetCategoriesSelectList()
            };
            return View(viewModel);
        }


        [Authorize(Roles = "A")]
        public IActionResult Approve()
        {
            var pendingUrls = _unitOfWork.Urls.GetAllIncluding(u => u.Category, u => u.User)
                .Where(u => !u.IsApproved)
                .ToList();
            return View(pendingUrls);
        }

        [HttpPost]
        [Authorize(Roles = "A")]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveUrl(int id)
        {
            var url = _unitOfWork.Urls.GetById(id);
            if (url != null)
            {
                url.IsApproved = true;
                url.ApprovedAt = DateTime.UtcNow;
                _unitOfWork.SaveChanges();

                TempData["SuccessMessage"] = "URL approved successfully.";
            }
            return RedirectToAction("Approve");
        }

        [HttpPost]
        [Authorize(Roles = "A")]
        [ValidateAntiForgeryToken]
        public IActionResult RejectUrl(int id)
        {
            var url = _unitOfWork.Urls.GetById(id);
            if (url != null)
            {
                _unitOfWork.Urls.Delete(url);
                _unitOfWork.SaveChanges();
                TempData["SuccessMessage"] = "URL rejected successfully.";
            }
            return RedirectToAction("Approve");
        }

        public IActionResult Browse()
        {
            var approvedUrls = _unitOfWork.Urls.GetAllIncluding(u => u.Category, u => u.User)
                    .Where(u => u.IsApproved)
                    .ToList();
            return View(approvedUrls);
        }

        private List<SelectListItem> GetCategoriesSelectList()
        {
            return _unitOfWork.Categories.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                })
                .OrderBy(c => c.Text)
                .ToList();
        }

        /*private List<SelectListItem> GetCategoriesSelectList()
        {
            var categories = _unitOfWork.Categories.GetAll().ToList();
            return categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();
        }*/
    }
}