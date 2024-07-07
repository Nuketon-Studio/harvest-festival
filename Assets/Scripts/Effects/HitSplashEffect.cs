using UnityEngine;

namespace HarvestFestival.Effects
{
    public class HitSplashEffect : MonoBehaviour
    {
        [SerializeField] private Sprite sprite;

        public void Create(Vector3 direction)
        {
            transform.eulerAngles = direction * -1;
        }
    }
}