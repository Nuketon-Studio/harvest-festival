using Nakama;
using UnityEngine;

namespace HarvestFestival.SO
{
    [CreateAssetMenu(fileName = "GameSettingSO", menuName = "ScriptableObjects/Game Settings", order = 0)]
    public class GameSettingSO : ScriptableObject
    {
       [Header("General")]
       public int timeLoadBeforeStartGameplay = 2000; // time in miliseconds
    }
}