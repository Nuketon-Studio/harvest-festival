using System.Collections.Generic;
using HarvestFestival.HUD.Item;
using HarvestFestival.SO;
using UnityEngine;

namespace HarvestFestival.HUD.Controllers
{
    class CharacterChooseHUDController : MonoBehaviour
    {
        [SerializeField] private GameObject characterItemPrefab;
        [SerializeField] private GameObject containerPrefab;

        private CharacterChooseItemHUD _characterSelected;

        public void SetCharacterSelected(CharacterChooseItemHUD character) {
            _characterSelected?.SelectedToggle();
            _characterSelected = character;
        }

        void Start()
        {
            foreach (var character in GameManager.Instance.Characters)
            {
                var instance = Instantiate(characterItemPrefab);
                instance.transform.SetParent(containerPrefab.transform);

                instance.GetComponent<CharacterChooseItemHUD>().Init(character, SetCharacterSelected);
            }
        }

        public void CharacterSelect() {
            GameManager.Instance.SetCharacterStats(_characterSelected.Character);
        }
    }
}