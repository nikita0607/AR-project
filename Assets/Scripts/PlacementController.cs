using TMPro;
using UnityEngine;

/// <summary>
/// Контроллер для управления режимами размещения объектов (QR и Globus) в AR-среде.
/// Обеспечивает переключение между режимами, управление видимостью объектов и отображение информации.
/// </summary>
public class PlacementController : MonoBehaviour
{
    /// <summary> Основной размещаемый объект, который будет позиционироваться относительно целей. </summary>
    [SerializeField] private GameObject Place;

    /// <summary> Целевой объект-маркер для QR режима размещения. </summary>
    [SerializeField] private GameObject QrTarget;

    /// <summary> Целевой объект-маркер для Globus режима размещения. </summary>
    [SerializeField] private GameObject GlobusTarget;

    /// <summary> AR-камера, которая активируется при выборе любого режима. </summary>
    [SerializeField] private GameObject ArCamera;

    /// <summary> Текстовое поле для отображения информации о текущем режиме. </summary>
    [SerializeField] private TMP_Text _textField;

    /// <summary> Массив объектов, которые нужно активировать при выборе QR режима. </summary>
    [Header("Настройки QR режима")]
    [SerializeField] private GameObject[] _qrModeEnObjects;

    /// <summary> Массив объектов, которые нужно деактивировать при выборе QR режима. </summary>
    [SerializeField] private GameObject[] _qrModeDisObjects;

    /// <summary> Текст с информацией о QR режиме (отображается в _textField). </summary>
    [SerializeField, TextArea(5,20)] private string _qrInfoText;

    /// <summary> Массив объектов, которые нужно активировать при выборе Globus режима. </summary>
    [Header("Настройки Globus режима")]
    [SerializeField] private GameObject[] _globusModeEnObjects;

    /// <summary> Массив объектов, которые нужно деактивировать при выборе Globus режима. </summary>
    [SerializeField] private GameObject[] _globusModeDisObjects;

    /// <summary> Текст с информацией о Globus режиме (отображается в _textField). </summary>
    [SerializeField, TextArea(5,20)] private string _globusInfoText;

    /// <summary> Инициализация начального состояния. </summary>
    private void Start() 
    {
        ArCamera.SetActive(false);
        QrTarget.SetActive(false);
        GlobusTarget.SetActive(false);
    }

    /// <summary> Активирует базовые компоненты для любого режима. </summary>
    private void SelectMode() 
    {
        ArCamera.SetActive(true);
        Place.SetActive(true);
    }

    /// <summary> Выход из текущего режима (деактивация всех компонентов). </summary>
    public void ExitMode()
    {
        ArCamera.SetActive(false);
        QrTarget.SetActive(false);
        GlobusTarget.SetActive(false);
    }

    /// <summary> Активирует QR режим размещения. </summary>
    public void QrMode()
    {
        SelectMode();
        QrTarget.SetActive(true);
        Place.transform.SetParent(QrTarget.transform);
        _textField.text = _qrInfoText;

        // Активация/деактивация связанных объектов
        SetObjectsActive(_qrModeEnObjects, true);
        SetObjectsActive(_qrModeDisObjects, false);
    }

    /// <summary> Активирует Globus режим размещения. </summary>
    public void GlobusMode() 
    {
        SelectMode();
        GlobusTarget.SetActive(true);
        Place.transform.SetParent(GlobusTarget.transform);
        _textField.text = _globusInfoText;

        // Активация/деактивация связанных объектов
        SetObjectsActive(_globusModeEnObjects, true);
        SetObjectsActive(_globusModeDisObjects, false);
    }

    /// <summary> Управляет активностью массива объектов. </summary>
    /// <param name="objects"> Массив объектов. </param>
    /// <param name="active"> Флаг активности. </param>
    private void SetObjectsActive(GameObject[] objects, bool active) 
    {
        foreach (var obj in objects)
            if (obj != null) 
                obj.SetActive(active);
    }
}