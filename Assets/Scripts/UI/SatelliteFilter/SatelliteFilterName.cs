using System;
using TMPro;
using UnityEngine;

namespace UI.SatelliteFilter
{
    public class SatelliteFilterName : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        private SatelliteInfoController _satelliteInfoController;

        private void Start()
        {
            _satelliteInfoController = SatelliteInfoController.Singleton;
        }

        public void ApplySearch()
        {
            _satelliteInfoController.SatelliteHideFilters.ClearAllFilters();
            _satelliteInfoController.SatelliteHideFilters &= new SatelliteHideFilter(nameFilter: inputField.text);
            _satelliteInfoController.DisableSingleSatelliteFilter();
        }
    }   
}
