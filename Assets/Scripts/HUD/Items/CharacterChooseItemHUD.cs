using HarvestFestival.HUD.Controllers;
using HarvestFestival.SO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HarvestFestival.HUD.Item
{
    class CharacterChooseItemHUD : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private GameObject selectMarkePrefab;
        [SerializeField] private Image image;

        private bool _isSelected = false;
        private System.Action<CharacterChooseItemHUD> _callback;
        public CharacterSO Character { get; private set; }

        public void Init(CharacterSO character, System.Action<CharacterChooseItemHUD> callback)
        {
            displayName.text = character.displayName;
            Character = character;
            _callback = callback;

            // image.sprite = status;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _callback(this);
            SelectedToggle();
        }

        public void SelectedToggle()
        {
            _isSelected = !_isSelected;

            selectMarkePrefab.SetActive(_isSelected);
        }
    }
}