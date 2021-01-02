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
                var serializedData = JsonConvert.SerializeObject(new JwtRequest() { PlayerName = playerName });
                var response = await httpClient.PostAsync("https://localhost:44307/token", new StringContent(serializedData, Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Что-то не так на сервере");
                }
                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var jwtResponse = JsonConvert.DeserializeObject<JwtResponse>(responseData);
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

        /// <summary>
        /// Отправляет запрос на проверку токена
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckToken(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var httpClient = new HttpClient();

            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await httpClient.GetAsync("https://localhost:44307/token/check");
                return response.IsSuccessStatusCode;
            }
            finally
            {
                httpClient.Dispose();
            }
        }
    }
}
