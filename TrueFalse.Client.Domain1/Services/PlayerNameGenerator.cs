using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Client.Domain.Services
{
    public static class PlayerNameGenerator
    {
        private const string _baseName = "Player";

        public static string Generate()
        {
            var random = new Random();
            return $"{_baseName}{random.Next(1, 1000000000)}";
        }
    }
}
