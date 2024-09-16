using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger {
    private static string _logFileName;
    private static bool _fileLoggingEnabled;

    public static void Log<T>(T message) {
        if (_fileLoggingEnabled) {
            File.AppendAllText(_logFileName, DateTime.Now.ToString() + ": " + message.ToString() + "\n");
        }
        Debug.Log(message.ToString());
    }

    public static void Log<T>(List<T> list) {
        string msg = "";
        list.ForEach(x => msg += x.ToString());
        Log(msg);
    }

    public static void EnableFileLogging(string filename) {
        _fileLoggingEnabled = true;
        _logFileName = filename;
    }
}