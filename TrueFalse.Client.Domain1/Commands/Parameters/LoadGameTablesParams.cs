﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Client.Domain.Commands.Parameters
{
    public class LoadGameTablesParams
    {
        public int PageNum { get; set; }

        public int PerPage { get; set; }
    }
}
