using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.Controllers.Dtos
{
    public class JwtRequest
    {
        [Required]
        public string PlayerName { get; set; }
    }
}
