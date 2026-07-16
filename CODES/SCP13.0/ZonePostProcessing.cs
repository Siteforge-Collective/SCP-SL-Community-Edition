using System;
using MapGeneration;
using UnityEngine;
using UnityEngine.Rendering;
using Utils.NonAllocLINQ;

public class ZonePostProcessing : MonoBehaviour
{
    [Serializable]
    private struct ZoneVolumePair
    {
        public FacilityZone Zone;

        [SerializeField]
        private Volume _volume;

        public void SetWeight(float weight)
        {
            if (_volume == null) return;

            if (weight > 0f)
            {
                _volume.enabled = true;
                _volume.weight = Mathf.Clamp01(weight);
            }
            else
            {
                _volume.enabled = false;
            }
        }
    }

    [SerializeField]
    private ZoneVolumePair[] _zoneVols;

    [SerializeField]
    private RoomName[] _excludedRooms;

    [SerializeField]
    private float _maxTransitionDis;

    [SerializeField]
    private float _heightMultiplier;

    private bool _initalized;

    private bool[] _boundsSet;

    private Bounds[] _boundsPerZone;

    private Bounds _lastBounds;

    private FacilityZone _lastZone;

    private RoomIdentifier _lastDetectedRoom;

    private RoomIdentifier _lastWhitelistedRoom;

    private FacilityZone[] _whitelistedZones;

    private Vector3 _boundsSize;

    private void Start()
    {
        SeedSynchronizer.OnMapGenerated += Initalize;
        if (SeedSynchronizer.MapGenerated)
        {
            Initalize();
        }
    }

    private void OnDestroy()
    {
        SeedSynchronizer.OnMapGenerated -= Initalize;
        MainCameraController.OnUpdated -= UpdateWeights;
    }

    private void UpdateWeights()
    {
        Vector3 position = MainCameraController.CurrentCamera.position;
        RoomIdentifier roomIdentifier = RoomIdUtils.RoomAtPositionRaycasts(position);
        if (roomIdentifier != null && roomIdentifier != _lastDetectedRoom)
        {
            if (CheckWhitelisted(roomIdentifier))
            {
                _lastWhitelistedRoom = roomIdentifier;
                _lastBounds = GetRoomBounds(roomIdentifier);
                _lastZone = roomIdentifier.Zone;
            }
            _lastDetectedRoom = roomIdentifier;
        }
        if (_lastWhitelistedRoom == roomIdentifier)
        {
            _zoneVols.ForEach(delegate (ZoneVolumePair x)
            {
                bool flag = x.Zone == _lastZone;
                x.SetWeight(flag ? 1 : 0);
            });
            return;
        }
        FacilityZone facilityZone = FacilityZone.None;
        float num = _maxTransitionDis * _maxTransitionDis;
        ZoneVolumePair[] zoneVols = _zoneVols;
        for (int num2 = 0; num2 < zoneVols.Length; num2++)
        {
            ZoneVolumePair zoneVolumePair = zoneVols[num2];
            if (zoneVolumePair.Zone != _lastZone)
            {
                float num3 = _boundsPerZone[(int)zoneVolumePair.Zone].SqrDistance(position);
                if (!(num3 >= num))
                {
                    facilityZone = zoneVolumePair.Zone;
                    num = num3;
                }
            }
        }
        if (facilityZone == FacilityZone.None)
        {
            return;
        }
        float num4 = Mathf.Sqrt(num);
        float num5 = Mathf.Sqrt(_lastBounds.SqrDistance(position));
        float num6 = Mathf.Min(_maxTransitionDis, num4 + num5);
        float num7 = Mathf.Clamp01(num4 / num6);
        zoneVols = _zoneVols;
        for (int num2 = 0; num2 < zoneVols.Length; num2++)
        {
            ZoneVolumePair zoneVolumePair2 = zoneVols[num2];
            if (zoneVolumePair2.Zone == _lastZone)
            {
                zoneVolumePair2.SetWeight(num7);
            }
            else if (zoneVolumePair2.Zone == facilityZone)
            {
                zoneVolumePair2.SetWeight(1f - num7);
            }
            else
            {
                zoneVolumePair2.SetWeight(0f);
            }
        }
    }

    private void Initalize()
    {
        if (!_initalized)
        {
            _initalized = true;
            _whitelistedZones = new FacilityZone[_zoneVols.Length];
            for (int i = 0; i < _zoneVols.Length; i++)
            {
                _whitelistedZones[i] = _zoneVols[i].Zone;
            }
            GenerateZoneBounds();
            MainCameraController.OnUpdated += UpdateWeights;
        }
    }

    private void GenerateZoneBounds()
    {
        int maxValue = 0;
        _zoneVols.ForEach(delegate (ZoneVolumePair x)
        {
            maxValue = Mathf.Max(maxValue, (int)x.Zone);
        });
        int num = maxValue + 1;
        _boundsSet = new bool[num];
        _boundsPerZone = new Bounds[num];
        _boundsSize = Vector3.Scale(RoomIdentifier.GridScale, new Vector3(1f, _heightMultiplier, 1f));
        RoomIdentifier.AllRoomIdentifiers.ForEach(AddRoomToBounds);
    }

    private void AddRoomToBounds(RoomIdentifier room)
    {
        if (CheckWhitelisted(room))
        {
            int zone = (int)room.Zone;
            Bounds roomBounds = GetRoomBounds(room);
            if (_boundsSet[zone])
            {
                _boundsPerZone[zone].Encapsulate(roomBounds);
                return;
            }
            _boundsSet[zone] = true;
            _boundsPerZone[zone] = roomBounds;
        }
    }

    private bool CheckWhitelisted(RoomIdentifier room)
    {
        if (!_excludedRooms.Contains(room.Name))
        {
            return _whitelistedZones.Contains(room.Zone);
        }
        return false;
    }

    private Bounds GetRoomBounds(RoomIdentifier room)
    {
        Bounds? bounds = null;
        Vector3Int[] occupiedCoords = room.OccupiedCoords;
        for (int i = 0; i < occupiedCoords.Length; i++)
        {
            Vector3 center = RoomIdUtils.CoordsToCenterPos(occupiedCoords[i]);
            Bounds bounds2 = new Bounds(center, _boundsSize);
            if (bounds.HasValue)
            {
                bounds.Value.Encapsulate(bounds2);
            }
            else
            {
                bounds = bounds2;
            }
        }
        return bounds.Value;
    }
}
