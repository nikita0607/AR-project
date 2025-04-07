using UnityEngine;

namespace Utilities
{
    /// <summary> Компонент для сохранения и восстановления исходной позиции объекта. </summary>
    public class PositionFixer : MonoBehaviour
    {
        /// <summary> Исходная позиция объекта. </summary>
        private Vector3 _originalPosition;

        /// <summary> Сохраняет начальную позицию объекта при старте. </summary>
        private void Start()
        {
            _originalPosition = transform.position;
        }

        /// <summary> Восстанавливает объект в исходную позицию. </summary>
        /// <remarks>
        /// Используется для сброса позиции после перемещений,
        /// например при обработке столкновений или телепортации.
        /// </remarks>
        public void RestorePosition()
        {
            transform.position = _originalPosition;
        }
    }
}