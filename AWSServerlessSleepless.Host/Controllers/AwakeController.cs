using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerlessSleepless.Awaker.Common.Logging;
using ServerlessSleepless.Host.Services.Interfaces;

namespace ServerlessSleepless.Host.Controllers
{
    public class AwakeController : Controller
    {
        public AwakeController(ISelfAwakeService service)
        {
            this.Logger = CustomLogFactory.CreateLogger(this.GetType());
            Service = service;
        }

        // GET /
        [HttpGet]
        public JsonResult Index()
        {
            return Json(Service.Status());
        }

        public ILogger Logger { get; private set; }

        ISelfAwakeService Service;

    }
}
