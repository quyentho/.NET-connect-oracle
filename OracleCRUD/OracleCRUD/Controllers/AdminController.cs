using AutoMapper;
using BL.Profile;
using BL.TableSpace;
using BL.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OracleCRUD.Helpers;
using OracleCRUD.Models.Admin;
using System;
using System.Collections.Generic;

namespace OracleCRUD.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userService;
        private readonly ITableSpaceRepository _tableSpaceService;
        private readonly IProfileRepository _profileService;
        private readonly IMapper _mapper;

        public AdminController(IUserRepository userService,
            ITableSpaceRepository tableSpaceService,
            IProfileRepository profileService,
            IMapper mapper)
        {
            _userService = userService;
            this._tableSpaceService = tableSpaceService;
            this._profileService = profileService;
            this._mapper = mapper;
        }

        // GET: UserController

        private bool LoggedInAsAdmin()
        {
            return HttpContext.Session.Get("Username") != null
                && HttpContext.Session.Get<string>("Username").Equals("dotnetcore", StringComparison.OrdinalIgnoreCase);
        }
        public ActionResult Index()
        {
            if (LoggedInAsAdmin())
            {
                List<ApplicationUser> users = _userService.GetAll();
                var userViewModels = _mapper.Map<List<UserIndexViewModel>>(users);

                return View(userViewModels);
            }

            return RedirectToAction("Login", "User");

        }

        // GET: UserController/Details/5
        public ActionResult Details(double id)
        {
            if (LoggedInAsAdmin())
            {
                ApplicationUser user = _userService.FindById(id);
                var userViewModel = _mapper.Map<UserDetailViewModel>(user);
                return View(userViewModel);
            }
            return RedirectToAction("Login", "User");

        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            if (LoggedInAsAdmin())
            {
                UserCreateViewModel userViewModel = new UserCreateViewModel();

                List<BL.Profile.Profile> profiles = _profileService.GetAll();
                userViewModel.ProfileList = _mapper.Map<List<SelectListItem>>(profiles);

                List<TableSpace> tableSpaces = _tableSpaceService.GetAll();
                userViewModel.TableSpaceList = _mapper.Map<List<SelectListItem>>(tableSpaces);

                return View(userViewModel);
            }
            return RedirectToAction("Login", "User");
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserCreateViewModel userViewModel)
        {

            if (LoggedInAsAdmin())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        ApplicationUser user = _mapper.Map<ApplicationUser>(userViewModel);

                        user.PasswordHash = SHA512PasswordHasher.Hash(userViewModel.PasswordHash);
                        _userService.Create(user);
                        return RedirectToAction(nameof(Index));
                    }

                    return View();

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
            }
            return RedirectToAction("Login", "User");
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(double id)
        {
            if (LoggedInAsAdmin())
            {
                if (TempData["postError"] != null)
                {
                    ModelState.AddModelError(string.Empty, TempData["postError"].ToString());
                }
                var user = _userService.FindById(id);
                UserEditViewModel userViewModel = _mapper.Map<UserEditViewModel>(user);

                List<BL.Profile.Profile> profiles = _profileService.GetAll();
                userViewModel.ProfileList = _mapper.Map<List<SelectListItem>>(profiles);

                List<TableSpace> tableSpaces = _tableSpaceService.GetAll();
                userViewModel.TableSpaceList = _mapper.Map<List<SelectListItem>>(tableSpaces);

                return View(userViewModel);
            }
            return RedirectToAction("Login", "User");
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserEditViewModel userEditViewModel)
        {

            if (LoggedInAsAdmin())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var userFromDb = _userService.FindById(id);

                        if (userFromDb.Username != userEditViewModel.Username)
                        {
                            TempData["postError"] = "Don't try to hack my site, brooo!";
                            return RedirectToAction(nameof(Edit), id);
                        }

                        _mapper.Map(userEditViewModel, userFromDb);
                        if (!string.IsNullOrEmpty(userEditViewModel.PasswordHash))
                        {
                            userFromDb.PasswordHash = SHA512PasswordHasher.Hash(userEditViewModel.PasswordHash);
                        }


                        _userService.Update(id, userFromDb);

                        return RedirectToAction(nameof(Index));
                    }

                    return Edit(id);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
            }
            return RedirectToAction("Login", "User");
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(double id)
        {
            ApplicationUser user = _userService.FindById(id);
            var userViewModel = _mapper.Map<UserDeleteViewModel>(user);
            return View(userViewModel);

            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(double id)
        {
            if (LoggedInAsAdmin())
            {
                try
                {
                    _userService.Delete(id);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
            }
            return RedirectToAction("Login", "User");
        }
    }
}
