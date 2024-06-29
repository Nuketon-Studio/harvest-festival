using System;
using Nakama;
using UnityEngine;

namespace HarvestFestival.Managers
{
    class EventManager : MonoBehaviour
    {
        #region Network Match Events
        public event Action<IMatchmakerMatched> OnReceivedMatchmakerMatched;
        public void ReceivedMatchmakerMatchedEvent(IMatchmakerMatched value)
        {
            if (OnReceivedMatchmakerMatched != null) OnReceivedMatchmakerMatched(value);
        }

        public event Action<IMatchPresenceEvent> OnReceivedMatchPresence;
        public void ReceivedMatchPresenceEvent(IMatchPresenceEvent value)
        {
            if (OnReceivedMatchPresence != null) OnReceivedMatchPresence(value);
        }

        public event Action<IStatusPresenceEvent> OnReceivedStatusPresence;
        public void ReceivedStatusPresenceEvent(IStatusPresenceEvent value)
        {
            if (OnReceivedStatusPresence != null) OnReceivedStatusPresence(value);
        }
        
        public event Action<IMatchState> OnReceivedMatchState;
        public void ReceivedMatchStateEvent(IMatchState value)
        {
            if (OnReceivedMatchState != null) OnReceivedMatchState(value);
        }
        #endregion
    }
}