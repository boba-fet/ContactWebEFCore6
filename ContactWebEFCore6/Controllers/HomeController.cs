using ContactWebEFCore6.Data;
using ContactWebEFCore6.Models;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContactWebEFCore6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRolesService _userRolesService;
        private readonly TelemetryClient _telemetryClient;

        public HomeController(ILogger<HomeController> logger, IUserRolesService userRolesService, TelemetryClient telemetryClient)
        {
            _logger = logger;
            _userRolesService = userRolesService;
            _telemetryClient = telemetryClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            _telemetryClient.TrackPageView("Privacy");
            _telemetryClient.TrackEvent("The user hit the privacy page");
            return View();
        }

        public IActionResult TestCustomExceptions()
        {
            try
            {
                throw new Exception("This is a test of my custom exception tracking");
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
            }
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> EnsureRolesAndUsers()
        {
            await _userRolesService.EnsureAdminUserRole();
            return RedirectToAction("Index");
        }
    }
}