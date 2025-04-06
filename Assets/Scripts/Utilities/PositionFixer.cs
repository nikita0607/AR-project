using System;
using UnityEngine;

namespace Utilities
{
    public class PositionFixer : MonoBehaviour
    {
        private Vector3 _originalPosition;
        private void Start()
        {
            _originalPosition = transform.position;
        }

        public void RestorePosition()
        {
            transform.position = _originalPosition;
        }
    }
}