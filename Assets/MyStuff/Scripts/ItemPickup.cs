using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    GameObject SpawnedItem;
    bool IsItemTaken;
    bool HasItemEvaporated;
    bool IsTimerSet;
    Timer EvaporationTimer = new Timer();

    void Awake()
    {
        Reset(5.0f);
    }

    void Reset(float waitTime)
    {
        EvaporationTimer.Initialize(waitTime);
        IsItemTaken = false;
        HasItemEvaporated = false;
        IsTimerSet = false;
    }

    void Update()
    {
        if(HasItemEvaporated == true)
        {
            Destroy(SpawnedItem);
            Destroy(gameObject);
        }
        else if (IsItemTaken == true)
        {
            // Used when the item is taken, but nothing was swapped
            if(SpawnedItem == null)
            {
                Destroy(gameObject);
            }
            // Used when the item is swapped with a previously held item
            else
            {
                IsItemTaken = false;
            }
        }
        else if(IsTimerSet == true)
        {
            EvaporationTimer.TimerAction(ItemEvaporation);
        }
    }

    void ItemEvaporation()
    {
        HasItemEvaporated = true;
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject != null && SpawnedItem != null && IsItemTaken == false)
        {
            Player player = collider.gameObject.GetComponent<Player>();

            if (player != null)
            {
                if (PickupUpdate(player) == true)
                {
                    IsItemTaken = true;
                }
            }
        }
    }

    public void SetItem(GameObject item)
    {
        SpawnedItem = item;
        IsTimerSet = true;
    }

    public bool PickupUpdate(Player activator)
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GrabItem("LeftHand", activator);
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            GrabItem("RightHand", activator);
            return true;
        }
        return false;
    }

    void GrabItem(string hand, Player activator)
    {
        if (hand == "LeftHand")
        {
            Transform leftHand = activator.transform.GetChild(0).GetChild(0);
            GameObject currentItem = SpawnedItem;
            DropCurrentItem(leftHand, activator);
            SetCurrentItemPosition(leftHand, currentItem);
            UI_HandWeapons.Instance.SetLeftHandImage(UI_HandWeapons.Instance.FolderNameToImageType(currentItem.tag));
        }
        else if (hand == "RightHand")
        {
            Transform rightHand = activator.transform.GetChild(0).GetChild(1);
            GameObject currentItem = SpawnedItem;
            DropCurrentItem(rightHand, activator);
            SetCurrentItemPosition(rightHand, currentItem);
            UI_HandWeapons.Instance.SetRightHandImage(UI_HandWeapons.Instance.FolderNameToImageType(currentItem.tag));
        }
        else
        {
            Debug.Log("[Chest::GrabItem] Invalid parameter");
        }
    }

    void DropCurrentItem(Transform parent, Player activator)
    {
        if (parent.childCount > 0)
        {
            GameObject currentItem = parent.GetChild(0).gameObject;

            if (currentItem != null)
            {
                // Delete the current item in the player's hand
                Destroy(currentItem);

                // Spawn the item on the ground
                currentItem = (GameObject)Instantiate(currentItem, activator.transform.position + activator.transform.forward, Quaternion.identity);
                // Reset the pickup timer
                Reset(2.0f);
                // Set item to this item pickup script
                SetItem(currentItem);
            }
        }
        else
        {
            SpawnedItem = null;
        }
    }

    void SetCurrentItemPosition(Transform parent, GameObject item)
    {
        Vector3 newPos = parent.position;
        Quaternion newRotate = parent.rotation;

        if (item.tag == "Shield")
        {
            newPos = new Vector3(parent.position.x, parent.position.y + 0.3f, parent.position.z);
        }
        item.transform.position = newPos;
        item.transform.rotation = newRotate;
        item.transform.parent = parent;
    }
}
