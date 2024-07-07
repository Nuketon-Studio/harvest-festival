using System.Reflection.Emit;
using System.Threading.Tasks;
using Nakama.TinyJson;
using UnityEngine;

namespace HarvestFestival.Helpers
{
    static class PrefabHelper
    {
        public static GameObject Load(string path) => Resources.Load<GameObject>(path);
    }
}