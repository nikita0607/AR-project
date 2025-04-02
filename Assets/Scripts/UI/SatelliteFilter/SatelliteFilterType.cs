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

        private void Start()
        {
            _satelliteInfoController = SatelliteInfoController.Singleton;
            for (int i = 1; i < Enum.GetNames(typeof(SatelliteType)).Length; i++)
            {
                var button = Instantiate(_button, parent: transform);
                int typeIndex = i;
                
                button.name = ((SatelliteType)typeIndex).ToString();
                button.GetComponentInChildren<TextMeshProUGUI>().text = ((SatelliteType)typeIndex).ToString();
                
                button.onClick.AddListener(() => ApplySearch((SatelliteType)typeIndex));
            }

        }

        private void ApplySearch(SatelliteType satelliteType)
        {
            Debug.Log("ApplySearch " +  satelliteType);
            _satelliteInfoController.SatelliteHideFilters.ClearAllFilters();
            _satelliteInfoController.SatelliteHideFilters &= new SatelliteHideFilter(satelliteType: satelliteType);
            _satelliteInfoController.DisableSingleSatelliteFilter();
        }
    }   
}