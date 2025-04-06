using System;
using One_Sgp4;
using SatelliteData;
using SatelliteData.Info;
using TMPro;
using UnityEngine;
using Utilities;

public class SatelliteInfo : MonoBehaviour

{
    public Action OnHide;
    
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private GameObject _lineRedererParrent;
    [SerializeField] private GameObject _trailPrefab;
    [SerializeField] private TextAsset _satteliteJson;

    private Action _onHide;
    private Satellite _currentSatellite;
    
    private void OnEnable()
    {
        TimeManager.Instance.OnMinuteChanged += UpdateInfo;
    }

    private void OnDisable()
    {
        TimeManager.Instance.OnMinuteChanged -= UpdateInfo;
    }

    public void SetInfo(Satellite satellite) {
        _currentSatellite = satellite;
        
        Coordinate cords = satellite.GetPosition();
        
        string text = $"Название: {satellite.Tle.getName()}\n" +
                      $"NORAD-ID: {satellite.Tle.getNoradID()}\n" +
                      $"Координаты:\n\tширота: {cords.getLatitude():f2}\n\tдолгота: {cords.getLongitude():f2}\n";

        if (SatellitesSerializer.GetSatellitesSer(_satteliteJson).ContainsKey(satellite.name))
            text += $"{SatellitesSerializer.GetSatellitesSer(_satteliteJson)[satellite.name].Info}\n";

        _textField.text = text;

        // satellite.timeForSatellite = new EpochTime(DateTime.UtcNow);
        satellite.TimeForSatellite = new EpochTime(TimeManager.Instance.GetTime());

        for (int i=0; i<94; i++) {
            Coordinate satCords = satellite.GetPosition();

            GameObject trail = Instantiate(_trailPrefab, Vector3.zero, Quaternion.identity, _lineRedererParrent.transform);
            
            trail.transform.localPosition = EciPositionable.FromLongLat(-(float)satCords.getLongitude(), (float)satCords.getLatitude(), EarthParametrs.VirtualEarthRadius+satellite.GetHeight());

            satellite.TimeForSatellite.addMinutes(1);
            satellite.UpdatePosition();

            _onHide += () => {
                Destroy(trail);
            };
        };
        
        // TimeManager.Instance.ResetTime();
        satellite.TimeForSatellite = TimeManager.Instance.GetTime();

    }

    private void UpdateInfo()
    {
        if (!_currentSatellite) return;
        
        _onHide?.Invoke();
        _onHide = null;
        SetInfo(_currentSatellite);
        Show();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);

        _currentSatellite = null;
        _onHide();
        _onHide = null;
        
        OnHide?.Invoke();
    }
}
