using UnityEngine;

namespace Utilities
{
    public class ARCalibrating : MonoBehaviour
    {
        [SerializeField] private float calibratoinCoef;

        private float _xValue = 0f;
        private float _yValue = 0f;
        private float _zValue = 0f;
        
        public void SetXRotation(float value)
        {
            Debug.Log("VAL: "+value);
            value *= calibratoinCoef;

            ClearCalibration();
            _xValue = value;
            _zValue = -value;
            UpdateCalibration();
        }
        
        public void SetYRotation(float value)
        {
            value *= calibratoinCoef;
            
            ClearCalibration();
            _yValue = value;
            UpdateCalibration();
        }
        
        public void SetZRotation(float value)
        {
            value *= calibratoinCoef;
            
            ClearCalibration();
            _zValue = value;
            UpdateCalibration();
        }
        
        
        private void ClearCalibration()
        {
             Vector3 eulerAngles = transform.localEulerAngles - new Vector3(_xValue, _yValue, _zValue); 
             transform.localRotation = Quaternion.Euler(eulerAngles);
        }

        private void UpdateCalibration()
        {
             Vector3 eulerAngles = transform.localEulerAngles + new Vector3(_xValue, _yValue, _zValue); 
             transform.localRotation = Quaternion.Euler(eulerAngles);
        }
    }
}