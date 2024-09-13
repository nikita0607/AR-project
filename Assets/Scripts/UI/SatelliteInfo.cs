using System;
using One_Sgp4;
using TMPro;
using UnityEngine;

public class SatelliteInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private GameObject _lineRedererParrent;
    [SerializeField] private GameObject _trailPrefab;

    private Action _onHide;

    private void Awake() {
    }

    public void SetInfo(Satellite satellite) {
        Coordinate cords = satellite.GetPosition();
        
        String text = $"Название: {satellite.TLE.getName()}\n" +
                      $"NORAD-ID: {satellite.TLE.getNoradID()}\n" +
                      $"Координаты:\n\tширота: {cords.getLatitude():f2}\n\tдолгота: {cords.getLongitude():f2}\n";

        _textField.text = text;

        satellite.timeForSatellite = new EpochTime(DateTime.UtcNow);

        for (int i=0; i<94; i++) {
            Coordinate satCords = satellite.GetPosition();

            GameObject trail = Instantiate(_trailPrefab, Vector3.zero, Quaternion.identity, _lineRedererParrent.transform);
            
            trail.transform.localPosition = EciPositionable.FromLongLat(-(float)satCords.getLongitude(), (float)satCords.getLatitude(), 2.2f);

            satellite.timeForSatellite.addMinutes(1);
            satellite.UpdatePosition();

            _onHide += () => {
                Destroy(trail);
            };
        };

        satellite.timeForSatellite = TimeManager.Instance.GetTime();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);

        _onHide();
        _onHide = null;
    }
}
