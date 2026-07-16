using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
    public static class FirearmLogger
    {
        public static bool Enabled = true;

        private static readonly object _lock = new object();
        private static StreamWriter _writer;
        private static string _path;

        private static StreamWriter Writer
        {
            get
            {
                if (_writer == null)
                {
                    _path = Path.Combine(Application.persistentDataPath, "firearm_debug.log");
                    _writer = new StreamWriter(_path, append: true, Encoding.UTF8)
                    {
                        AutoFlush = true
                    };
                    _writer.WriteLine($"\n=== SESSION START {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===\n");
                }
                return _writer;
            }
        }

        public static void Log(string tag, string msg)
        {
            if (!Enabled) return;
            string line = $"[{DateTime.Now:HH:mm:ss.fff}][{tag}] {msg}";
            lock (_lock)
            {
                try { Writer.WriteLine(line); }
                catch { }
            }
            UnityEngine.Debug.Log(line);
        }

        public static void Warn(string tag, string msg)
        {
            if (!Enabled) return;
            string line = $"[{DateTime.Now:HH:mm:ss.fff}][{tag}][WARN] {msg}";
            lock (_lock)
            {
                try { Writer.WriteLine(line); }
                catch { }
            }
            UnityEngine.Debug.LogWarning(line);
        }

        public static void Clear()
        {
            lock (_lock)
            {
                try
                {
                    _writer?.Close();
                    _writer = null;
                    if (_path != null && File.Exists(_path))
                        File.Delete(_path);
                }
                catch { }
            }
        }
    }
}
