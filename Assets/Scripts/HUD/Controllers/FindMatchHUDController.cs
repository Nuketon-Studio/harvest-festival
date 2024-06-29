using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarvestFestival.Entities;
using HarvestFestival.Entities.Network;
using HarvestFestival.Helpers;
using HarvestFestival.HUD.Item;
using HarvestFestival.Types;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

namespace HarvestFestival.HUD.Controllers
{
    class FindMatchHUDController : MonoBehaviour
    {
        [SerializeField] private GameObject playerItemListPrefab;
        [SerializeField] private GameObject containerListPrefab;
        [SerializeField] private GameObject displayWaitAnotherPlayerPrefab;

        private List<PlayerMatch> _playerMatchs = new List<PlayerMatch>();
        private bool _isReady = false;
        private string _username;

        void Start()
        {
            GameManager.Instance.eventManager.OnReceivedMatchmakerMatched += OnReceivedMatchmakerMatched;
            GameManager.Instance.eventManager.OnReceivedMatchPresence += OnReceivedMatchPresence;
            GameManager.Instance.eventManager.OnReceivedMatchState += OnReceivedMatchState;
        }

        #region Action Buttons
        public void SetUsername(string name) => _username = name;

        public async void FindMatch()
        {
            await GameManager.Instance.Connection.FindMatch(_username);
            GameManager.Instance.SetUserId(GameManager.Instance.Connection.Session.UserId);
        }

        public async void ToggleStatus()
        {
            _isReady = !_isReady;

            var status = new MatchStatusEntityNetwork
            {
                isReady = _isReady,
                display = StatusMessage()
            };

            ChangeStatusUI(GameManager.Instance.UserId, status);

            await CommunicationHelper.Send<MatchStatusEntityNetwork>(OpCodeType.MATCH_STATUS, status);
        }
        #endregion

        private string StatusMessage()
        {
            if (_isReady) return "Ready!";

            return "Wait...";
        }


        private void ChangeStatusUI(string userId, MatchStatusEntityNetwork status)
        {
            var player = _playerMatchs.Find(f => f.player.Id == userId);

            if (player != null)
            {
                player.instance.GetComponent<ElementMatchItemHUD>().SetStatus(status.display);
                player.isReady = status.isReady;
            }
        }

        private async void CheckAllPlayerIsReady()
        {
            var isAllReady = _playerMatchs.All(a => a.isReady);

            if (isAllReady)
            {
                var data = new StartGameEntityNetwork { sceneIndex = 1 };

                GameManager.Instance.ChangeScene(data.sceneIndex); // change localhost
                await CommunicationHelper.Send<StartGameEntityNetwork>(OpCodeType.START_GAME, data);
            }
        }

        #region network Event
        private void OnReceivedMatchState(IMatchState matchState)
        {
            switch (matchState.OpCode)
            {
                case OpCodeType.MATCH_STATUS:
                    var stateJson = Encoding.UTF8.GetString(matchState.State);
                    var state = JsonParser.FromJson<MatchStatusEntityNetwork>(stateJson);

                    ChangeStatusUI(matchState.UserPresence.UserId, state);

                    CheckAllPlayerIsReady();
                    break;
            }
        }

        private void OnReceivedMatchPresence(IMatchPresenceEvent match)
        {
            GameManager.Instance.SetMatchId(match.MatchId);
            displayWaitAnotherPlayerPrefab.gameObject.SetActive(false);
        }

        private async void OnReceivedMatchmakerMatched(IMatchmakerMatched match)
        {
            await GameManager.Instance.Connection.Socket.JoinMatchAsync(match);

            List<string> usersIds = new List<string>();

            foreach (var user in match.Users)
            {
                usersIds.Add(user.Presence.UserId);
            }

            var accounts = await GameManager.Instance.Connection.Client.GetUsersAsync(
                GameManager.Instance.Connection.Session,
                usersIds
            );

            foreach (IApiUser user in accounts.Users)
            {
                var instance = Instantiate(playerItemListPrefab);
                instance.transform.SetParent(containerListPrefab.transform);

                instance.GetComponent<ElementMatchItemHUD>().Init(user.DisplayName, StatusMessage());

                _playerMatchs.Add(new PlayerMatch(user, "", instance));
            }
        }
        #endregion
    }

    class PlayerMatch
    {
        public GameObject instance;
        public IApiUser player;
        public string sessionId;
        public bool isReady = false;

        public PlayerMatch(IApiUser user, string _sessionId, GameObject _instance)
        {
            instance = _instance;
            player = user;
            sessionId = _sessionId;
        }
    }
}