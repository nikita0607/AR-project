using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace TLE
{
    public class WebParser : MonoBehaviour
    {
        public void LoadTleData(System.Action<List<Tle>> onSuccess, System.Action<string> onError)
        {
            StartCoroutine(DownloadAndParseTle(onSuccess, onError));
        }

        private IEnumerator DownloadAndParseTle(System.Action<List<Tle>> onSuccess, System.Action<string> onError)
        {
            string url = "http://r4uab.ru/satonline.txt";

            using (UnityWebRequest request = new UnityWebRequest(url)
            {
                downloadHandler = new DownloadHandlerBuffer(),
                certificateHandler = new CustomCertificateHandler()
            })
            {
                yield return request.SendWebRequest();

                    // Обновлённая проверка ошибок
                    if (request.result == UnityWebRequest.Result.ConnectionError || 
                        request.result == UnityWebRequest.Result.ProtocolError)
                    {
                        onError?.Invoke($"Ошибка: {request.error}");
                        yield break;
                    }

                    string rawText = request.downloadHandler.text;

                    SaveToFile(rawText);
                    List<string> processedLines = ProcessRawText(rawText);
                
                    if (processedLines.Count % 3 != 0)
                    {
                        onError?.Invoke("Некорректный формат данных: количество строк не кратно 3");
                        yield break;
                    }

                    List<Tle> tleList = ParseTleList(processedLines);
                    onSuccess?.Invoke(tleList);
            }
        }

        // Добавляем кастомный обработчик сертификатов
        private class CustomCertificateHandler : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true; // Всегда принимать сертификат
            }
        }

        // Остальные методы без изменений
        private List<string> ProcessRawText(string rawText)
        {
            return rawText
                .Split('\n')
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .ToList();
        }

        private List<Tle> ParseTleList(List<string> lines)
        {
            return Enumerable
                .Range(0, lines.Count / 3)
                .Select(i => new Tle(
                    name: lines[i * 3],
                    line1: lines[i * 3 + 1],
                    line2: lines[i * 3 + 2]))
                .ToList();
        }
        
        private void SaveToFile(string content)
        {
            string path = "Assets/Resources/TLE_data/data.txt";
            
            try
            {
                File.WriteAllText(path, content);
                Debug.Log($"Данные сохранены в: {path}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка записи файла: {e.Message}");
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
    }
}