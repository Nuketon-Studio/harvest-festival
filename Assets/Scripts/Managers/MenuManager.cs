using System.Collections.Generic;
using UnityEngine;

namespace HavestFestival.Managers
{
    class MenuManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> menus;

        private int _current = 0;

        public void Open(int index)
        {
            if (menus.Count > index) {
                menus[_current].SetActive(false);
                menus[index].SetActive(true);

                _current = index;
            }
        }
    }
}