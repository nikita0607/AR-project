using UnityEngine;

namespace Utilities
{
    public class LowpassFIlter
    {
        private Vector3? _prevValue;
        
        public Vector3 Filter(Vector3 value, float K, float filterDelta)
        {
            if (_prevValue == null)
            {
                _prevValue = value;
                return value;
            }
            
            
            var newPos = _prevValue.Value * K + value * (1 - K);

            if (Mathf.Abs(newPos.magnitude - value.magnitude) > filterDelta)
                return value;
            return newPos;
        }
    }
}