using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TLE
{
    public class TleLoader
    {
        private static readonly WebParser _webParser = new();
        private static List<Tle> _loadedTles = new();
        
        private static readonly string _tleFilePath = Path.Combine(Application.persistentDataPath, "TLE.txt");
        
        public static IEnumerator GetTle(System.Action<List<Tle>> onLoadSuccess)
        {
             yield return _webParser.DownloadAndParseTle(
                        tleList =>
                        {
                            _loadedTles = tleList;
                        },
                        error => {
                            Debug.LogError(error);
                            LoadFromLocalFile();
                        }
                    );
             
             onLoadSuccess?.Invoke(_loadedTles);
             SaveToLocalFile();
        }

        private static void SaveToLocalFile()
        {
            string allData = String.Join("\n", _loadedTles.Select(tle => tle.ToString()));
            
            try
            {
                File.WriteAllText(_tleFilePath, allData);
                Debug.Log($"Данные сохранены в: {_tleFilePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка записи файла: {e.Message}");
            }
        }
        
        private static void LoadFromLocalFile()
          {
              
              if(!File.Exists(_tleFilePath))
              {
                  Debug.LogError("Локальный файл данных не найден");
                  return;
              }
      
              try
              {
                  string rawData = File.ReadAllText(_tleFilePath);
                  
                  List<string> lines = new List<string>(rawData.Split('\n'))
                      .Select(line => line.Trim())
                      .Where(line => !string.IsNullOrEmpty(line))
                      .ToList();
                  
                  if(lines.Count % 3 != 0)
                  {
                      Debug.LogError("Некорректный формат локального файла");
                      return;
                  }
          
                  _loadedTles = Enumerable
                      .Range(0, lines.Count / 3)
                      .Select(i => new TLE.Tle(
                          name: lines[i * 3],
                          line1: lines[i * 3 + 1],
                          line2: lines[i * 3 + 2]))
                      .ToList();
              }
              catch (System.Exception e)
              {
                  Debug.LogError($"Ошибка чтения файла: {e.Message}");
              }
          }
    }
    public class Tle
    {
        public string Name;
        public string Line1;
        public string Line2;

        public Tle(string name, string line1, string line2)
        {
            Name = name;
            Line1 = line1;
            Line2 = line2;
        }

        public override string ToString()
        {
           return $"{Name}\n{Line1}\n{Line2}";
        }
    }
}