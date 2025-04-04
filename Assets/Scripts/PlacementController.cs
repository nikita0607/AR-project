using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacementController : MonoBehaviour
{
    [SerializeField] private GameObject Place;
    [SerializeField] private GameObject QrTarget;
    [SerializeField] private GameObject GlobusTarget;

    [SerializeField] private GameObject ArCamera;
    
    [SerializeField] private TMP_Text _textField;

    [Header("Objects for QR Mode")]
    [SerializeField] private GameObject[] _qrModeEnObjects;
    [SerializeField] private GameObject[] _qrModeDisObjects;
    [SerializeField, TextArea(5,20)] private string _qrInfoText;

    [Header("Objects for GLOBUS Mode")]
    [SerializeField] private GameObject[] _globusModeEnObjects;
    [SerializeField] private GameObject[] _globusModeDisObjects;
    [SerializeField, TextArea(5,20)] private string _globusInfoText;

    private void Start() {
        ArCamera.SetActive(false);
        QrTarget.SetActive(false);
        GlobusTarget.SetActive(false);
    }

    private void SelectMode() {
        ArCamera.SetActive(true);
        Place.SetActive(true);
    }

    public void ExitMode()
    {
        ArCamera.SetActive(false);
        QrTarget.SetActive(false);
        GlobusTarget.SetActive(false);
    }

    public void QrMode() {
        SelectMode();
        QrTarget.SetActive(true);
        Place.transform.SetParent(QrTarget.transform);
        _textField.text = _qrInfoText;

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
        _textField.text = _globusInfoText;

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
