using HavestFestival.Managers;
using HavestFestival.Network;
using HavestFestival.Utils;
using UnityEngine;

namespace HavestFestival
{
    class GameManager : Singleton<GameManager>
    {
        public NakamaConnection Connection;
        public MenuManager menuManager;

        void Start()
        {
            Connection.Connect();
            DontDestroyOnLoad(gameObject);
        }

        public void SavePlayerName(string name)
        {
            PlayerPrefs.SetString("playerName", name);
        }

        public void StartHost()
        {
            Connection.CreateMatch();
        }

        public void StartJoin(string matchId)
        {
            Connection.JoinMatch(matchId);
            menuManager.Open(1);
        }
    }
}