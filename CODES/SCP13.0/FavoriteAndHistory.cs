using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FavoriteAndHistory : MonoBehaviour
{
    public enum StorageLocation
    {
        History = 0,
        Favorites = 1,
        IPHistory = 2
    }

    public const int MaxHistoryAmount = 10;

    public static readonly List<string> Favorites = new List<string>();
    public static readonly List<string> History = new List<string>();
    public static readonly List<string> IPHistory = new List<string>();

    public static string ServerIDLastJoined = string.Empty;

    public static readonly Dictionary<StorageLocation, List<string>> LocationToList = new Dictionary<StorageLocation, List<string>>
    {
        { StorageLocation.History, History },
        { StorageLocation.Favorites, Favorites },
        { StorageLocation.IPHistory, IPHistory }
    };

    private static readonly Dictionary<StorageLocation, string> StorageEnumToPath = new Dictionary<StorageLocation, string>
    {
        { StorageLocation.History, "History.txt" },
        { StorageLocation.Favorites, "Favorites.txt" },
        { StorageLocation.IPHistory, "IPHistory.txt" }
    };

    static FavoriteAndHistory()
    {
        Load(StorageLocation.History);
        Load(StorageLocation.Favorites);
        Load(StorageLocation.IPHistory);
    }

    public static string GetPath(StorageLocation location)
    {
        if (StorageEnumToPath.TryGetValue(location, out string fileName))
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "../LocalLow/SCP Secret Laboratory", fileName);
        }
        return null;
    }

    public static void ResetServerID()
    {
        ServerIDLastJoined = string.Empty;
    }

    public static void Load(StorageLocation location)
    {
        string path = GetPath(location);
        if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;

        try
        {
            List<string> list = LocationToList[location];
            list.Clear();
            list.AddRange(File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line)));
        }
        catch (Exception ex)
        {
            Debug.LogError($"[FavoriteAndHistory] Failed to load {location}: {ex.Message}");
        }
    }

    public static void Modify(StorageLocation location, string id, bool delete = false)
    {
        if (string.IsNullOrEmpty(id)) return;

        List<string> list = LocationToList[location];

        list.RemoveAll(x => x.Equals(id, StringComparison.OrdinalIgnoreCase));

        if (!delete)
        {
            if (location == StorageLocation.History || location == StorageLocation.IPHistory)
            {
                HistoryLimit(location, id);
            }
            else
            {
                list.Insert(0, id);
            }
        }
        try
        {
            string path = GetPath(location);
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            File.WriteAllLines(path, list);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[FavoriteAndHistory] Failed to save {location}: {ex.Message}");
        }
    }

    private static void HistoryLimit(StorageLocation location, string id)
    {
        List<string> list = LocationToList[location];

        list.Insert(0, id);

        while (list.Count > MaxHistoryAmount)
        {
            list.RemoveAt(list.Count - 1);
        }
    }

    public static bool IsInStorage(StorageLocation location, string id)
    {
        if (string.IsNullOrEmpty(id)) return false;
        return LocationToList[location].Contains(id, StringComparer.OrdinalIgnoreCase);
    }

    private static void Revent(string fileName)
    {
        Debug.Log("Reventing:" + fileName);
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "../LocalLow/SCP Secret Laboratory", fileName);

        try
        {
            if (File.Exists(path))
            {
                File.WriteAllText(path, string.Empty);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to revent file: " + ex.Message);
        }
    }
}