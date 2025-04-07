using System.Collections.Generic;
using UnityEngine;

namespace SatelliteData.Info
{
    /// <summary> Класс для сериализации и кэширования данных о спутниках. </summary>
    public class SatellitesSerializer
    {
        /// <summary> Массив данных о спутниках (используется для десериализации). </summary>
        public SatelliteSer[] Satellites;

        /// <summary> Кэш данных о спутниках в виде словаря (ключ - название спутника). </summary>
        private static readonly Dictionary<string, SatelliteSer> _satellites = new();

        /// <summary> Получает словарь данных о спутниках из JSON-файла. </summary>
        /// <param name="textAsset"> Текстовый ассет с JSON-данными. </param>
        /// <returns> Словарь с данными спутников (кэшируется после первого обращения). </returns>
        public static Dictionary<string, SatelliteSer> GetSatellitesSer(TextAsset textAsset)
        {
            if (_satellites.Count == 0)
            {
                SatellitesSerializer satells = JsonUtility.FromJson<SatellitesSerializer>(textAsset.text);
                foreach (var sat in satells.Satellites)
                {
                    Debug.Log(sat.Name + " " + sat.Info);
                    _satellites.Add(sat.Name, sat);
                }
            }

            return _satellites;
        }
    }
}