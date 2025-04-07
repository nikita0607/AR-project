using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary> Предоставляет статические методы для логирования сообщений в Unity-консоль и/или файл. </summary>
public class Logger 
{
    private static string _logFileName;
    private static bool _fileLoggingEnabled;

    /// <summary> Логирует сообщение в консоль Debug и (если включено) в файл. </summary>
    /// <typeparam name="T"> Тип логируемого сообщения (автоматически выводится). </typeparam>
    /// <param name="message"> Сообщение для логирования. Будет преобразовано в строку через ToString(). </param>
    public static void Log<T>(T message) 
    {
        if (_fileLoggingEnabled) {
            File.AppendAllText(_logFileName, DateTime.Now.ToString() + ": " + message.ToString() + "\n");
        }
        Debug.Log(message.ToString());
    }

    /// <summary> Логирует все элементы списка в консоль Debug и пустую строку в файл (если логирование в файл включено). </summary>
    /// <typeparam name="T"> Тип элементов списка. </typeparam>
    /// <param name="list"> Список элементов для логирования. Каждый элемент будет выведен в консоль Debug. </param>
    public static void Log<T>(List<T> list) 
    {
        string msg = "";
        list.ForEach(x => Debug.Log(x));
        Log(msg);
    }

    /// <summary> Включает логирование в файл и устанавливает имя файла для логирования. </summary>
    /// <param name="filename"> Полный путь к файлу для сохранения логов. </param>
    /// <remarks> После вызова этого метода все последующие вызовы Log() будут записывать сообщения в указанный файл.
    /// Файл будет создан, если не существует, или дополнен, если уже существует. </remarks>
    public static void EnableFileLogging(string filename) 
    {
        _fileLoggingEnabled = true;
        _logFileName = filename;
    }
}