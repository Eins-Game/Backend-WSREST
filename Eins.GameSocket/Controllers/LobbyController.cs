using Eins.TransportEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eins.GameSocket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LobbyController : ControllerBase
    {
        private readonly ILogger<LobbyController> _logger;
        private readonly List<Lobby> _lobbies;

        public LobbyController(ILogger<LobbyController> logger, List<Lobby> lobbies)
        {
            this._logger = logger;
            this._lobbies = lobbies;
        }
    }
}
