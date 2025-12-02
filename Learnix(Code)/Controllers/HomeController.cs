using Learnix.Models;
using Learnix.Others;
using Learnix.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Learnix.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailService emailService;
        private readonly IInstructorService instructorService;
        private readonly ICourseService courseService;


        public HomeController(ILogger<HomeController> logger, IEmailService emailService, IInstructorService instructorService, ICourseService courseService)
        {
            _logger = logger;
            this.emailService = emailService;
            this.instructorService = instructorService;
            this.courseService = courseService;
        }

        public IActionResult Index()
        {
           
            return View("Index");
        }

        public IActionResult About()
        {
            return View("About");
        }

        public IActionResult Help()
        {
            return View("HelpCenter");
        }

        public IActionResult Privacy()
        {
            return View("PrivacyPage");
        }

        public IActionResult Terms()
        {
            return View("Terms");
        }















        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /*
          \ //public async Task  SendingEmailTest()
        //{
        //    var email = new Email
        //    {
        //        ReceiverEmail = "a2004nsakr@gmail.com",
        //        Subject = "Welcome to Learnix!",
        //        Body = "Hi Abdelrahaman, welcome to Learnix "
        //    };

        //    await emailService.SendEmailAsync(email);
           
        //}
        //public IActionResult Test()
        //{
        //    Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        //    string id = IDClaim.Value;

        //    var i = instructorService.GetById(id);

        //    return Content($"FName : {i.Id}");
        //}


         */

    }
}
