using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject mEnemyToSpawn;
    [SerializeField]
    GameObject mEnemyWithDrop;
    Room mParent;
    float mTimeBetweenSpawns;
    
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
        Enemy temp = go.GetComponent<Enemy>();
        temp.GenerateValues(mParent.mDifficulty);
    }
}
