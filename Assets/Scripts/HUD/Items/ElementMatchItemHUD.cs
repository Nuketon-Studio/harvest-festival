using TMPro;
using UnityEngine;

namespace HavestFestival.HUD.Item
{
    class ElementMatchItemHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displayName;

        public void Init(string name)
        {
            displayName.text = name;
        }
    }
}