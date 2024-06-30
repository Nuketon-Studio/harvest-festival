using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HarvestFestival.Entities.Network;
using HarvestFestival.Managers;
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
        public NetworkManager Connection;
        public MenuManager menuManager;
        public EventManager eventManager;
        public MatchManager matchManager;

        [Header("Configs")]
        public List<CharacterSO> Characters;

        public string UserId { get; private set; }
        public CharacterSO CharacterStats { get; private set; }
        [field: SerializeField] public GameSettingSO GameSettings { get; private set; }

        public bool IsHost { get; private set; } = false;

        public void SetUserId(string userId) => UserId = userId;
        public void SetIsHost(bool isHost) => IsHost = isHost;
        public void SetCharacterStats(CharacterSO character) => CharacterStats = character;

        #region Validators
        public bool IsLocal(string userId) => userId == UserId;
        #endregion

        #region Flow Game
        public async void StartGame(List<UserLobbyNetworkEntity> players)
        {
            SceneManager.LoadScene((int)SceneType.GamePlay);
            matchManager.Init();

            await Task.Delay(GameSettings.timeLoadBeforeStartGameplay);

            matchManager.SetPlayerLobbyInfo(players);
            
            if(IsHost) matchManager.HostAddPlayers(players);
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

        // remover depois ta pra apertar i e remover a conta no servidor
        void Update()
        {
            if (SceneManager.GetActiveScene().buildIndex == 1 && Input.GetKeyDown(KeyCode.I)) Exit();

            if (SceneManager.GetActiveScene().buildIndex == 1 && Input.GetKeyDown(KeyCode.P))
            {
                var camera = GameObject.Find("Camera/Main Camera")?.GetComponent<CameraManager>();
                camera?.TurnToggleCamera();
            }
        }
        #endregion
    }
}