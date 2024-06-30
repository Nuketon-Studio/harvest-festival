using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarvestFestival.Entities;
using HarvestFestival.Entities.Network;
using HarvestFestival.Helpers;
using HarvestFestival.HUD.Item;
using HarvestFestival.SO;
using HarvestFestival.Types;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

// TODO -  esse script pode ser dividido em 2 onde teremos um controller e um HUD script
namespace HarvestFestival.HUD.Controllers
{
    class FindMatchHUDController : MonoBehaviour
    {
        [SerializeField] private GameObject playerItemListPrefab;
        [SerializeField] private GameObject containerListPrefab;
        [SerializeField] private GameObject displayWaitAnotherPlayerPrefab;

        private List<UserLobbyNetworkEntity> _playerMatchs = new List<UserLobbyNetworkEntity>();
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

            var status = new MatchStatusNetworkEntity
            {
                isReady = _isReady,
                display = MatchStatusDisplay()
            };

            ChangeStatusUI(GameManager.Instance.UserId, status);

            await NetworkHelper.Send<MatchStatusNetworkEntity>(OpCodeType.MATCH_STATUS, status);
            
            CheckAllPlayerIsReady();
        }
        #endregion

        private string MatchStatusDisplay()
        {
            if (_isReady) return "Ready!";

            return "Wait...";
        }


        private void ChangeStatusUI(string userId, MatchStatusNetworkEntity status)
        {
            var player = _playerMatchs.Find(f => f.account.Id == userId);

            if (player != null)
            {
                player.instance.GetComponent<ElementMatchItemHUD>().SetStatus(status.display);
                player.isReady = status.isReady;
            }
        }

        private void CheckAllPlayerIsReady()
        {
            var isAllReady = _playerMatchs.All(a => a.isReady);

            if (isAllReady) GameManager.Instance.StartGame(_playerMatchs);
        }

        #region network Event
        private void OnReceivedMatchState(IMatchState matchState)
        {
            var state = matchState.State.Length > 0 ? Encoding.UTF8.GetString(matchState.State) : null;

            if (state == null) return;

            switch (matchState.OpCode)
            {
                case OpCodeType.MATCH_STATUS:
                    var data = JsonParser.FromJson<MatchStatusNetworkEntity>(state);

                    ChangeStatusUI(matchState.UserPresence.UserId, data);

                    CheckAllPlayerIsReady();
                    break;
                case OpCodeType.MATCH_UPDATE_CHARACTER:
                    UpdateCharacterNetworkEntity characterData = JsonParser.FromJson<UpdateCharacterNetworkEntity>(state);

                    var player = _playerMatchs.Find(f => f.userId == characterData.userId);

                    if (player is not null) player.characterStats = GameManager.Instance.Characters.Find(f => f.displayName == characterData.characterName);

                    break;
            }
        }

        private void OnReceivedMatchPresence(IMatchPresenceEvent match)
        {
            GameManager.Instance.matchManager.SetMatchId(match.MatchId);
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

                instance.GetComponent<ElementMatchItemHUD>().Init(user.DisplayName, MatchStatusDisplay());

                _playerMatchs.Add(new UserLobbyNetworkEntity
                {
                    account = user,
                    userId = user.Id,
                    instance = instance,
                });
            }

            // add the character stats to player local
            var userLocal = _playerMatchs.Find(f => GameManager.Instance.IsLocal(f.userId));
            userLocal.characterStats = GameManager.Instance.CharacterStats;

            // send information to all player what character this player local
            await NetworkHelper.Send<UpdateCharacterNetworkEntity>(
                OpCodeType.MATCH_UPDATE_CHARACTER,
                new UpdateCharacterNetworkEntity
                {
                    characterName = GameManager.Instance.CharacterStats.displayName,
                    userId = GameManager.Instance.UserId
                }
            );

            // select first player to has host in game
            var firstUser = match.Users.First();

            if(GameManager.Instance.IsLocal(firstUser.Presence.UserId)) GameManager.Instance.SetIsHost(true);
        }
        #endregion
    }
}