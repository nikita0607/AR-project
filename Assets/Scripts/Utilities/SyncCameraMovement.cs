using UnityEngine;

namespace Utilities
{
    public class SyncCameraMovement : MonoBehaviour
    {
        public Camera sourceCamera; // Исходная камера, которую нужно копировать
        private Camera _targetCamera;

        [Header("Настройки синхронизации")]
        [SerializeField] private bool syncPosition = true;
        [SerializeField] private bool syncRotation = true;
        [SerializeField] private bool syncFOV = true;
        [SerializeField] private bool syncProjection = true;
        
        private MedianFilterVector3 _medianFilter = new();
        
        private Camera _camera;

        void Start()
        {
            _targetCamera = GetComponent<Camera>();
            _camera = GetComponent<Camera>();
        }

        void LateUpdate()
        {
            if (sourceCamera == null) return;
            _camera.enabled = sourceCamera.isActiveAndEnabled;

            // Синхронизация позиции и поворота
            if (syncPosition) 
                transform.position = _medianFilter.Filter(sourceCamera.transform.position);

            if (syncRotation) 
                transform.rotation = sourceCamera.transform.rotation;

            // Синхронизация параметров камеры
            if (syncFOV) 
                _targetCamera.fieldOfView = sourceCamera.fieldOfView;

            if (syncProjection)
            {
                _targetCamera.orthographic = sourceCamera.orthographic;
                _targetCamera.orthographicSize = sourceCamera.orthographicSize;
                _targetCamera.nearClipPlane = sourceCamera.nearClipPlane;
                _targetCamera.farClipPlane = sourceCamera.farClipPlane;
            }
        }
    }
}