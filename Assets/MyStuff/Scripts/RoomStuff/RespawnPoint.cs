using UnityEngine;
using System.Collections;

public class RespawnPoint : MonoBehaviour
{
    bool KeyWait = true;

    void OnTriggerStay(Collider collider)
    {
        if(collider.tag == "Player")
        {
            // Show the player a button they can press to change rooms
            if (Input.GetKeyDown(KeyCode.E) && KeyWait)
            {
                RoomManager.Instance.mDoorConnectingToStartRoom.SetInactive();
                RoomManager.Instance.Generate();
                collider.transform.gameObject.GetComponent<Player>().Respawn();
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
}
