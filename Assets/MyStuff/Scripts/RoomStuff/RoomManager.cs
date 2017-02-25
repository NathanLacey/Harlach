using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    // Singleton Stuff
    private RoomManager() { }
    private static RoomManager TheInstance;
    public static RoomManager Instance
    {
        get
        {
            return TheInstance;
        }
    }
    void Awake()
    {
        TheInstance = this;
        GenerateRoomArray();
    }
    //
    // All the rooms from the resources folder
    [SerializeField]
    List<Room> mAllRooms = new List<Room>();
    // The rooms that are going to be spawned
    public List<Room> mRoomList = new List<Room>();
    [SerializeField]
    Room mLastRoom;
    [SerializeField]
    Room mCurrentRoom;
    public const uint mInitialRoomCount = 10;
    public int mCurrentRoomCount = 0;
    public Vector3 mPositionOffset;
    
    void GenerateRoomArray()
    {
        // Clear possible previous rooms
        mAllRooms.Clear();
        // Load in all possible rooms
        mAllRooms.AddRange(Resources.LoadAll<Room>("Rooms"));
        // Instantiate All rooms
        for(int i = 0; i < mInitialRoomCount - 1; ++i)
        {
            mRoomList.Add((Room)Instantiate(GetRandomNullRoom()));
        }
        mRoomList.Add((Room)Instantiate(mLastRoom));
        mCurrentRoom = mRoomList[0];
        ++mCurrentRoomCount;
        // Generate room position and links
        for(int i = 0; i < mRoomList.Count; ++i)
        {
            GenerateRoom(mRoomList[i], i * mPositionOffset);
        }
        
    }

    public Room IterateRoomlist()
    {
        if (mCurrentRoomCount >= mInitialRoomCount)
            return null;
        return mRoomList[mCurrentRoomCount++];
    }

    void GenerateRoom(Room room, Vector3 position)
    {
        // Initialize room
        room.transform.position = position;
        room.GenerateDoors();
        //// Check each door in the room and if it connects to another room then make it active
        //foreach(Door door in room.GetAllDoors())
        //{
        //    if(door.RoomLink != null)
        //    {
        //        door.gameObject.SetActive(true);
        //    }
        //}
    }

    public Room GetRandomNullRoom()
    {
        return mAllRooms[Random.Range(0, mAllRooms.Count)];
    }

    public Room GetRandomExistingRoom()
    {
        return mRoomList[Random.Range(0, mRoomList.Count)];
    }

    public void ChangeRoom(Room room, Transform player)
    {
        Debug.Log("HERE");
        // Check through room to see which door connects to the current room
        Transform teleportPoint = room.GetDoor(mCurrentRoom).transform;
        mCurrentRoom = room;
        // Do some sort of fading out fading in screen
        // Move player's position to that of the door, but turned away from the door
        player.transform.position = teleportPoint.position;
    }
}
