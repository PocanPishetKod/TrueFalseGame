using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Interfaces;

namespace TrueFalse.Client.ConsoleApp
{
    public class StoreFolderProvider : IStoreFolderPathProvider
    {
        private readonly string _path;

        public StoreFolderProvider(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            _path = path;
        }

        public string ProvidePath()
        {
            return _path;
        }
    }
}
