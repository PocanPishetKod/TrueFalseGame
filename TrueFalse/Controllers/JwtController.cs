using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public JwtController(JwtService jwtService, PlayerService playerService)
        {
            _jwtService = jwtService;
            _playerService = playerService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Token([Required] JwtRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PlayerName))
            {
                return BadRequest(new { Error = $"{nameof(request.PlayerName)} не должно быть пустым или null" });
            }

            try
            {
                var player = _playerService.CreatePlayer(request.PlayerName);
                var jwt = _jwtService.CreateJwt(player.Id);

                return Ok(new JwtResponse()
                {
                    Token = jwt
                });
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
    }
}
