using System;
using System.Collections.Generic;
using GameCore;
using NorthwoodLib.Pools;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Serializable]
    public class Room
    {
        public string label;

        public Offset roomOffset;

        public GameObject roomPrefab;

        public string type;

        public Transform readonlyPoint;

        public Offset iconoffset;
    }

    [Serializable]
    public struct RoomPosition : IEquatable<RoomPosition>
    {
        public string type;

        public Transform point;

        public RectTransform ui_point;

        public bool Equals(RoomPosition other)
        {
            if (string.Equals(type, other.type) && point == other.point)
            {
                return ui_point == other.ui_point;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is RoomPosition other)
            {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (((((type != null) ? type.GetHashCode() : 0) * 397) ^ ((point != null) ? point.GetHashCode() : 0)) * 397) ^ ((ui_point != null) ? ui_point.GetHashCode() : 0);
        }

        public static bool operator ==(RoomPosition left, RoomPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RoomPosition left, RoomPosition right)
        {
            return !left.Equals(right);
        }
    }

    public bool isGenerated;

    public int useSimulator = -1;

    public List<Room> rooms = new List<Room>();

    public List<RoomPosition> positions = new List<RoomPosition>();

    private void Start()
    {
        if (useSimulator != -1)
        {
            GenerateMap(useSimulator);
        }
    }

    public void GenerateMap(int seed)
    {
        PocketDimensionGenerator.RandomizeTeleports();
        for (int i = 0; i < positions.Count; i++)
        {
            positions[i].point.name = "POINT" + i;
            if (!(positions[i].point.GetComponent<Point>() != null))
            {
                Debug.LogError("RoomManager: Missing 'Point' script at current position.");
                return;
            }
        }
        UnityEngine.Random.InitState(seed);
        GameCore.Console.AddLog("[MG REPLY]: Successfully recieved map seed!", new Color32(0, byte.MaxValue, 0, byte.MaxValue), nospace: true);
        List<RoomPosition> list = positions;
        GameCore.Console.AddLog("[MG TASK]: Setting rooms positions...", new Color32(0, byte.MaxValue, 0, byte.MaxValue));
        foreach (Room room in rooms)
        {
            GameCore.Console.AddLog("\t\t[MG INFO]: " + room.label + " is about to set!", new Color32(120, 120, 120, byte.MaxValue), nospace: true);
            List<int> list2 = ListPool<int>.Shared.Rent();
            for (int j = 0; j < list.Count; j++)
            {
                if (!positions[j].type.Equals(room.type))
                {
                    continue;
                }
                bool flag = true;
                Point[] componentsInChildren = room.roomPrefab.GetComponentsInChildren<Point>();
                foreach (Point point in componentsInChildren)
                {
                    if (positions[j].point.name == point.gameObject.name)
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    list2.Add(j);
                }
            }
            Point[] componentsInChildren2 = room.roomPrefab.GetComponentsInChildren<Point>();
            for (int num = list2.Count - 1; num >= 0; num--)
            {
                Point[] componentsInChildren = componentsInChildren2;
                foreach (Point point2 in componentsInChildren)
                {
                    if (positions[list2[num]].point.name == point2.gameObject.name)
                    {
                        list2.RemoveAt(num);
                        break;
                    }
                }
            }
            int index = list2[UnityEngine.Random.Range(0, list2.Count)];
            ListPool<int>.Shared.Return(list2);
            RoomPosition roomPosition = positions[index];
            GameObject roomPrefab = room.roomPrefab;
            room.readonlyPoint = roomPosition.point;
            roomPrefab.transform.SetParent(roomPosition.point);
            roomPrefab.transform.localPosition = room.roomOffset.position;
            roomPrefab.transform.localRotation = Quaternion.Euler(room.roomOffset.rotation);
            roomPrefab.transform.localScale = room.roomOffset.scale;
            roomPrefab.SetActive(value: true);
            positions.RemoveAt(index);
        }
        GameCore.Console.AddLog("--Map successfully generated--", new Color32(0, byte.MaxValue, 0, byte.MaxValue));
        isGenerated = true;
    }
}
