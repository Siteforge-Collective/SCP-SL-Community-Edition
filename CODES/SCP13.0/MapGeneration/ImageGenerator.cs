using System;
using System.Collections.Generic;
using System.Linq;
using NorthwoodLib.Pools;
using UnityEngine;

namespace MapGeneration
{
    public class ImageGenerator : MonoBehaviour
    {
        [Serializable]
        public class ColorMap
        {
            public Color color = Color.white;

            public RoomType type;

            public float rotationY;

            public Vector2 centerOffset;

            public float RandomizedRotation;
        }

        [Serializable]
        public class RoomsOfType
        {
            public List<Room> roomsOfType = new List<Room>();

            public int amount;
        }

        [Serializable]
        public class Room
        {
            public List<GameObject> room = new List<GameObject>();

            public RoomType type;

            public bool required;

            public Room(Room r)
            {
                room = r.room;
                type = r.type;
                required = r.required;
            }
        }

        [Serializable]
        public class MinimapElement
        {
            public string roomName;

            public Texture icon;

            public Vector2 position;

            public int rotation;

            public GameObject roomSource;
        }

        [Serializable]
        public class MinimapLegend
        {
            public string containsInName;

            public Texture icon;

            public string label;
        }

        public enum RoomType
        {
            Straight = 0,
            Curve = 1,
            RoomT = 2,
            Cross = 3,
            Endoff = 4,
            Prison = 5
        }

        public int height;

        public Texture2D[] maps;

        private Texture2D map;

        private Color[] copy;

        private string alias;

        public float gridSize;

        public float minimapSize;

        public List<ColorMap> colorMap = new List<ColorMap>();

        public List<Room> availableRooms = new List<Room>();

        private List<MinimapElement> minimap = new List<MinimapElement>();

        public MinimapLegend[] legend;

        public RectTransform minimapTarget;

        public float y_offset;

        public static ImageGenerator[] ZoneGenerators = new ImageGenerator[3];

        private Transform entrRooms;

        private System.Random _rand;

        public RoomsOfType[] roomsOfType;

        public bool GenerateMap(int seed, string newAlias, out string blackbox)
        {
            blackbox = string.Empty;
            alias = newAlias;
            if (!NonFacilityCompatibility.currentSceneSettings.enableWorldGeneration)
            {
                return true;
            }
            try
            {
                blackbox = "Activating available rooms.";
                foreach (Room availableRoom in availableRooms)
                {
                    foreach (GameObject item in availableRoom.room)
                    {
                        item.SetActive(value: false);
                    }
                }
                PocketDimensionGenerator.RandomizeTeleports();
                blackbox = "Randomizing...";
                _rand = new System.Random(seed);
                blackbox = "Picking up a map atlas...";
                map = maps[RandomRange(0, maps.Length)];
                blackbox = "Getting pixels...";
                copy = map.GetPixels();
                blackbox = "Checking rooms...";
                GeneratorTask_CheckRooms();
                blackbox = "Removing not required rooms...";
                GeneratorTask_RemoveNotRequired();
                blackbox = "Setting up rooms...";
                GeneratorTask_SetRooms();
                blackbox = "Entrance Zone initializing...";
                InitEntrance();
                blackbox = "Cleaning up...";
                GeneratorTask_Cleanup();
                blackbox = "Reventing map...";
                map.SetPixels(copy);
                map.Apply();
                if (entrRooms != null)
                {
                    entrRooms.parent = null;
                }
                blackbox = "Completed.";
            }
            catch (Exception ex)
            {
                blackbox = blackbox + "\nError: " + ex.Message;
                return false;
            }
            return true;
        }

        private int RandomRange(int min, int max)
        {
            return _rand.Next(min, max);
        }

        private void InitEntrance()
        {
            if (height == -1001)
            {
                RoomIdentifier[] array = RoomIdentifier.AllRoomIdentifiers.Where((RoomIdentifier x) => x.Name == RoomName.HczCheckpointToEntranceZone && x.Zone == FacilityZone.HeavyContainment).ToArray();
                if (array.Length == 2)
                {
                    Transform transform = array[0].transform;
                    Transform transform2 = array[1].transform;
                    Transform transform3 = ((transform.position.z > transform2.position.z) ? transform2 : transform);
                    Transform transform4 = GameObject.Find("ChkpRef").transform;
                    GameObject.Find("EntranceRooms").transform.position += transform3.position - transform4.position;
                }
            }
        }

        private void GeneratorTask_Cleanup()
        {
            RoomsOfType[] array = roomsOfType;
            for (int i = 0; i < array.Length; i++)
            {
                foreach (Room item in array[i].roomsOfType)
                {
                    foreach (GameObject item2 in item.room)
                    {
                        if (item.type != RoomType.Prison)
                        {
                            item2.SetActive(value: false);
                        }
                    }
                }
            }
        }

        private void GeneratorTask_SetRooms()
        {
            for (int i = 0; i < map.height; i++)
            {
                for (int j = 0; j < map.width; j++)
                {
                    Color pixel = map.GetPixel(j, i);
                    foreach (ColorMap item in colorMap)
                    {
                        if (!(item.color != pixel))
                        {
                            Vector2 pos = new Vector2(j, i) + item.centerOffset;
                            PlaceRoom(pos, item);
                        }
                    }
                }
            }
        }

        private void GeneratorTask_RemoveNotRequired()
        {
            foreach (ColorMap item in colorMap)
            {
                bool flag = false;
                while (!flag)
                {
                    int num = 0;
                    foreach (Room item2 in roomsOfType[(int)item.type].roomsOfType)
                    {
                        num += item2.room.Count;
                    }
                    if (num <= roomsOfType[(int)item.type].amount)
                    {
                        break;
                    }
                    flag = true;
                    foreach (Room item3 in roomsOfType[(int)item.type].roomsOfType)
                    {
                        if (!item3.required && item3.room.Count > 0)
                        {
                            item3.room[0].SetActive(value: false);
                            item3.room.RemoveAt(0);
                            flag = false;
                            break;
                        }
                    }
                }
            }
        }

        private void GeneratorTask_CheckRooms()
        {
            for (int i = 0; i < map.height; i++)
            {
                for (int j = 0; j < map.width; j++)
                {
                    Color pixel = map.GetPixel(j, i);
                    foreach (ColorMap item in colorMap)
                    {
                        if (item.color != pixel)
                        {
                            continue;
                        }
                        BlankSquare(new Vector2(j, i) + item.centerOffset);
                        roomsOfType[(int)item.type].amount++;
                        List<Room> list = ListPool<Room>.Shared.Rent();
                        bool flag = availableRooms.Any((Room room) => room.type == item.type && room.room.Count > 0 && room.required);
                        bool flag2;
                        do
                        {
                            flag2 = false;
                            for (int num = 0; num < availableRooms.Count; num++)
                            {
                                if (availableRooms[num].type == item.type && availableRooms[num].room.Count > 0 && !(!availableRooms[num].required && flag))
                                {
                                    list.Add(new Room(availableRooms[num]));
                                    availableRooms.RemoveAt(num);
                                    flag2 = true;
                                    break;
                                }
                            }
                        }
                        while (flag2);
                        foreach (Room item2 in list)
                        {
                            roomsOfType[(int)item.type].roomsOfType.Add(new Room(item2));
                        }
                        ListPool<Room>.Shared.Return(list);
                    }
                }
            }
            map.SetPixels(copy);
            map.Apply();
        }

        private void PlaceRoom(Vector2 pos, ColorMap type)
        {
            string text = "";
            try
            {
                text = "ERR#1 (marking bitmap)";
                BlankSquare(pos);
                Room room = null;
                text = "ERR#2 (looping)";
                do
                {
                    text = "ERR#3 (randomizing)";
                    int index = RandomRange(0, roomsOfType[(int)type.type].roomsOfType.Count);
                    text = $"ERR#4 ({roomsOfType[(int)type.type].roomsOfType.Count} rooms remaining)";
                    room = roomsOfType[(int)type.type].roomsOfType[index];
                    if (room.room.Count == 0)
                    {
                        text = "ERR#5 (randomizing)";
                        roomsOfType[(int)type.type].roomsOfType.RemoveAt(index);
                    }
                }
                while (room.room.Count == 0);
                while (pos.x % 3f != 0f)
                {
                    pos.x += 1f;
                }
                pos.x /= 3f;
                while (pos.y % 3f != 0f)
                {
                    pos.y += 1f;
                }
                pos.y /= 3f;
                float num = type.rotationY + (float)RandomRange(0, 4) * type.RandomizedRotation;
                room.room[0].transform.localPosition = new Vector3(pos.x * gridSize, height, pos.y * gridSize);
                room.room[0].transform.localRotation = Quaternion.Euler(Vector3.up * (num + y_offset));
                text = "ERR#6 (preparing minimap)";
                if (minimapTarget != null)
                {
                    MinimapLegend minimapLegend = null;
                    MinimapLegend[] array = legend;
                    foreach (MinimapLegend minimapLegend2 in array)
                    {
                        if (room.room[0].name.Contains(minimapLegend2.containsInName))
                        {
                            minimapLegend = minimapLegend2;
                        }
                    }
                    if (minimapLegend != null)
                    {
                        minimap.Add(new MinimapElement
                        {
                            icon = minimapLegend.icon,
                            position = pos * 3f,
                            roomName = minimapLegend.label,
                            rotation = (int)num,
                            roomSource = room.room[0].gameObject
                        });
                    }
                }
                text = "ERR#7 (list element removal)";
                room.room[0].SetActive(value: true);
                room.room.RemoveAt(0);
            }
            catch (Exception ex)
            {
                SeedSynchronizer.DebugError(isFatal: true, "Failed to generate a room of " + alias + " zone (TYPE#" + type.type.ToString() + "). Error code - " + text + " | Debug info - " + ex.Message);
            }
        }

        private void BlankSquare(Vector2 centerPoint)
        {
            centerPoint = new Vector2(centerPoint.x - 1f, centerPoint.y - 1f);
            for (ushort num = 0; num < 3; num++)
            {
                for (ushort num2 = 0; num2 < 3; num2++)
                {
                    map.SetPixel((int)centerPoint.x + num, (int)centerPoint.y + num2, new Color(0.3921f, 0.3921f, 0.3921f, 1f));
                }
            }
            map.Apply();
        }

        private void Awake()
        {
            int num = -1;
            switch (height)
            {
                case 0:
                    num = 0;
                    break;
                case -1000:
                    num = 1;
                    break;
                case -1001:
                    num = 2;
                    break;
            }
            if (num < 0)
            {
                SeedSynchronizer.DebugError(isFatal: true, "The array of Image Generators could not be set up. Height: " + height);
            }
            else
            {
                ZoneGenerators[num] = this;
            }
        }
    }
}
