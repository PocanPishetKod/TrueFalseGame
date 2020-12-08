using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.SignalR.Client.Api
{
    public class GameTablesClient : IDisposable
    {
        private readonly string _accessToken;
        private readonly HttpClient _httpClient;
        private bool _isDisposed;

        public GameTablesClient(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            _accessToken = accessToken;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            _isDisposed = false;
        }

        /// <summary>
        /// Постранично возвращает игровые столы
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public async Task<List<GameTableDto>> GetGameTables(int pageNum, int perPage)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Ресурсы объекта были освобождены");
            }

            if (pageNum <= 0 || perPage <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var response = await _httpClient.GetAsync($"https://localhost:54613/gametables/{pageNum}/{perPage}");

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new Exception("Токен был отвергнут сервером");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new Exception("Не правильный запрос");
            }
            else
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<GameTableDto>>(responseData);
                return result;
            }
        }

        public void Dispose()
        {
            if (!_isDisposed && _httpClient != null)
            {
                _httpClient.Dispose();
                _isDisposed = true;
            }
        }
    }
}
