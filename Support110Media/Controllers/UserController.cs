using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Support110Media.Data.Context;
using Support110Media.Data.Model;
using Support110Media.DataAccess.UnitOfWork;
using Support110Media.Helper;

namespace Support110Media.Controllers
{
    /// <summary>
    /// Kullanıcı işlemleri sınıfı
    /// </summary>
    [Authorize, AuthFilter]
    public class UserController : Controller
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public UserController(MasterContext context)
        {
            masterContext = context;
        }

        #endregion

        #region Member

        private MasterContext masterContext;

        #endregion

        #region Methods

        /// <summary>
        /// Kullanıcı listesini döner
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            List<UserModel> userList;
            using (UnitOfWork unitOfWork = new UnitOfWork(masterContext))
            {
                userList = new List<UserModel>();
                userList =  unitOfWork.GetRepository<UserModel>().GetAll().ToList();
            }
            return View(userList);
        }

        /// <summary>
        /// yeni kullanııcı ekleme viewiniz döner
        /// </summary>
        /// <returns></returns>
        public IActionResult AddNewUser()
        {
            return View();
        }

        /// <summary>
        /// Yeni kullanıcı ekler
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddNewUser(UserModel userModel)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(masterContext))
            {
                unitOfWork.GetRepository<UserModel>().Add(userModel);
                unitOfWork.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Kullanıcıı siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteUser(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(masterContext))
            {
                unitOfWork.GetRepository<UserModel>().Delete(id);
                unitOfWork.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Kullanıcı güncelleme viewin döner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult UpdateUser(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(masterContext))
            {
               ViewBag.User = unitOfWork.GetRepository<UserModel>().GetById(id);
            }
            return View();
        }

        /// <summary>
        /// Kullanıcı günceller
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateUser(UserModel userModel)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(masterContext))
            {
                unitOfWork.GetRepository<UserModel>().Update(userModel);
                unitOfWork.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        #endregion

    }
}