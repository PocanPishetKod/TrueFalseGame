using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Players;
using TrueFalse.SignalR.Client.Api;

namespace TrueFalse.Client.Domain.Services
{
    public class AuthService
    {
        private readonly AuthClient _authClient;
        private readonly SaveService _saveService;

        public AuthService(SaveService saveService)
        {
            _authClient = new AuthClient();
            _saveService = saveService;
        }

        /// <summary>
        /// Запрос на аутентификацию
        /// </summary>
        /// <returns></returns>
        public async Task<SavedPlayer> Authenticate()
        {
            var playerData = _saveService.GetPlayerData();

            if (playerData == null)
            {
                var response = await _authClient.Token(PlayerNameGenerator.Generate());
                playerData = new SavedPlayer()
                {
                    Name = response.PlayerName,
                    Token = response.Token,
                    Id = response.PlayerId
                };

                _saveService.SavePlayerData(playerData);
            }
            else
            {
                if (playerData.Token == null)
                {
                    var response = await _authClient.Token(string.IsNullOrWhiteSpace(playerData.Name) ? PlayerNameGenerator.Generate() : playerData.Name);
                    playerData.Token = response.Token;
                    playerData.Name = response.PlayerName;
                    playerData.Id = response.PlayerId;

                    _saveService.SavePlayerData(playerData);
                }
                else
                {
                    if (!await _authClient.CheckToken(playerData.Token))
                    {
                        var response = await _authClient.Token(string.IsNullOrWhiteSpace(playerData.Name) ? PlayerNameGenerator.Generate() : playerData.Name);
                        playerData.Token = response.Token;
                        playerData.Name = response.PlayerName;
                        playerData.Id = response.PlayerId;

                        _saveService.SavePlayerData(playerData);
                    }
                }
            }

            return playerData;
        }
    }
}
