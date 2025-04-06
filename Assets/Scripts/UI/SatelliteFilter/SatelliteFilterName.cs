using TLE.Filters;
using TLE.Info;
using TMPro;
using UnityEngine;

namespace UI.SatelliteFilter
{
    public class SatelliteFilterName : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        private SatelliteInfoController _satelliteInfoController;
        
        private SatelliteHideFilter _satelliteHideFilter;

        private void Start()
        {
            _satelliteInfoController = SatelliteInfoController.Singleton;
        }

        public void ApplySearch()
        {
            if (_satelliteHideFilter != null)
                _satelliteInfoController.SatelliteHideFilters -= _satelliteHideFilter;

            if (inputField.text.Length >= 3)
            {
                _satelliteHideFilter = new SatelliteHideFilter(nameFilter: inputField.text);
                _satelliteInfoController.SatelliteHideFilters &= _satelliteHideFilter;
            }
            
            _satelliteInfoController.TryApplySatelliteFilter();
        }
    }   
}
