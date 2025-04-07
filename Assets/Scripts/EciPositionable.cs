using UnityEngine;

/// <summary> Класс для работы с ECI (Earth-Centered Inertial) координатами. </summary>
public class EciPositionable : MonoBehaviour
{
    /// <summary> Устанавливает позицию объекта в локальных координатах. </summary>
    /// <param name="x"> Координата X. </param>
    /// <param name="y"> Координата Y. </param>
    /// <param name="z"> Координата Z. </param>
    public void SetPosition(float x, float y, float z) => SetPosition(new Vector3(x, y, z));

    /// <summary> Устанавливает позицию объекта в локальных координатах. </summary>
    /// <param name="position"> Вектор позиции. </param>
    public void SetPosition(Vector3 position) => transform.localPosition = position;

    /// <summary> Преобразует географические координаты в ECI координаты. </summary>
    /// <param name="longitude"> Долгота в градусах. </param>
    /// <param name="latitude"> Широта в градусах. </param>
    /// <param name="radius"> Радиус от центра Земли. </param>
    /// <returns> Вектор позиции в ECI системе координат. </returns>
    /// <remarks>
    /// Формулы преобразования:
    /// omega = π/2 - latitude (в радианах)
    /// phi = longitude (в радианах)
    /// x = radius * sin(omega) * cos(phi)
    /// y = radius * sin(omega) * sin(phi)
    /// z = radius * cos(omega)
    /// Окончательный вектор: (x, z, -y) для соответствия Unity-координатам
    /// </remarks>
    public static Vector3 FromLongLat(float longitude, float latitude, float radius) {
        float omega = Mathf.PI/2 - Mathf.Deg2Rad * latitude;
        float phi = longitude * Mathf.Deg2Rad;

        float x = radius * Mathf.Sin(omega) * Mathf.Cos(phi);
        float y = radius * Mathf.Sin(omega) * Mathf.Sin(phi);
        float z = radius * Mathf.Cos(omega);
        
        return new Vector3(x, z, -y);
    }
}