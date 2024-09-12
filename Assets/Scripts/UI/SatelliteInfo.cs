using System;
using One_Sgp4;
using TMPro;
using UnityEngine;

public class SatelliteInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _textField;
    public void SetInfo(Satellite satellite) {
        Coordinate cords = satellite.GetPosition();

        String text = $"Название: {satellite.TLE.getName()}\n" +
                      $"NORAD-ID: {satellite.TLE.getNoradID()}\n" +
                      $"Координаты: широта-{cords.getLatitude()} долгота-{cords.getLongitude()}\n";

        _textField.text = text;

    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
