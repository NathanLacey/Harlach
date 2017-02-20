using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Room : MonoBehaviour, ICloneable
{
    public enum RoomSize { Small, Medium, Large };
    public enum RoomDifficulty { None, Easy, Medium, Hard };

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
    List<Door> mDoors = new List<Door>();

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
