using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities
{
    public class MedianFilterVector3
    {
        private readonly Queue<Vector3> _window = new();

        private Vector3 _centerValue;
        
        public Vector3 Filter(Vector3 value, int windowSize = 5)
        {
            _window.Enqueue(value);
            if (_window.Count > windowSize)
            {
                _window.Dequeue();
            }
            var sorted = _window.OrderBy(v => Vector3.Distance(_centerValue, v));
            _centerValue = sorted.ElementAt(_window.Count / 2);
            return _centerValue;
        }
    }
}