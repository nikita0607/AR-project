using UnityEngine;
using System;

using One_Sgp4;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float timeVelocity;
    [SerializeField] private TMP_Text text;
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
        time = new EpochTime(DateTime.UtcNow);
    }


    private void Update()
    {
        time.addTick(timeVelocity * Time.deltaTime);
        text.text = time.ToString();
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