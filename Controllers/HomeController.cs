using azure_ad_address_book.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace azure_ad_address_book.Controllers
{
    [Authorize]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GraphServiceClient _graphServiceClient;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger,
                          GraphServiceClient graphServiceClient,
                          IConfiguration configuration)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["Company"] = _configuration.GetValue<string>("CompanyName");
            return View();
        }

        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("Profile")]
        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> Profile()
        {
            var me = await _graphServiceClient.Me.Request().GetAsync();
            ViewData["Me"] = me;

            try
            {
                // Get user photo
                using (var photoStream = await _graphServiceClient.Me.Photo.Content.Request().GetAsync())
                {
                    byte[] photoByte = ((MemoryStream)photoStream).ToArray();
                    ViewData["Photo"] = Convert.ToBase64String(photoByte);
                }
            }
            catch (System.Exception)
            {
                ViewData["Photo"] = null;
            }

            return View();
        }

        [HttpGet("AddressBook")]
        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> AddressBook()
        {
            List<Microsoft.Graph.User> addressBook = new List<Microsoft.Graph.User>();
            // https://docs.microsoft.com/en-us/graph/api/user-list?view=graph-rest-1.0&tabs=http
            IGraphServiceUsersCollectionPage users = await _graphServiceClient.Users
                .Request().
                Top(999).
                Select(u => new
                {
                    u.GivenName,
                    u.Surname,
                    u.Mail,
                    u.MobilePhone,
                    u.BusinessPhones,
                    u.JobTitle,
                    u.CompanyName,
                    u.Department
                })
                .GetAsync();

            addressBook.AddRange(users.CurrentPage);
            while (users.AdditionalData.ContainsKey("@odata.nextLink"))
            {
                users = await users.NextPageRequest.GetAsync();
                addressBook.AddRange(users.CurrentPage);
            }

            // Filter out empty results
            addressBook = (addressBook.Where(o => o.Mail != "" && o.MobilePhone != "" && o.BusinessPhones.Any())).ToList();
            ViewData["AddressBook"] = addressBook;

            return View();
        }

        [HttpGet("Error")]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
