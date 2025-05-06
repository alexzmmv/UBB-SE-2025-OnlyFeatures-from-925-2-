using Microsoft.AspNetCore.Mvc;
using WinUIApp.WebAPI.Services.DummyServices;

namespace WinUIApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("is-admin")]
        public IActionResult IsAdmin([FromQuery] int userId)
        {
            return Ok(_adminService.IsAdmin(userId));
        }

        [HttpPost("send-notification")]
        public IActionResult SendNotification([FromBody] SendNotificationRequest request)
        {
            _adminService.SendNotificationFromUserToAdmin(
                request.SenderUserId,
                request.UserModificationRequestType,
                request.UserModificationRequestDetails);
            return Ok();
        }
    }

    public class SendNotificationRequest
    {
        public int SenderUserId { get; set; }
        public string UserModificationRequestType { get; set; } = string.Empty;
        public string UserModificationRequestDetails { get; set; } = string.Empty;
    }
} 