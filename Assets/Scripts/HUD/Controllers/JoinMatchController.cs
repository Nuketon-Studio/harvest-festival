using System.Collections.Generic;
using HavestFestival.HUD.Item;
using Nakama;
using UnityEngine;

namespace HavestFestival.HUD.Controllers
{
    class JoinMatchController : MonoBehaviour
    {
        [SerializeField] private GameObject itemMatchPrefab;
        [SerializeField] private GameObject containerListPrefab;

        private List<GameObject> _instances = new List<GameObject>();

        void Start()
        {            
            GetMatches();

            GameManager.Instance.Connection.Socket.ReceivedMatchmakerMatched += OnReceivedMatchmakerMatched;
        }

        private async void GetMatches()
        {
            _instances.ForEach(a => Destroy(a));
            _instances.Clear();

            var minPlayers = 2;
            var maxPlayers = 8;
            var limit = 10;
            var authoritative = true;
            var label = "";
            var query = "";
            var result = await GameManager.Instance.Connection.Client.ListMatchesAsync(GameManager.Instance.Connection.Session, minPlayers, maxPlayers, limit, authoritative, label, query);

            foreach (var match in result.Matches)
            {
                Debug.LogFormat("{0}: {1}/10 players", match.Label, match.Size);
                var instance = Instantiate(itemMatchPrefab);
                instance.transform.SetParent(containerListPrefab.transform);

                instance.GetComponent<ElementMatchItemHUD>().Init(match.Label);

                _instances.Add(instance);
            }
        }

        private void OnReceivedMatchmakerMatched(IMatchmakerMatched match) => GetMatches();
    }
}