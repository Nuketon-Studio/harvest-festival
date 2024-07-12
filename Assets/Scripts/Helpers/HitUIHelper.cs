using HarvestFestival.Helpers;
using UnityEngine;

namespace HarvestFestival.Helpers
{
    class UIHelper : MonoBehaviour
    {
        public static void Hit(Transform transform, float timeToDestroy = .7f)
        {
            var ui = PrefabHelper.Load("UI/UIHitPrefab");
            var instance = Instantiate(ui);

            instance.transform.position = transform.position + Vector3.up * 4;
            instance.transform.LookAt(Camera.main.transform);

            Destroy(instance, timeToDestroy);
        }
    }
}