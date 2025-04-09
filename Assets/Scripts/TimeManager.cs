using UnityEngine;
using System;
using One_Sgp4;
using TMPro;

/// <summary>
/// Менеджер времени, управляющий виртуальным временем в приложении.
/// Обеспечивает контроль скорости времени, отслеживание изменения минут и отображение времени.
/// </summary>
public class TimeManager : MonoBehaviour
{
    /// <summary> Текущая скорость течения времени (множитель реального времени). </summary>
    [Header("Настройки времени")]
    [SerializeField] 
    private float timeVelocity = 1f;
    
    /// <summary> Текстовое поле для отображения текущей даты и времени. </summary>
    [SerializeField] 
    private TMP_Text timeText;
    
    /// <summary> Текстовое поле для отображения текущей скорости времени. </summary>
    [SerializeField] 
    private TMP_Text timeSpeedText;

    /// <summary> Текущее виртуальное время (в формате EpochTime). </summary>
    private EpochTime time;
    
    /// <summary> Последняя зафиксированная минута для отслеживания изменения минут. </summary>
    private int _lastMinunte;
    
    /// <summary> Статическая ссылка на экземпляр менеджера времени. (Singleton)</summary>
    private static TimeManager _instance;
    
    /// <summary> Событие, вызываемое при каждом изменении минуты. </summary>
    public Action OnMinuteChanged;
    
    /// <summary> Текущее время в формате UTC+3. </summary>
    private EpochTime _correctedTime;

    /// <summary> Доступ к экземпляру TimeManager через Singleton pattern.
    /// При первом обращении автоматически ищет существующий экземпляр на сцене. </summary>
    public static TimeManager Instance {
        get {
            if(_instance == null)
            {
                _instance = FindObjectOfType<TimeManager>();
                if(_instance == null)
                {
                    Debug.LogError("TimeManager не найден на сцене");
                }
            }
            return _instance;
        }
    }

    /// <summary> Инициализация времени. Устанавливает начальное время (UTC+3) и базовую скорость. </summary>
    private void Start()
    {
        timeVelocity = 1f;
        ResetTime();
        _lastMinunte = time.getMin();
        
        _correctedTime = new EpochTime(time);
        _correctedTime.addHours(3);
    }

    /// <summary> Основной цикл обновления времени. Вызывается каждый кадр.
    /// Обновляет виртуальное время, отображает его и проверяет изменение минут. </summary>
    private void Update()
    {
        // Обновление виртуального времени
        time.addTick(timeVelocity * Time.deltaTime);
        _correctedTime.addTick(timeVelocity * Time.deltaTime);
        
        // Обновление UI

        timeText.text = $"{time.getDateToString()}\n{_correctedTime.getTimeToString()}";
        timeSpeedText.text = $"Управление временем. Текущая скорость = {timeVelocity}.";

        // Проверка изменения минуты
        if (time.getMin() != _lastMinunte)
        {
            OnMinuteChanged?.Invoke();
            _lastMinunte = time.getMin();
        }
    }

    /// <summary> Возвращает текущее виртуальное время. </summary>
    /// <returns> Текущее время в формате EpochTime. </returns>
    public EpochTime GetTime() => time;

    /// <summary> Устанавливает новую скорость течения времени. </summary>
    /// <param name="newVelocity"> Новая скорость (1.0 = реальное время). </param>
    private void SetNewVelocity(float newVelocity) => timeVelocity = newVelocity > 0f ? newVelocity : 1f;

    /// <summary> Изменяет текущую скорость течения времени на указанное значение. </summary>
    /// <param name="valueToChange"> Значение для изменения скорости (может быть отрицательным). </param>
    public void ChangeVelocity(float valueToChange)
    {
        float newVal = timeVelocity += valueToChange;
        timeVelocity = newVal > 0f ? newVal : 0f;
    }

    /// <summary> Сбрасывает время на текущее (UTC+3) и устанавливает стандартную скорость (1.0). </summary>
    public void ResetTime()
    {
        time = new EpochTime(DateTime.UtcNow);
        _correctedTime = new EpochTime(DateTime.UtcNow.AddHours(3));
        SetNewVelocity(1);
    }
}