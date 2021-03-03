using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Exceptions;
using TrueFalse.Client.Domain.Interfaces;
using TrueFalse.Client.Domain.Models.Cards;
using TrueFalse.Client.Domain.Models.GameTables;
using TrueFalse.Client.Domain.Models.Moves;
using TrueFalse.Client.Domain.Models.Players;
using TrueFalse.Client.Domain.Services;
using TrueFalse.SignalR.Client.Api;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.Client.Domain.ViewModels
{
    public class GameTableViewModel : BaseViewModel
    {
        private readonly IStateService _stateService;
        private readonly INavigator _navigator;
        private readonly IDispatcher _dispatcher;
        private readonly IMainHubApi _mainHubApi;
        private readonly IBlockUIService _blockUIService;

        public GameTable GameTable => _stateService.GetGameTable();

        public GameTableViewModel(IStateService stateService, INavigator navigator, IDispatcher dispatcher, IMainHubApi mainHubApi, IBlockUIService blockUIService)
        {
            _stateService = stateService;
            _navigator = navigator;
            _dispatcher = dispatcher;
            _mainHubApi = mainHubApi;
            _blockUIService = blockUIService;

            _mainHubApi.GameStarted += OnGameStarted;
            _mainHubApi.FirstMoveMade += OnFirstMoveMade;
            _mainHubApi.PlayerJoined += OnPlayerJoined;
            _mainHubApi.BeliveMoveMade += OnBeliveMoveMade;
            _mainHubApi.DontBeliveMoveMade += OnDontBeliveMoveMade;
        }

        private void OnGameStarted(OnGameStartedParams @params)
        {
            if (@params == null)
            {
                return;
            }

            GameTable.StartGame(@params.MoverId, @params.PlayerCardsInfo);
        }

        private void OnFirstMoveMade(OnFirstMoveMadeParams @params)
        {
            if (@params == null)
            {
                return;
            }

            var mover = GameTable.Players.FirstOrDefault(gp => gp.Player.Id == @params.MoverId);
            if (mover == null)
            {
                return;
            }

            var nextMover = GameTable.Players.FirstOrDefault(gp => gp.Player.Id == @params.NextMoverId);
            if (nextMover == null)
            {
                return;
            }

            var move = new FirstMove(mover.Player, @params.CardIds.Select(c => new PlayingCard() { Id = c }).ToList())
            {
                Rank = (PlayingCardRank)@params.Rank
            };

            GameTable.MakeFirstMove(move, nextMover.Player.Id);
            GameTable.SetNextPossibleMoves(@params.NextPossibleMoves);
        }

        private void OnBeliveMoveMade(OnBeliveMoveMadeParams @params)
        {

        }

        private void OnDontBeliveMoveMade(OnDontBeliveMoveMadeParams @params)
        {

        }

        private void OnPlayerJoined(OnPlayerJoinedParams @params)
        {
            if (@params == null)
            {
                return;
            }

            if (@params.GameTableId != GameTable.Id)
            {
                return;
            }

            GameTable.JoinPlayer(new Player() { Id = @params.Player.Id, Name = @params.Player.Name }, @params.PlaceNumber);
        }

        public void StartGame()
        {
            if (GameTable.IsStarted)
            {
                throw new TrueFalseGameException("Игра уже идет");
            }

            if (GameTable.Owner.Id == _stateService.GetSavedPlayer().Id)
            {
                _blockUIService.StartBlocking();

                _mainHubApi.StartGame(new StartGameParams())
                    .Then((response) =>
                    {
                        if (response.Succeeded)
                        {
                            _dispatcher.Invoke(() =>
                            {
                                GameTable.StartGame(response.MoverId.Value, response.PlayerCardsInfo);
                            });
                        }
                    })
                    .Finally(() =>
                    {
                        _dispatcher.Invoke(() =>
                        {
                            _blockUIService.StopBlocking();
                        });
                    });
            } 
        }

        public void MakeFirstMove()
        {
            if (_stateService.GetSavedPlayer().Id != GameTable.CurrentGame?.CurrentMover.Id)
            {
                return;
            }

            if (_stateService.FirstMove == null || !_stateService.FirstMove.IsValid)
            {
                return;
            }

            if (!GameTable.CanMakeMove(MoveType.FirstMove))
            {
                return;
            }

            _blockUIService.StartBlocking();

            _mainHubApi.MakeFirstMove(new MakeFirstMoveParams()
            {
                Rank = (int)_stateService.FirstMove.Rank,
                CardIds = _stateService.FirstMove.SelectedCards.Select(c => c.Id).ToList()
            })
                .Then(response =>
                {
                    if (response.Succeeded)
                    {
                        GameTable.MakeFirstMove(_stateService.FirstMove, response.NextMoverId.Value);
                    }
                })
                .Finally(() =>
                {
                    _dispatcher.Invoke(() =>
                    {
                        _blockUIService.StopBlocking();
                    });
                });
        }

        public void MakeBeliveMove()
        {
            if (_stateService.GetSavedPlayer().Id != GameTable.CurrentGame?.CurrentMover.Id)
            {
                return;
            }

            if (_stateService.BeliveMove == null || !_stateService.BeliveMove.IsValid)
            {
                return;
            }

            if (!GameTable.CanMakeMove(MoveType.BeliveMove))
            {
                return;
            }

            _blockUIService.StartBlocking();

            _mainHubApi.MakeBeliveMove(new MakeBeliveMoveParams()
            {
                SelectedCardId = _stateService.BeliveMove.SelectedCard.Id
            })
                .Then((response) =>
                {
                    if (response.Succeeded)
                    {
                        _dispatcher.Invoke(() =>
                        {
                            if (response.LoserId == _stateService.GetSavedPlayer().Id)
                            {
                                GameTable.MakeBeliveMove(_stateService.BeliveMove, response.NextMoverId.Value,
                                    response.LoserId.Value, response.TakedLoserCards.Select(c => new PlayingCard()
                                {
                                    Id = c.Id,
                                    Rank = (PlayingCardRank)(int)c.Rank,
                                    Suit = (PlayingCardSuit)(int)c.Suit
                                }).ToList());
                            }
                            else
                            {
                                GameTable.MakeBeliveMove(_stateService.BeliveMove, response.NextMoverId.Value, response.LoserId.Value,
                                    response.HiddenTakedLoserCards.Select(c => new PlayingCard()
                                    {
                                        Id = c
                                    }).ToList());
                            }
                        });
                    }
                })
                .Finally(() =>
                {
                    _dispatcher.Invoke(() => _blockUIService.StartBlocking());
                });
        }

        public void MakeDontBeliveMove()
        {

        }

        public void Leave()
        {
            _mainHubApi.LeaveFromGameTable(new LeaveFromGameTableParams())
                .Then((response) =>
                {
                    if (response.Succeeded)
                    {
                        GameTable.LeavePlayer(_stateService.GetSavedPlayer());

                        if (GameTable.IsInvalid)
                        {
                            _stateService.SetGameTable(null);
                        }

                        _dispatcher.Invoke(() =>
                        {
                            Navigate<MainMenuViewModel>();
                        });
                    }
                });
        }

        public override Task Navigate<TViewModel>()
        {
            _navigator.Navigate<TViewModel>();
            return Task.CompletedTask;
        }
    }
}
