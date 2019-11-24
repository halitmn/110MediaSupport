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
    /// File backend
    /// </summary>
    [Authorize,AuthFilter]
    public class FileController : Controller
    {
        #region Constructor

        public FileController(MasterContext context)
        {
            masterContext = context;
        }

        #endregion

        #region Member

        private MasterContext masterContext;

        #endregion

        #region Methods

        /// <summary>
        /// dosya listesini döner
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var fileModelList = new List<FileModel>();
            using (UnitOfWork unitOfWork = new UnitOfWork(masterContext))
            {
                fileModelList = unitOfWork.GetRepository<FileModel>().GetAll().ToList();
                foreach (var item in fileModelList)
                {
                    item.CostumerModel = unitOfWork.GetRepository<CostumerModel>().GetById(item.CostumerId);
                }
            }
            return View(fileModelList);
        }

        /// <summary>
        /// Dosya ekeleme viewi döner
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddNewFile()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(masterContext))
            {
                ViewBag.Costumer = unitOfWork.GetRepository<CostumerModel>().GetAll();
                return View();
            }
        }

        /// <summary>
        /// Dosya ekler
        /// </summary>
        /// <param name="fileModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddNewFile(FileModel fileModel)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(masterContext))
            {
                unitOfWork.GetRepository<FileModel>().Add(fileModel);
                unitOfWork.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Dosya siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteFile(int id)
        {
            using (var unitOfWork = new UnitOfWork(masterContext))
            {
                unitOfWork.GetRepository<FileModel>().Delete(id);
                unitOfWork.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Dosya güncelleme viewi döner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult UpdateFile(int id)
        {
            using (var unitofwork = new UnitOfWork(masterContext))
            {
                ViewBag.Costumers = unitofwork.GetRepository<CostumerModel>().GetAll();
                ViewBag.File = unitofwork.GetRepository<FileModel>().GetById(id);
            }
            return View();
        }

        /// <summary>
        /// Dosya günceller
        /// </summary>
        /// <param name="fileModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateFile(FileModel fileModel)
        {
            using (UnitOfWork unitofwork = new UnitOfWork(masterContext))
            {
                unitofwork.GetRepository<FileModel>().Update(fileModel);
                unitofwork.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}