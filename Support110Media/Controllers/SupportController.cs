using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Support110Media.Data.Context;
using Support110Media.Data.Model;
using Support110Media.DataAccess.UnitOfWork;
using System.Linq;

namespace Support110Media.Controllers
{
    [Authorize]
    public class SupportController : Controller
    {
        public SupportController(MasterContext masterContext)
        {
            this.masterContext = masterContext;
        }
        private CostumerModel costumer;
        private MasterContext masterContext;

        public IActionResult SupportIndex()
        {
            var costumerSession = HttpContext.Session.GetString("costumer");
            using (var unitofWork = new UnitOfWork(masterContext))
            {
                if (costumerSession != null)
                {
                    costumer = new CostumerModel();
                    costumer = JsonConvert.DeserializeObject<CostumerModel>(costumerSession);
                    var fileList = unitofWork.GetRepository<FileModel>().GetAll().
                                   Where(x => x.CostumerId == costumer.CostumerId).ToList();
                    return View(fileList);
                }
                var adminSession = HttpContext.Session.GetString("admin");
                if (adminSession != null)
                {
                    var fileList = unitofWork.GetRepository<FileModel>().GetAll().ToList();
                    return View(fileList);
                }
                return RedirectToAction("Login", "Login");
            }
        }
    }
}