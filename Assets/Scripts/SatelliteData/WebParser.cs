using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

namespace SatelliteData
{
    /// <summary> Класс для загрузки и парсинга TLE-данных из сети. </summary>
    public class WebParser 
    {
        /// <summary> Загружает и парсит TLE-данные с указанного URL. </summary>
        /// <param name="url"> URL для загрузки данных. </param>
        /// <param name="onSuccess"> Коллбек при успешной загрузке. </param>
        /// <param name="onError"> Коллбек при ошибке загрузки. </param>
        public IEnumerator DownloadAndParseTle(string url, Action<List<LoadedTle>> onSuccess, Action<string> onError)
        {
            using UnityWebRequest request = new UnityWebRequest(url);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.certificateHandler = new CustomCertificateHandler();
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

            List<LoadedTle> tleList = ParseTleList(processedLines);
            onSuccess?.Invoke(tleList);
        }

        /// <summary> Кастомный обработчик сертификатов для обхода проверки SSL. </summary>
        private class CustomCertificateHandler : CertificateHandler
        {
            /// <summary> Всегда возвращает true, пропуская проверку сертификата. </summary>
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }

        /// <summary> Обрабатывает сырой текст, удаляя пустые строки. </summary>
        /// <param name="rawText"> Исходный текст TLE-данных. </param>
        /// <returns> Очищенный список строк. </returns>
        private List<string> ProcessRawText(string rawText)
        {
            return rawText
                .Split('\n')
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .ToList();
        }

        /// <summary> Парсит список строк в список объектов LoadedTle. </summary>
        /// <param name="lines"> Список строк в формате TLE. </param>
        /// <returns> Список объектов LoadedTle. </returns>
        private List<LoadedTle> ParseTleList(List<string> lines)
        {
            return Enumerable
                .Range(0, lines.Count / 3)
                .Select(i => new LoadedTle(
                    name: lines[i * 3],
                    line1: lines[i * 3 + 1],
                    line2: lines[i * 3 + 2]))
                .ToList();
        }
    }
}