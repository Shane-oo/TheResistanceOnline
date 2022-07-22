using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.BusinessLogic.Timers;
using TheResistanceOnline.SocketServer.HubConfigurations;

namespace TheResistanceOnline.SocketServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController: ControllerBase
    {
        #region Fields

        private readonly IHubContext<TheResistanceHub> _hub;
        private readonly ITimerService _timerService;

        #endregion

        #region Construction

        public MessagesController(IHubContext<TheResistanceHub> hub, ITimerService timerService)
        {
            _hub = hub;
            _timerService = timerService;

        }

        [HttpGet]
        public IActionResult Get()
        {
            if (!_timerService.CheckTimerHasStarted())
            {
                _timerService.PrepareTimer(() => _hub.Clients.All.SendAsync("TransferMessageData", "Hello Clients - from Server"));
                
            }
            return Ok(new { Message = "Request Completed" });
        }
        #endregion
    }
}
