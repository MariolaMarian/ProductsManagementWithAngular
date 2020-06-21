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
        [HttpPost]
        public IActionResult SendEmail(EmailForSentDTO email)
        {
            if(_emailService.SendEmail(email))
                return Ok("Email was sent");
            return BadRequest("Something went wrong");
        }

    }
}