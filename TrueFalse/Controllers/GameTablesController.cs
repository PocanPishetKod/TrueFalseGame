using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrueFalse.Application.Services;

namespace TrueFalse.Controllers
{
    [Route("gametables")]
    [ApiController]
    public class GameTablesController : BaseController
    {
        private readonly GameTableService _gameTableService;
        private readonly ILogger<GameTablesController> _logger;

        public GameTablesController(GameTableService gameTableService, ILogger<GameTablesController> logger)
        {
            _gameTableService = gameTableService;
            _logger = logger;
        }

        [HttpGet("{pageNum}/{perPage}")]
        [Authorize]
        public IActionResult GetGameTables(int pageNum, int perPage)
        {
            try
            {
                if (pageNum <= 0 || perPage <= 0)
                {
                    return BadRequest();
                }

                var result = _gameTableService.GetGameTables(pageNum, perPage);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка возврата игровых столов");
                return InternalServerError();
            }
        }
    }
}
