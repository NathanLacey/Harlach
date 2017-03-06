using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject mEnemyToSpawn;
    [SerializeField]
    GameObject mEnemyWithDrop;
    Room mParent;
    float mTimeBetweenSpawns;
    List<GameObject> mSpawnedEnemies = new List<GameObject>();

    public void Initialize()
    {
        mParent = transform.parent.GetComponent<Room>();
        if (mParent.mDifficulty != Room.RoomDifficulty.None)
        {
            GenerateValues();
            InvokeRepeating("SpawnEnemy", 3.0f, mTimeBetweenSpawns);
        }
    }

    public void Terminate()
    {
        CancelInvoke();
        foreach (GameObject enemy in mSpawnedEnemies)
        {
            Destroy(enemy);
        }
    }

    void GenerateValues()
    {
        mTimeBetweenSpawns = 20.0f;
        mTimeBetweenSpawns *= (float)mParent.mDifficulty;
        mTimeBetweenSpawns *= (float)mParent.mSize;
    }

    void SpawnEnemy()
    {
        GameObject go;
        if (Random.Range(0, 10) == 0)
        {
            go = Instantiate(mEnemyWithDrop, transform.position, transform.rotation) as GameObject;
        }
        else
        {
            go = Instantiate(mEnemyToSpawn, transform.position, transform.rotation) as GameObject;
        }
        mSpawnedEnemies.Add(go);
        Enemy temp = go.GetComponent<Enemy>();
        temp.GenerateValues(mParent.mDifficulty);
    }
}
