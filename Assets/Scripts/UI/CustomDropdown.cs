using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CustomDropdown : MonoBehaviour

    {
        [SerializeField] private GameObject[] _objects;
        private TMP_Dropdown _dropdown;
        private int _currentIndex;

        private void Awake()
        {
            _dropdown =  GetComponent<TMP_Dropdown>();
        }

        private void Start()
        {
            _currentIndex = 0;
            
            var dropdownValues = new List<string>();
            foreach (var obj in _objects)
                dropdownValues.Add(obj.name);
            
            _dropdown.ClearOptions();
            _dropdown.AddOptions(dropdownValues);
            _dropdown.onValueChanged.AddListener(DropdownValueChange);
        }

        private void DropdownValueChange(int index)
        {
            _objects[_currentIndex].SetActive(false);
            _objects[index].SetActive(true);
            _currentIndex = index;
        }
    }
}