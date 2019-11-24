using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Support110Media.Data.Context;
using Support110Media.Data.Model;
using Support110Media.DataAccess.UnitOfWork;
using Support110Media.Helper;
using Support110Media.Utils.Helper;

namespace Support110Media.Controllers
{
    /// <summary>
    /// Costumer işlemleri controlleri
    /// </summary>
    [Authorize, AuthFilter]
    public class MainController : Controller
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="masterContext"></param>
        public MainController(MasterContext masterContext)
        {
            context = masterContext;
        }

        #endregion

        #region Member

        private MasterContext context;

        #endregion

        #region Methods

        /// <summary>
        /// Costumer listesini döner
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var costumerList = new List<CostumerModel>();
            using (UnitOfWork unitOfWork = new UnitOfWork(context))
            {
                costumerList = unitOfWork.GetRepository<CostumerModel>().GetAll().ToList();
            }
            return View(costumerList);
        }

        /// <summary>
        /// yeni kullanıcı ekleme viewi döner
        /// </summary>
        /// <returns></returns>
        public IActionResult AddNewCostumer()
        {
            return View();
        }

        /// <summary>
        /// yeni kullanıcı ekler
        /// </summary>
        /// <param name="costumerModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddNewCostumer(CostumerModel costumerModel)
        {
            using (var unitofwork = new UnitOfWork(context))
            {
                unitofwork.GetRepository<CostumerModel>().Add(costumerModel);
                unitofwork.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Kullanıcı siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteCostumer(int id)
        {
            using (var unitOfWork = new UnitOfWork(context))
            {
                unitOfWork.GetRepository<CostumerModel>().Delete(id);
                unitOfWork.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Kullanıcı güncelleme viewi döner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult UpdateCostumer(int id)
        {
            using (var unitOfWork = new UnitOfWork(context))
            {
                ViewBag.Costumer = unitOfWork.GetRepository<CostumerModel>().GetById(id);
            }
            return View();
        }

        /// <summary>
        /// Kullanıcı günceller
        /// </summary>
        /// <param name="costumerModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateCostumer(CostumerModel costumerModel)
        {
            using (var unitofwork = new UnitOfWork(context))
            {
                unitofwork.GetRepository<CostumerModel>().Update(costumerModel);
                unitofwork.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        
        public IActionResult SendMail(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(context))
            {
                var costumer = unitOfWork.GetRepository<CostumerModel>().GetById(id);
                if (costumer != null && !string.IsNullOrEmpty(costumer.CostumerMailAddress))
                {
                    HelperMethods.SendMail(costumer.CostumerMailAddress);
                }
                return RedirectToAction("Index");
            }
        }

        #endregion

    }
}