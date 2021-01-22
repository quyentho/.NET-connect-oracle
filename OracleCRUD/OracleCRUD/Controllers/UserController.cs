using AutoMapper;
using BL.Authentication;
using BL.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OracleCRUD.Models.User;
using System;
using OracleCRUD.Helpers;

namespace OracleCRUD.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthenticUserService _authenticUserService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IAuthenticUserService authenticUserService,
                            IUserRepository userRepository,
                            IMapper mapper)
        {
            this._authenticUserService = authenticUserService;
            this._userRepository = userRepository;
            this._mapper = mapper;
        }
        // GET: UserController
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            try
            {

                if (loginViewModel.Username.Equals("dotnetcore", StringComparison.OrdinalIgnoreCase))
                {
                    bool isAdmin = _authenticUserService.CheckConnection(loginViewModel.Username, loginViewModel.Password);
                    if (isAdmin)
                    {
                        HttpContext.Session.Set("Username", loginViewModel.Username);
                        return RedirectToAction("Index", "Admin");
                    }
                }

                loginViewModel.Password = SHA512PasswordHasher.Hash(loginViewModel.Password);

                var user = _authenticUserService.GetMyInfo(loginViewModel.Username, loginViewModel.Password);

                HttpContext.Session.Set("UserId", user.Id);
                HttpContext.Session.Set("Username", loginViewModel.Username);

                ProfileViewModel userVM = _mapper.Map<ProfileViewModel>(user);

                return View("Profile", userVM);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        public ActionResult Profile(ProfileViewModel profileViewModel)
        {
            if (LoggedIn())
            {
                return View(profileViewModel);
            }

            return RedirectToAction(nameof(Login));
        }

        private bool LoggedIn()
        {
            return HttpContext.Session.Get("UserId") != null && HttpContext.Session.Get("Username") != null;
        }

        public ActionResult ProfilePost(ProfileViewModel profileViewModel)
        {
            if (LoggedIn())
            {
                try
                {
                    double loggedInUserId = HttpContext.Session.Get<double>("UserId");

                    if (loggedInUserId != profileViewModel.Id)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid update attempt");
                        return View("Profile", profileViewModel);
                    }

                    var userFromDb = _userRepository.FindById(loggedInUserId);

                    if (userFromDb is null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid update attempt");
                        return View("Profile", profileViewModel);
                    }

                    _mapper.Map(profileViewModel, userFromDb);

                    _authenticUserService.UpdateMyInfo(userFromDb);

                    return View("Profile", profileViewModel);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View("Profile", profileViewModel);
                }
            }

            return RedirectToAction(nameof(Login));
        }
    }
}
