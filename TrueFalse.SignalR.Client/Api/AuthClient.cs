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
    public class AuthClient
    {
        private string _accessToken;

        public AuthClient(string accessToken)
        {
            _accessToken = accessToken;
        }

        /// <summary>
        /// Создает игрока и возвращает токен доступа
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns></returns>
        public async Task<JwtResponse> Token(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                throw new ArgumentNullException(nameof(playerName));
            }

            var httpClient = new HttpClient();

            try
            {
                if (!string.IsNullOrWhiteSpace(_accessToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                }

                var serializedData = JsonConvert.SerializeObject(new JwtRequest() { PlayerName = playerName });
                var response = await httpClient.PostAsync("https://localhost:54613/token", new StringContent(serializedData, Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Что-то не так на сервере");
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new Exception("Попытка получения токена при уже полученном токене");
                }
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var jwtResponse = JsonConvert.DeserializeObject<JwtResponse>(responseData);
                    _accessToken = jwtResponse.Token;
                    return jwtResponse;
                }
                else
                {
                    throw new Exception($"Не ожидаемый статус код - {response.StatusCode}");
                }
            }
            finally
            {
                httpClient.Dispose();
            }
        }
    }
}
