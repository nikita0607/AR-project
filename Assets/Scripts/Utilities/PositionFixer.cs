using UnityEngine;

namespace Utilities
{
    /// <summary> Компонент для сохранения и восстановления исходной позиции объекта. </summary>
    public class PositionFixer : MonoBehaviour
    {
        /// <summary> Исходная позиция объекта. </summary>
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private Transform _parentTransform;

        private bool _isInitialized = false;

        /// <summary> Сохраняет начальную позицию объекта при старте. </summary>
        private void Start()
        {
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
            _parentTransform = transform.parent;
            _isInitialized = true;
            
            Debug.Log(_originalPosition.y + " " + name);
        }

        /// <summary> Восстанавливает объект в исходную позицию. </summary>
        /// <remarks>
        /// Используется для сброса позиции после перемещений,
        /// например при обработке столкновений или телепортации.
        /// </remarks>
        public void RestorePosition()
        {
            if (!_isInitialized) return;
            // transform.parent = null;
            transform.parent = _parentTransform;
            transform.rotation = _originalRotation;
            transform.position = _originalPosition;
        }
        
        
    }
}