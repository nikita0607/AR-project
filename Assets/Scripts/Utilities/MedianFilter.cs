using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities
{
    /// <summary> Реализует медианный фильтр для сглаживания значений Vector3. </summary>
    public class MedianFilterVector3
    {
        /// <summary> Окно фильтра (кольцевой буфер значений). </summary>
        private readonly Queue<Vector3> _window = new();
        
        /// <summary> Текущее медианное значение. </summary>
        private Vector3 _centerValue;
        
        /// <summary> Применяет медианный фильтр к входному значению. </summary>
        /// <param name="value"> Входное значение для фильтрации. </param>
        /// <param name="maxDelta"> Максимально допустимое отклонение от медианы. </param>
        /// <param name="windowSize"> Размер окна фильтра (по умолчанию 5). </param>
        /// <returns> Отфильтрованное значение Vector3. </returns>
        /// <remarks>
        /// Алгоритм:
        /// 1. Добавляет значение в окно фильтра
        /// 2. Поддерживает заданный размер окна
        /// 3. Находит медиану по расстоянию от предыдущей медианы
        /// 4. Возвращает медиану, если отклонение в пределах maxDelta, иначе исходное значение
        /// </remarks>
        public Vector3 Filter(Vector3 value, float maxDelta, int windowSize = 5)
        {
            // Добавление нового значения в окно
            _window.Enqueue(value);
            
            // Поддержание размера окна
            if (_window.Count > windowSize)
                _window.Dequeue();
            
            // Сортировка по расстоянию от текущей медианы
            var sorted = _window.OrderBy(v => Vector3.Distance(_centerValue, v));
            
            // Вычисление новой медианы
            _centerValue = sorted.ElementAt(_window.Count / 2);
            
            // Проверка на допустимое отклонение
            return Mathf.Abs(_centerValue.magnitude - value.magnitude) <= maxDelta ? _centerValue : value;
        }
    }
}