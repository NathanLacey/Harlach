using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{ 
    [SerializeField]
    Animator MyAnimator;
    bool IsItemSpawned;
    [SerializeField]
    ItemPickup CurrentItem;
    [Tooltip("This item type will be the name of the folder used in item spawner. Put 'any' if you don't want to specify the random item type")]
    [SerializeField]
    string ItemType;

    void Awake()
    {
        IsItemSpawned = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject != null && IsItemSpawned == false)
        {
            Player player = collider.gameObject.GetComponent<Player>();

            if(player != null)
            {
                CurrentItem = (ItemPickup)Instantiate(CurrentItem, transform.position, transform.rotation);
                OpenChest();
            }
        }
    }

    void CloseChest()
    {
        MyAnimator.SetTrigger("Close");
    }

    void OpenChest()
    {
        MyAnimator.SetTrigger("Open");
        ItemSpawner.Instance.SpawnRandomItem(transform, CurrentItem, ItemType);
        IsItemSpawned = true;
    }
}
