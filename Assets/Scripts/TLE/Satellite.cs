using UnityEngine;
using System;
using One_Sgp4;
using Utilities;
using TLE.Filters;

public class Satellite : EciPositionable
{
    /// <summary> Название спутника. </summary>
    [SerializeField] public String Name;
    
    /// <summary> TLE-данные для спутника. </summary>
    public Tle Tle { get; set; }
    
    /// <summary> Время для спутника. </summary>
    public EpochTime TimeForSatellite {get; set; }
    
    /// <summary> Тип спутника. </summary>
    public SatelliteType  SatelliteType {get; set;}
    
    /// <summary> Менеджер времени. </summary>
    private TimeManager timeManager;

    /// <summary> Задание стартовых данных. </summary>
    private void Start()
    {
        transform.rotation = Quaternion.identity;
        gameObject.name = Tle.getName();

        timeManager = GameObject.FindWithTag("TimeManager").GetComponent<TimeManager>();
        TimeForSatellite = timeManager.GetTime();
    }

    private void Update()
    {
        if (Tle == null) Destroy(gameObject);

        TimeForSatellite = timeManager.GetTime();
        UpdatePosition();
    }
    
    /// <summary> Получить позицию. </summary>
    /// <param name="time"> Заданный момент времени. </param>
    /// <returns> Позиция спутника в заданный момент времени. </returns>
    public Coordinate GetPosition(EpochTime time = null) {
        if (time is null) time = TimeForSatellite;

        try
        {
            Sgp4Data satellitePos = SatFunctions.getSatPositionAtTime(Tle, time, Sgp4.wgsConstant.WGS_84);
            return SatFunctions.calcSatSubPoint(time, satellitePos, Sgp4.wgsConstant.WGS_84);
        }
        catch (ArgumentException)
        {
            // Debug.Log("Satellite " + name + " has problems" );
            return new Coordinate(0, 0);
        }
    }

    /// <summary> Действия при скрытии спутника. </summary>
    /// <param name="filter"> Фильтр для показа/скрытия спутника. </param>
    public void OnHide(Filter filter) => gameObject.SetActive(filter.ShouldShowSatellite(this));

    /// <summary> Поулчить высоту. </summary>
    /// <returns> Текущая высота спутника, относительно виртуальной модели Земли. </returns>
    public float GetHeight() => EarthParametrs.RealToVirtualDistance((float)GetPosition().getHeight() / 4);

    /// <summary> Обновить позицию. </summary>
    public void UpdatePosition()
    {
        Coordinate cords = GetPosition();
        Vector3 newPos = FromLongLat(-(float)cords.getLongitude(), (float)cords.getLatitude(), EarthParametrs.VirtualEarthRadius+GetHeight());

        SetPosition(newPos);
    }

  
}
