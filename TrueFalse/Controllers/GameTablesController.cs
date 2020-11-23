using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueFalse.Application.Services;

namespace TrueFalse.Controllers
{
    [Route("gametables")]
    [ApiController]
    public class GameTablesController : ControllerBase
    {
        private readonly GameTableService _gameTableService;

        public GameTablesController(GameTableService gameTableService)
        {
            _gameTableService = gameTableService;
        }

        [HttpGet("{pageNum}/{perPage}")]
        [Authorize]
        public IActionResult GetGameTables(int pageNum, int perPage)
        {
            if (pageNum <= 0 || perPage <= 0)
            {
                return BadRequest();
            }

            var result = _gameTableService.GetGameTables(pageNum, perPage);
            return Ok(result);
        }
    }
}
