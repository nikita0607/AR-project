 using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SatelliteData.Filters;
using UnityEngine;
using Exception = System.Exception;

namespace SatelliteData
{
    /// <summary> Класс для загрузки и обработки TLE-данных спутников. </summary>
    public class TleLoader
    {
        private static readonly WebParser _webParser = new();
        private static List<LoadedTle> _loadedTles = new();
        
        /// <summary> Загружает TLE-данные из указанного источника. </summary>
        /// <param name="url"> URL для загрузки данных. </param>
        /// <param name="satelliteType"> Тип спутников для загрузки. </param>
        /// <param name="onLoadSuccess"> Коллбек при успешной загрузке. </param>
        /// <param name="onError"> Коллбек при ошибке загрузки. </param>
        /// <param name="useWeb"> Флаг использования веб-загрузки. </param>
        public static IEnumerator GetTle(string url, SatelliteType satelliteType, Action<List<LoadedTle>> onLoadSuccess, 
            Action<string> onError, bool useWeb=true)
        {
            bool isLoadSuccess = false;
            
            if (useWeb)
                yield return _webParser.DownloadAndParseTle(
                    url,
                    tleList => {
                        _loadedTles = tleList;
                        SaveToLocalFile(satelliteType);
                        isLoadSuccess = true;
                    },
                    error =>
                    {
                        Debug.LogError(error);
                        isLoadSuccess = LoadFromLocalFile(satelliteType);
                    }
                );
            else
                isLoadSuccess = LoadFromLocalFile(satelliteType);
            
            if (isLoadSuccess)
                onLoadSuccess?.Invoke(_loadedTles);
            else
                onError?.Invoke(url+": не получилось");
            
            yield return null;
        }

        /// <summary> Сохраняет загруженные TLE-данные в локальный файл. </summary>
        /// <param name="satelliteType"> Тип спутников для сохранения. </param>
        private static void SaveToLocalFile(SatelliteType satelliteType)
        {
            string allData = String.Join("\n", _loadedTles.Select(tle => tle.ToString()));
            string tleFilePath = Path.Combine(Application.persistentDataPath, $"{satelliteType+"_TLE"}.txt");
            
            try
            {
                File.WriteAllText(tleFilePath, allData);
                Debug.Log($"Данные сохранены в: {tleFilePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка записи файла: {e.Message}");
            }
        }
        
        /// <summary> Загружает TLE-данные из локального файла. </summary>
        /// <param name="satelliteType"> Тип спутников для загрузки. </param>
        /// <returns> True если загрузка успешна, иначе False. </returns>
        private static bool LoadFromLocalFile(SatelliteType satelliteType)
        {
            string tleFilePath = Path.Combine(Application.persistentDataPath, $"{satelliteType+"_TLE"}.txt");
            
            if(!File.Exists(tleFilePath))
            {
                Debug.LogError("Локальный файл данных не найден");
                return false;
            }
    
            try
            {
                string rawData = File.ReadAllText(tleFilePath);
                
                List<string> lines = new List<string>(rawData.Split('\n'))
                    .Select(line => line.Trim())
                    .Where(line => !string.IsNullOrEmpty(line))
                    .ToList();
                
                if(lines.Count % 3 != 0)
                {
                    Debug.LogError("Некорректный формат локального файла");
                    return false;
                }
        
                _loadedTles = Enumerable
                    .Range(0, lines.Count / 3)
                    .Select(i => new LoadedTle(
                        name: lines[i * 3],
                        line1: lines[i * 3 + 1],
                        line2: lines[i * 3 + 2]))
                    .ToList();
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка чтения файла: {e.Message}");
                return false;
            }
            
            return true;
        }
    }

    /// <summary> Класс для хранения загруженных TLE-данных. </summary>
    public class LoadedTle
    {
        /// <summary> Название спутника. </summary>
        public string Name;
        
        /// <summary> Первая строка TLE-данных. </summary>
        public string Line1;
        
        /// <summary> Вторая строка TLE-данных. </summary>
        public string Line2;

        /// <summary> Создает новый экземпляр загруженных TLE-данных. </summary>
        public LoadedTle(string name, string line1, string line2)
        {
            Name = name;
            Line1 = line1;
            Line2 = line2;
        }

        /// <summary> Возвращает строковое представление TLE-данных. </summary>
        public override string ToString() => $"{Name}\n{Line1}\n{Line2}";
    }
}