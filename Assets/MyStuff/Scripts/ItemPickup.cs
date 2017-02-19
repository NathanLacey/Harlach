using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
    Transform SpawnedItemParent;
    [SerializeField]
    GameObject SpawnedItem;
    [SerializeField]
    ParticleEffect SpawnedParticleSystem;

    bool IsItemTaken;
    bool HasItemEvaporated;
    bool IsTimerSet;
    Timer EvaporationTimer = new Timer();

    void Awake()
    {
        SpawnedItemParent = transform;
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
            SpawnedParticleSystem.Terminate(3.0f);
            Destroy(gameObject);
        }
        else if (IsItemTaken == true)
        {
            // Used when the item is taken, but nothing was swapped
            if(SpawnedItem == null)
            {
                SpawnedParticleSystem.Terminate(3.0f);
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

    public void SetItem(GameObject item, Vector3 pos)
    {
        SpawnedItemParent.position = pos;
        item.transform.parent = SpawnedItemParent;
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        SpawnedItem = item;
        if(SpawnedItem.GetComponent<AttackController>().IsSword() == false && SpawnedItem.GetComponent<Animator>().runtimeAnimatorController == null)
        {
            SpawnedItem.GetComponent<AttackController>().LoadAnimator("Animators/ItemAnimator");
        }
        SpawnedItem.GetComponent<AttackController>().GetComponent<Animator>().SetBool("FloatingItem", true);
        IsTimerSet = true;
    }

    public void SetParticleSystem(Vector3 position, Quaternion rotation)
    {
        SpawnedParticleSystem.Initialize(position, rotation);
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
            currentItem.GetComponent<Animator>().SetBool("FloatingItem", false);
            if (currentItem.GetComponent<AttackController>().IsSword() == false)
            {
                currentItem.GetComponent<AttackController>().LoadAnimator("NULL");
            }
            DropCurrentItem(leftHand, activator);
            SetCurrentItemPosition(leftHand, currentItem);
            UI_HandWeapons.Instance.SetLeftHandImage(UI_HandWeapons.Instance.FolderNameToImageType(currentItem.tag));
            SetAttackController(hand, activator, currentItem);
        }
        else if (hand == "RightHand")
        {
            Transform rightHand = activator.transform.GetChild(0).GetChild(1);
            GameObject currentItem = SpawnedItem;
            currentItem.GetComponent<Animator>().SetBool("FloatingItem", false);
            if(currentItem.GetComponent<AttackController>().IsSword() == false)
            {
                currentItem.GetComponent<AttackController>().LoadAnimator("NULL");
            }
            DropCurrentItem(rightHand, activator);
            SetCurrentItemPosition(rightHand, currentItem);
            UI_HandWeapons.Instance.SetRightHandImage(UI_HandWeapons.Instance.FolderNameToImageType(currentItem.tag));
            SetAttackController(hand, activator, currentItem);
        }
        else
        {
            Debug.Log("[Chest::GrabItem] Invalid parameter");
        }
    }

    void SetAttackController(string hand, Player activator, GameObject currentItem)
    {
        if(hand == "LeftHand")
        {
            activator.mAttackControllerLeft = currentItem.GetComponent<AttackController>();
            if (currentItem.tag == "sword1h")
            {
                activator.mAttackControllerLeft.GetComponent<Animator>().SetBool("Mirror", true);
            }
        }
        else if(hand == "RightHand")
        {
            activator.mAttackControllerRight = currentItem.GetComponent<AttackController>();
        }
    }

    void DropCurrentItem(Transform parent, Player activator)
    {
        if (parent.childCount > 0)
        {
            GameObject currentItem = parent.GetChild(0).gameObject;

            if (currentItem != null)
            {
                //// Delete the current item in the player's hand
                //Destroy(currentItem);

                //// Spawn the item on the ground
                //currentItem = Instantiate(currentItem);
                // Reset the pickup timer
                Reset(2.0f);
                // Set item to this item pickup script
                SetItem(currentItem, activator.transform.position + activator.transform.forward);
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

        if (item.tag == "shield")
        {
            newPos = new Vector3(parent.position.x, parent.position.y + 0.3f, parent.position.z);
        }
        else if(item.tag == "wand")
        {
            newPos = new Vector3(parent.position.x, parent.position.y + 0.2f, parent.position.z - 0.3f);
        }

        item.transform.position = newPos;
        item.transform.rotation = newRotate;
        item.transform.parent = parent;

        if(item.tag == "wand")
        {
            item.transform.Rotate(new Vector3(1.0f, 0.0f, 0.0f), 80.0f);
        }
        else if (item.tag == "sword1h")
        {
            item.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
        }
    }
}
