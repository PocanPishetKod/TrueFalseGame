using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrueFalse.Controllers
{
    public class BaseController : ControllerBase
    {
        public IActionResult InternalServerError()
        {
            return StatusCode(500, new { Message = "Что-то пошло не так. Попробуйте еще раз" });
        }
    }
}
