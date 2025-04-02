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

    private int _lastMinunte;
    
    public static TimeManager _instance;
    public Action OnMinuteChanged;
    
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
        ResetTime();
        
        _lastMinunte = time.getMin();
    }


    private void Update()
    {
        time.addTick(timeVelocity * Time.deltaTime);
        timeText.text = $"{time.getDateToString()}\n{time.getTimeToString()}";
        timeSpeedText.text = $"Управление временем. Текущая скорость = {timeVelocity}.";

        if (time.getMin() != _lastMinunte)
        {
            OnMinuteChanged?.Invoke();
            _lastMinunte = time.getMin();
        }
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
        time = new EpochTime(DateTime.UtcNow.AddHours(3));
        SetNewVelocity(1);
    }
}