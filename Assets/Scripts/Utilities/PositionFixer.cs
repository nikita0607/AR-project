using UnityEngine;

namespace Utilities
{
    public class PositionFixer : MonoBehaviour
    {
        [SerializeField] private float changeDelta;
        [SerializeField] private float changeCamDelta;

        private Vector3 _relativePositionInParent;
        private bool _parentChanged;
        private Transform _parentTransform;


        private Vector3 _prevCamPosition;
        private Vector3 _prevCamPositionBeforeChange;

        private Vector3 _prevPosition;
        
        private Camera _cam;
        
        private void OnTransformParentChanged()
        {
            Debug.Log("Change Parent!");
            if (_parentChanged) return;

            _parentChanged = true;
            _relativePositionInParent = transform.parent.position - transform.position;
            
            _parentTransform = transform.parent;
            transform.parent = null;
        }

        private void Start()
        {
            _prevCamPosition = Camera.main.transform.position;
        }

        private void Update()
        {
            var newPosition = _parentTransform.position - _relativePositionInParent;
            if (Mathf.Abs((transform.position - newPosition).magnitude) > changeDelta)
            {
                Debug.Log("Rrrrr");
                _prevPosition = newPosition;
            }
            transform.position = _prevPosition;

            if (Mathf.Abs((Camera.main.transform.position - _prevCamPosition).magnitude) < changeCamDelta)
            {
                transform.position -= _prevCamPosition - Camera.main.transform.position;
            }
            else
            {
                _prevCamPosition = Camera.main.transform.position;
            }
        }
    }
}