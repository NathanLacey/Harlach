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
        GetRoomsFromResources();
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
    Room mStartRoom;
    Room mCurrentRoom;
    public Door mDoorConnectingToStartRoom;

    [SerializeField]
    bool mInstantiateFirstRoom;
    public const uint mInitialRoomCount = 3;
    public int mCurrentRoomCount = 0;
    public Vector3 mPositionOffset;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            ClearRoomArray();
        }
    }

    public void Generate()
    {
        ClearRoomArray();
        GenerateRoomArray();
    }
    void GetRoomsFromResources()
    {
        // Clear possible previous rooms
        mAllRooms.Clear();
        // Load in all possible rooms
        mAllRooms.AddRange(Resources.LoadAll<Room>("Rooms"));
    }

    void GenerateRoomArray()
    {
        // Instantiate All rooms
        if(mInstantiateFirstRoom == true)
        {
            mRoomList.Add((Room)Instantiate(mStartRoom));
        }
        else
        {
            mRoomList.Add(mStartRoom);
        }

        for (int i = 1; i < mInitialRoomCount - 1; ++i)
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

        // For figuring out what door to make inactive
        mDoorConnectingToStartRoom = mRoomList[0].GetDoor(mRoomList[1]);
    }

    void ClearRoomArray()
    {
        if(mInstantiateFirstRoom == true && mRoomList.Count > 0)
        {
            Destroy(mRoomList[0]);
        }
        for(int i = 1; i < mRoomList.Count; ++i)
        {
            Destroy(mRoomList[i].gameObject);
        }

        mRoomList.Clear();
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
        room.transform.position += position;
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
        // Check through room to see which door connects to the current room
        Transform teleportPoint = room.GetDoor(mCurrentRoom).transform;
        // Start Enemy Spawner on the room you are entering and stop the one from the room you are leaving
        mCurrentRoom.SpawnerTerminate();
        mCurrentRoom = room;
        mCurrentRoom.SpawnerInitialize();
        // Do some sort of fading out fading in screen
        UI_ScreenFadeout.Instance.Fade(15.0f);
        // Move player's position to that of the door, but turned away from the door
        StartCoroutine(TransportPlayer(1.0f, player, teleportPoint));
    }

    IEnumerator TransportPlayer(float waitTime, Transform player, Transform teleportPoint)
    {
        yield return new WaitForSeconds(waitTime);
        player.transform.position = teleportPoint.position;
    }
}
