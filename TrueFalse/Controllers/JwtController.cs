using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrueFalse.Application.Services;
using TrueFalse.Auth.Services;
using TrueFalse.Controllers.Dtos;

namespace TrueFalse.Controllers
{
    [Route("token")]
    [ApiController]
    public class JwtController : BaseController
    {
        private readonly JwtService _jwtService;
        private readonly PlayerService _playerService;
        private readonly ILogger<JwtController> _logger;

        public JwtController(JwtService jwtService, PlayerService playerService, ILogger<JwtController> logger)
        {
            _jwtService = jwtService;
            _playerService = playerService;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Token([Required] JwtRequest request)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Forbid();
            }

            try
            {
                var player = _playerService.CreatePlayer(request.PlayerName);
                var jwt = _jwtService.CreateJwt(player.Id);

                return Ok(new JwtResponse()
                {
                    Token = jwt,
                    PlayerId = player.Id,
                    PlayerName = player.Name
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка выдачи токена");
                return InternalServerError();
            }
        }

        [HttpGet("check")]
        [Authorize]
        public IActionResult CheckToken()
        {
            return Ok();
        }
    }
}
