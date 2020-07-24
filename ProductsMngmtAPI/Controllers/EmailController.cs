using Microsoft.AspNetCore.Mvc;
using ProductsMngmt.ViewModels.DTOs.Email;
using ProductsMngmtAPI.Services.Emails;

namespace ProductsMngmtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        
        /// <summary>
        /// Sends email to customer, that his order was reqistered and to the owner of application
        /// </summary>
        /// <response code="200">Both emails were sent successfully</response>
        /// <response code="400">An error occured trying to send email</response>
        [HttpPost]
        public IActionResult SendEmail(EmailForSentDTO email)
        {
            if(_emailService.SendEmail(email))
                return Ok("Email was sent");
            return BadRequest("Something went wrong");
        }

    }
}