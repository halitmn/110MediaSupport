using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Support110Media.Data.Context;
using Support110Media.Data.Model;
using Support110Media.DataAccess.UnitOfWork;

namespace Support110Media.Controllers
{
    /// <summary>
    /// Uygulamaya giriş kontrolleri
    /// </summary>
    public class LoginController : Controller
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="masterContext"></param>
        public LoginController(MasterContext masterContext)
        {
            this.masterContext = masterContext;
        }
        #endregion

        #region Member

        /// <summary>
        /// Veri tabanı erişim classını nesnesi
        /// </summary>
        private MasterContext masterContext;

        #endregion

        #region Methods

        public IActionResult LoginIndex()
        {
            return View();
        }

        /// <summary>
        /// Uygulamaya giriş yapar
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> LogIn(Data.Model.LoginModel loginModel)
        {
            using (var uow = new UnitOfWork(masterContext))
            {
                if (Environment.GetEnvironmentVariable("USER_NAME")==loginModel.UserName
                    && Environment.GetEnvironmentVariable("PASSWORD")==loginModel.Password)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,loginModel.UserName)
                    };
                    var userIdentity = new ClaimsIdentity(claims, "admin");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(userIdentity);
                    HttpContext.Session.SetString("admin",JsonConvert.SerializeObject(loginModel));
                    await HttpContext.SignInAsync(claimsPrincipal);
                    return RedirectToAction("Index","Main");
                }
                var costumerList = uow.GetRepository<CostumerModel>().GetAll();
                foreach (var costumer in costumerList)
                {
                    if (costumer.CostumerMailAddress == loginModel.UserName && costumer.CostumerPassword == loginModel.Password)
                    {
                        var claims = new List<Claim>
                        {
                           new Claim(ClaimTypes.Name,loginModel.UserName)
                        };
                        var userIdentity = new ClaimsIdentity(claims, "costumer");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(userIdentity);
                        HttpContext.Session.SetString("costumer", JsonConvert.SerializeObject(costumer));
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("SupportIndex", "Support");
                    }
                }

                var userList = uow.GetRepository<UserModel>().GetAll();
                foreach (var user in userList)
                {
                    if (user.UserName == loginModel.UserName && user.Password == loginModel.Password)
                    {
                        var claims = new List<Claim>
                        {
                           new Claim(ClaimTypes.Name,loginModel.UserName)
                        };
                        var userIdentity = new ClaimsIdentity(claims, "user");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(userIdentity);
                        HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Index", "Costumer");
                    }
                }
                
                return View();
            }
        }
        
        /// <summary>
        /// uygulamadan çıkış yapar
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Main");
        }

        #endregion

    }
}