using UnityEngine;

public class PlacementController : MonoBehaviour
{
    [SerializeField] private GameObject Place;
    [SerializeField] private GameObject QrTarget;
    [SerializeField] private GameObject GlobusTarget;

    [SerializeField] private GameObject ArCamera;

    [Header("Objects for QR Mode")]
    [SerializeField] private GameObject[] _qrModeEnObjects;
    [SerializeField] private GameObject[] _qrModeDisObjects;

    [Header("Objects for GLOBUS Mode")]
    [SerializeField] private GameObject[] _globusModeEnObjects;
    [SerializeField] private GameObject[] _globusModeDisObjects;

    private void Start() {
        ArCamera.SetActive(false);
        QrTarget.SetActive(false);
        GlobusTarget.SetActive(false);
    }

    private void SelectMode() {
        ArCamera.SetActive(true);
        Place.SetActive(true);
    }

    public void QrMode() {
        SelectMode();
        QrTarget.SetActive(true);
        Place.transform.SetParent(QrTarget.transform);

        foreach (var obj in _qrModeEnObjects)
        {
            obj.SetActive(true);
        }
        foreach (var obj in _qrModeDisObjects)
        {
            obj.SetActive(true);
        }
    }

    public void GlobusMode() {
        SelectMode();
        GlobusTarget.SetActive(true);
        Place.transform.SetParent(GlobusTarget.transform);

        foreach (var obj in _globusModeDisObjects)
        {
            obj.SetActive(false);
        }
        foreach (var obj in _globusModeEnObjects)
        {
            obj.SetActive(true);
        }
    }
}
