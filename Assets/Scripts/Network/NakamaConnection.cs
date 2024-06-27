using HavestFestival.SO;
using Nakama;
using UnityEngine;

namespace HavestFestival.Network
{
    public class NakamaConnection : MonoBehaviour
    {
        private const string _clientRefName = "nakama.clientId";
        private const string _sessionTokenName = "nakama.SessionToken";

        [SerializeField] private NakamaConnectionSO connectionSO;

        public IClient Client;
        public ISession Session;
        public ISocket Socket;

        private string _ticket;

        public async void Connect()
        {
            Client = new Client(connectionSO.scheme, connectionSO.host, connectionSO.port, connectionSO.serverKey, UnityWebRequestAdapter.Instance);

            var authToken = PlayerPrefs.GetString(_sessionTokenName);

            if (!string.IsNullOrEmpty(authToken))
            {
                var session = Nakama.Session.Restore(authToken);

                if (session.IsExpired)
                {
                    Debug.LogError("Session has expired");
                    return;
                }

                Session = session;
            }

            if (Session == null)
            {
                Session = await Client.AuthenticateDeviceAsync(PlayerPrefs.GetString(_clientRefName));

                PlayerPrefs.SetString(_sessionTokenName, Session.AuthToken);
            }

            Socket = Client.NewSocket(useMainThread: true);
            await Socket.ConnectAsync(Session, true);
        }

        public async void CreateMatch() {
            await Client.UpdateAccountAsync(Session, PlayerPrefs.GetString("playerName"), PlayerPrefs.GetString("playerName"));
            
            var matchName = "Room " + PlayerPrefs.GetString("playerName");
            await Socket.CreateMatchAsync(matchName);
        }

        public async void JoinMatch(string matchId) {
            await Client.UpdateAccountAsync(Session, PlayerPrefs.GetString("playerName"), PlayerPrefs.GetString("playerName"));
            
            await Socket.JoinMatchAsync(matchId);
        }

        #region Unity Events
        void Start()
        {
            if(!PlayerPrefs.HasKey(_clientRefName)) PlayerPrefs.SetString(_clientRefName, SystemInfo.deviceUniqueIdentifier);
        }
        #endregion
    }
}