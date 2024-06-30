using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HarvestFestival.Entities.Network;
using HarvestFestival.Managers;
using HarvestFestival.Network;
using HarvestFestival.SO;
using HarvestFestival.Types;
using HarvestFestival.Utils;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HarvestFestival
{
    class GameManager : Singleton<GameManager>
    {
        public NakamaConnection Connection;
        public MenuManager menuManager;
        public EventManager eventManager;
        public MatchManager matchManager;

        [Header("Configs")]
        public List<CharacterSO> Characters;

        public string UserId { get; private set; }
        public CharacterSO CharacterStats { get; private set; }

        public void SetUserId(string userId) => UserId = userId;
        public void SetCharacterStats(CharacterSO character) => CharacterStats = character;

        #region Validators
        public bool IsLocal(string userId) => userId == UserId;
        #endregion

        #region Flow Game
        public async void StartGame(List<UserLobbyNetworkEntity> players)
        {
            SceneManager.LoadScene((int)SceneType.GamePlay);

            // add splash scene

            await Task.Delay(2000);
            matchManager.AddPlayers(players);
        }
        #endregion

        #region System
        public async void Exit()
        {
            await Connection.Client.DeleteAccountAsync(Connection.Session);

            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
        #endregion

        #region NetworkEvents
        private void OnReceivedMatchState(IMatchState matchState)
        {
            var jsonUtf8 = Encoding.UTF8.GetString(matchState.State);

            switch (matchState.OpCode)
            {
                default:
                    break;
            }
        }
        #endregion

        #region Unity Event
        void Start()
        {
            Connection.Connect();
            eventManager.OnReceivedMatchState += OnReceivedMatchState;

            DontDestroyOnLoad(gameObject);
        }
        #endregion
    }
}