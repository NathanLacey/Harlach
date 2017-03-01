using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    // Place one of these in scene that contains any dropable item at anypoint. Whether that be a chest or an enemy that drops an item
    private ItemSpawner() { }
    private static ItemSpawner TheInstance;
    public static ItemSpawner Instance
    {
        get
        {
            return TheInstance;
        }
    }

    public static Vector3 Right_Pos_Sword = new Vector3(0.8f, -0.7f, 0.8f);
    public static Vector3 Left_Pos_Sword = new Vector3(-0.8f, -0.7f, 0.8f);

    public static Vector3 Left_Pos_Shield = new Vector3(-0.5f, -0.7f, 0.5f);
    public static Vector3 Right_Pos_Shield = new Vector3(0.5f, -0.7f, 0.5f);
    [Tooltip("Sword, then shield, then magic")]
    [Header("SpawnableItems")]
    [SerializeField]
    public List<string> FolderNames = new List<string>();
    List<GameObject> AllItems = new List<GameObject>();
    [SerializeField]
    ParticleSystem ParticleSystemToSpawn;

    void Awake()
    {
        TheInstance = this;
    }

    public void SpawnRandomItem(Transform spawnPosition, ItemPickup currentItem, string folderChoice)
    {
        AllItems.Clear();
        int whichFolder = Random.Range(0, FolderNames.Count);
        // If someone wants to only spawn from a specific folder
        if (folderChoice != "any")
        {
            for(int i = 0; i < FolderNames.Count; ++i)
            {
                if(FolderNames[i] == folderChoice)
                {
                    whichFolder = i;
                    i = FolderNames.Count;
                }
            }
        }
        AllItems.AddRange(Resources.LoadAll<GameObject>(FolderNames[whichFolder]));
        int whichItem = Random.Range(0, AllItems.Count);

        StartCoroutine(WaitToSpawn(0.5f, AllItems[whichItem], spawnPosition, currentItem));
    }

    IEnumerator WaitToSpawn(float waitTime, GameObject item, Transform spawnPosition, ItemPickup currentItem)
    {
        yield return new WaitForSeconds(waitTime);
        currentItem.SetItem(Instantiate(item),  new Vector3(spawnPosition.position.x, spawnPosition.position.y + 1.0f, spawnPosition.position.z - 1.0f));
        currentItem.SetParticleSystem(new Vector3(spawnPosition.position.x + 0.5f, spawnPosition.position.y, spawnPosition.position.z - 2.0f), Quaternion.identity);
    }
}
