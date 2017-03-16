using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Room : MonoBehaviour, ICloneable
{
    // The values of this do matter
    public enum RoomSize { Small = 3, Medium = 2, Large = 1 };
    public enum RoomDifficulty { None = 0, Easy = 1, Medium = 2, Hard = 3 };

    [Tooltip("Set these for the room manager's generation")]
    [Header("RoomValues")]
    [SerializeField]
    public RoomSize mSize;
    [SerializeField]
    public RoomDifficulty mDifficulty;
    [SerializeField]
    public bool mTreasure;
    [SerializeField]
    public float mHeight;
    [SerializeField]
    public float mWidth;
    [SerializeField]
    public List<Door> mDoors = new List<Door>();

    EnemySpawner mSpawner;

    void Awake()
    {
        mSpawner = GetComponentInChildren<EnemySpawner>();
    }

    public void SpawnerInitialize()
    {
        if (mSpawner)
        {
            mSpawner.Initialize();
        }
    }
    public void SpawnerTerminate()
    {
        if (mSpawner)
        {
            mSpawner.Terminate();
        }
    }

    public void GenerateDoors()
    {
        for(int i = 0; i < mDoors.Count; ++i)
        {
            if (mDoors[i].RoomLink == null)
            {
                mDoors[i].RoomLink = RoomManager.Instance.IterateRoomlist();
                if(mDoors[i].RoomLink != null)
                    mDoors[i].RoomLink.LinkBack(this);
            }
        }
    }
    // Can return null if door doesn't exist in room
    public Door GetDoor(Room compareRoom)
    {
        foreach(Door door in mDoors)
        {
            if(door.RoomLink == compareRoom)
            {
                return door;
            }
        }

        return null;
    }

    public void LinkBack(Room linkback)
    {
        mDoors[0].RoomLink = linkback;
    }

    public List<Door> GetAllDoors()
    {
        return mDoors;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
