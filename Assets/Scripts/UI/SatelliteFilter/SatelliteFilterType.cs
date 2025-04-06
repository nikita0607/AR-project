using System;
using TLE.Filters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SatelliteFilter
{
    public class SatelliteFilterType: MonoBehaviour
    {
        private SatelliteInfoController _satelliteInfoController;
        
        [SerializeField] private Button _button;
        [SerializeField] private GameObject content;
        
        private SatelliteHideFilter _satelliteFilter;

        private void Start()
        {
            _satelliteInfoController = SatelliteInfoController.Singleton;
            for (int i = 1; i < Enum.GetNames(typeof(SatelliteType)).Length; i++)
            {
                var button = Instantiate(_button, parent: content.transform);
                int typeIndex = i;
                
                button.name = ((SatelliteType)typeIndex).ToString();
                button.GetComponentInChildren<TextMeshProUGUI>().text = ((SatelliteType)typeIndex).ToString();
                
                button.onClick.AddListener(() => ApplySearch((SatelliteType)typeIndex));
            }
        }

        private void ApplySearch(SatelliteType satelliteType)
        {
            Debug.Log("ApplySearch " +  satelliteType);
            if (_satelliteFilter != null)
                _satelliteInfoController.SatelliteHideFilters -= _satelliteFilter;
            
            _satelliteFilter = new SatelliteHideFilter(satelliteType: satelliteType);
            _satelliteInfoController.SatelliteHideFilters &= _satelliteFilter;
            
            _satelliteInfoController.TryApplySatelliteFilter();
        }
    }   
}