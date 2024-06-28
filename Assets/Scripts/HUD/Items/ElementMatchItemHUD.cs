using TMPro;
using UnityEngine;

namespace HavestFestival.HUD.Item
{
    class ElementMatchItemHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private TextMeshProUGUI displayStatus;

        public void Init(string name, string status)
        {
            displayName.text = name;
            displayStatus.text = status;
        }

        public void SetStatus(string status) => displayStatus.text = status;
    }
}