using System.Collections.Generic;
using HavestFestival.HUD.Item;
using Nakama;
using UnityEngine;

namespace HavestFestival.HUD.Controllers
{
    class HostMatchController : MonoBehaviour
    {
        [SerializeField] private GameObject playerItemListPrefab;
        [SerializeField] private GameObject containerListPrefab;

        private List<GameObject> _instances = new List<GameObject>();

        void Start()
        {
            GameManager.Instance.Connection.Socket.ReceivedMatchPresence += OnReceiveMatchesPresences;
        }

        private void OnReceiveMatchesPresences(IMatchPresenceEvent matchPresenceEvent) {
            foreach (var players in matchPresenceEvent.Joins)
            {
                var instance = Instantiate(playerItemListPrefab);
                instance.transform.SetParent(containerListPrefab.transform);

                instance.GetComponent<ElementMatchItemHUD>().Init(players.Username);

                _instances.Add(instance);
            }
        }
    }
}