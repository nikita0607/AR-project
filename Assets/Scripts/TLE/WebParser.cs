using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

namespace TLE
{
    public class WebParser 
    {
        public IEnumerator DownloadAndParseTle(string url, Action<List<Tle>> onSuccess, Action<string> onError)
        {
        //     string url = "http://r4uab.ru/satonline.txt";
        
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
    }

    
}