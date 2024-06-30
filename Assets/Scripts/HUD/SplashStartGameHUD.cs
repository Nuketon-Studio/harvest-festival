using UnityEngine;

namespace HarvestFestival.HUD
{
    class SplashStartGame : MonoBehaviour
    {
        void Start()
        {
            Destroy(gameObject, (GameManager.Instance.GameSettings.timeLoadBeforeStartGameplay / 1000) + .5f);
        }
    }
}