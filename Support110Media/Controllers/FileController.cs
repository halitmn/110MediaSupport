using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Support110Media.Data.Context;
using Support110Media.Data.Model;
using Support110Media.DataAccess.UnitOfWork;
using Support110Media.Helper;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Support110Media.Utils.Log;

namespace Support110Media.Controllers
{
    /// <summary>
    /// File backend
    /// </summary>
    [Authorize, AuthFilter]
    public class FileController : Controller
    {
        #region Constructor

        public FileController(MasterContext context, IHostingEnvironment env)
        {
            masterContext = context;
            wwwrootPath = env.WebRootPath;
        }

        #endregion

        #region Member

        private MasterContext masterContext;
        private string wwwrootPath;

        #endregion

        #region Methods

        /// <summary>
        /// dosya listesini döner
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex, "FileIndex");
                return Content("Error");
            }
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
        public async Task<IActionResult> AddNewFile(FileModel fileModel, IFormFile file)
        {
            try
            {
                if (file != null && file.Length != 0)
                {
                    var path = wwwrootPath + @"\AudioFileUploaded\" + file.FileName;
                    Logger.Info(path);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        var splitPath = path.Split("\\");
                        path = string.Empty;
                        for (int i = splitPath.Length; i > 0; i--)
                        {
                            path = splitPath[i - 2].ToString() + "/" + splitPath[i - 1].ToString();
                            break;
                        }
                        Logger.Info(path);
                        Logger.Info(Environment.GetEnvironmentVariable("URI") + path);
                        int index = file.FileName.IndexOf('.');
                        string fileName = file.FileName.Substring(0, index);
                        using (UnitOfWork unitOfWork = new UnitOfWork(masterContext))
                        {
                            fileModel.FileName = fileName;
                            fileModel.FileUploadDate = DateTime.Now.ToShortDateString();
                            fileModel.FilePath = Environment.GetEnvironmentVariable("URI") + path;
                            unitOfWork.GetRepository<FileModel>().Add(fileModel);
                            unitOfWork.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "FileUpload");
                return Content("Error");
            }
        }

        /// <summary>
        /// Dosya siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteFile(int id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(masterContext))
                {
                    unitOfWork.GetRepository<FileModel>().Delete(id);
                    unitOfWork.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "DeleteFile");
                return Content("Error");
            }

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
            try
            {
                using (UnitOfWork unitofwork = new UnitOfWork(masterContext))
                {
                    unitofwork.GetRepository<FileModel>().Update(fileModel);
                    unitofwork.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "UpdateFile(FileModel)");
                return Content("Error");
            }
        }

        #endregion
    }
}