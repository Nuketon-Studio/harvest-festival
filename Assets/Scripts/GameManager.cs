using System.Text;
using HavestFestival.Entities;
using HavestFestival.Managers;
using HavestFestival.Network;
using HavestFestival.Types;
using HavestFestival.Utils;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HavestFestival
{
    class GameManager : Singleton<GameManager>
    {
        public NakamaConnection Connection;
        public MenuManager menuManager;
        public EventManager eventManager;

        public string MatchId { get; private set; }
        public string UserId { get; private set; }

        public void SetMatchId(string matchId) => MatchId = matchId;
        public void SetUserId(string userId) => UserId = userId;

        void Start()
        {
            Connection.Connect();
            eventManager.OnReceivedMatchState += OnReceivedMatchState;


            DontDestroyOnLoad(gameObject);
        }

        public void ChangeScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        public async void Exit()
        {
            await Connection.Client.DeleteAccountAsync(Connection.Session);

            Application.Quit();
        }

        #region NetworkEvents
        private void OnReceivedMatchState(IMatchState matchState)
        {
            var jsonUtf8 = Encoding.UTF8.GetString(matchState.State);

            switch (matchState.OpCode)
            {
                case OpCodeType.START_GAME:
                    StartGameEntity data = JsonParser.FromJson<StartGameEntity>(jsonUtf8);
                    ChangeScene(data.sceneIndex);
                    break;
            }
        }
        #endregion
    }
}