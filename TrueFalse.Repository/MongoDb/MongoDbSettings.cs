using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Repository.MongoDb
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public static MongoDbSettings Create(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var settings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

            if (string.IsNullOrWhiteSpace(settings.ConnectionString) || string.IsNullOrWhiteSpace(settings.DatabaseName))
            {
                throw new Exception("Не полностью прописана конфигурация для mongoDb");
            }

            return new MongoDbSettings()
            {
                ConnectionString = settings.ConnectionString,
                DatabaseName = settings.DatabaseName
            };
        }
    }
}
