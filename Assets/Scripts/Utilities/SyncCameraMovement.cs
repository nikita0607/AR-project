using UnityEngine;

namespace Utilities
{
    /// <summary> Компонент для синхронизации параметров камеры с исходной камерой. </summary>
    public class SyncCameraMovement : MonoBehaviour
    {
        /// <summary> Исходная камера для синхронизации параметров. </summary>
        public Camera sourceCamera;

        /// <summary> Флаг синхронизации позиции камеры. </summary>
        [Header("Настройки синхронизации")]
        [SerializeField] private bool syncPosition = true;
        
        /// <summary> Флаг синхронизации поворота камеры. </summary>
        [SerializeField] private bool syncRotation = true;
        
        /// <summary> Флаг синхронизации поля зрения камеры. </summary>
        [SerializeField] private bool syncFOV = true;
        
        /// <summary> Флаг синхронизации типа проекции камеры. </summary>
        [SerializeField] private bool syncProjection = true;

        /// <summary> Максимальное допустимое отклонение при фильтрации. </summary>
        [Header("Настройки фильтрации")] 
        [SerializeField] private float filterDelta = 0.1f;
        
        /// <summary> Размер окна для медианного фильтра. </summary>
        [SerializeField] private int windowSize = 5;
        [SerializeField] private float lowpassK = 0.1f;
        
        /// <summary> Целевая камера для синхронизации. </summary>
        private Camera _targetCamera;
        
        /// <summary> Фильтр для сглаживания позиции камеры. </summary>
        private MedianFilterVector3 _medianFilter = new();
        private LowpassFIlter _lowpassFilter = new();

        /// <summary> Инициализирует компонент, проверяя наличие камеры. </summary>
        private void Start()
        {
            _targetCamera = GetComponent<Camera>();
            if (_targetCamera == null)
            {
                Debug.LogError("SyncCameraMovement требует компонент Camera на этом объекте");
                enabled = false;
            }
        }

        /// <summary> Позднее обновление для синхронизации параметров камеры. </summary>
        private void LateUpdate()
        {
            if (sourceCamera == null) return;

            // Синхронизация активности камеры
            _targetCamera.enabled = sourceCamera.isActiveAndEnabled;

            SyncTransform();
            SyncCameraParameters();
        }

        /// <summary> Синхронизирует позицию и поворот камеры. </summary>
        private void SyncTransform()
        {
            if (syncPosition)
            {
                Vector3 filteredPosition = _lowpassFilter.Filter(
                    sourceCamera.transform.position,
                    lowpassK,
                    filterDelta
                );
                transform.position = filteredPosition;
            }

            if (syncRotation)
            {
                transform.rotation = sourceCamera.transform.rotation;
            }
        }

        /// <summary> Синхронизирует параметры камеры. </summary>
        private void SyncCameraParameters()
        {
            if (syncFOV)
            {
                _targetCamera.fieldOfView = sourceCamera.fieldOfView;
            }

            if (syncProjection)
            {
                _targetCamera.orthographic = sourceCamera.orthographic;
                if (sourceCamera.orthographic)
                {
                    _targetCamera.orthographicSize = sourceCamera.orthographicSize;
                }
                _targetCamera.nearClipPlane = sourceCamera.nearClipPlane;
                _targetCamera.farClipPlane = sourceCamera.farClipPlane;
            }
        }
    }
}