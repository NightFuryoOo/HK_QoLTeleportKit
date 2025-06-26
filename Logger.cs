using System.IO;
using System.Text;
using UnityEngine;

namespace QoLTeleportKit
{
    public class QoLLogger
    {
        private readonly string _logFilePath;

        public QoLLogger()
        {
            _logFilePath = Path.Combine(Application.persistentDataPath, "QoLTeleportKit.log");
        }

        public void Write(string message)
        {
            try
            {
                File.AppendAllText(_logFilePath, $"[{System.DateTime.Now:dd.MM.yyyy HH:mm:ss}] {message}\n", Encoding.UTF8);
            }
            catch
            {
                Modding.Logger.LogError("[QoLTeleportKit] Failed to write to log");
            }
        }
    }
}