using UnityEngine;

namespace Utilities
{
    /// <summary> Компонент для калибровки AR-позиции объекта. </summary>
    public class ARCalibrating : MonoBehaviour
    {
        /// <summary> Коэффициент калибровки для точной настройки. </summary>
        [SerializeField] private float calibratoinCoef = 1f;

        /// <summary> Текущие калибровочные значения по осям. </summary>
        private float _xValue = 0f;
        private float _yValue = 0f; 
        private float _zValue = 0f;

        /// <summary> Устанавливает калибровку по оси X. </summary>
        /// <param name="value"> Входное значение вращения. </param>
        public void SetXRotation(float value)
        {
            Debug.Log($"Calibration X input: {value}");
            value *= calibratoinCoef;

            ClearCalibration();
            _xValue = value;
            UpdateCalibration();
        }

        /// <summary> Устанавливает калибровку по оси Y. </summary>
        /// <param name="value"> Входное значение вращения. </param>
        public void SetYRotation(float value)
        {
            value *= calibratoinCoef;
            
            ClearCalibration();
            _yValue = value;
            UpdateCalibration();
        }

        /// <summary> Устанавливает калибровку по оси Z. </summary>
        /// <param name="value"> Входное значение вращения. </param>
        public void SetZRotation(float value)
        {
            value *= calibratoinCoef;
            
            ClearCalibration();
            _zValue = value;
            UpdateCalibration();
        }

        /// <summary> Сбрасывает текущую калибровку. </summary>
        private void ClearCalibration()
        {
            Vector3 eulerAngles = transform.localEulerAngles - new Vector3(_xValue, _yValue, _zValue); 
            transform.localRotation = Quaternion.Euler(eulerAngles);
        }

        /// <summary> Применяет текущие калибровочные значения. </summary>
        private void UpdateCalibration()
        {
            Vector3 eulerAngles = transform.localEulerAngles + new Vector3(_xValue, _yValue, _zValue); 
            transform.localRotation = Quaternion.Euler(eulerAngles);
        }
    }
}