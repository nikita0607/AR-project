using UnityEngine;
using System;

using One_Sgp4;
using TMPro;
using UnityEngine.Serialization;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float timeVelocity;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text timeSpeedText;
    private EpochTime time;

    public static TimeManager _instance;
    public static TimeManager Instance {
        get {
            if(_instance == null)
            {
                _instance = FindObjectOfType<TimeManager>();
            }
            return _instance;
        }
    }

    private void Start()
    {
        timeVelocity = 1;
        time = new EpochTime(DateTime.UtcNow.AddHours(3));
    }


    private void Update()
    {
        time.addTick(timeVelocity * Time.deltaTime);
        timeText.text = $"{time.getDateToString()}\n{time.getTimeToString()}";
        timeSpeedText.text = $"Управление временем. Текущая скорость = {timeVelocity}.";
    }

    public EpochTime GetTime()
    {
        return time;
    }

    public void SetNewVelocity(float newVelocity)
    {
        timeVelocity = newVelocity;
    }

    public void ChangeVelocity(float valueToChange)
    {
        timeVelocity += valueToChange;
    }

    public void ResetTime()
    {
        time = new EpochTime(DateTime.UtcNow);
        SetNewVelocity(1);
    }
}