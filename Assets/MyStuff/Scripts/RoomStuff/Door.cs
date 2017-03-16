using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    [SerializeField]
    BoxCollider Trigger;
    bool KeyWait = true;

    [SerializeField]
    private Room mRoomLink;
    public Room RoomLink
    {
        get
        {
            return mRoomLink;
        }
        set
        {
            mRoomLink = value;

            if (mRoomLink != null)
            {
                gameObject.SetActive(true);
            }
        }
    }

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerStay(Collider collider)
    {
        if(collider.tag == "Player" && RoomLink != null)
        {
            // Show the player a button they can press to change rooms
            if(Input.GetKeyDown(KeyCode.E) && KeyWait)
            {
                RoomManager.Instance.ChangeRoom(RoomLink, collider.transform);
                StartCoroutine(KeyPressWait(0.1f));
            }
        }
    }

    IEnumerator KeyPressWait(float waitTime)
    {
        KeyWait = false;
        yield return new WaitForSeconds(waitTime);
        KeyWait = true;
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }
}
