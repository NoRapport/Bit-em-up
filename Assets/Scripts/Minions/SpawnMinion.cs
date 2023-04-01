using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinion : MonoBehaviour
{
    public GameObject[] minionSpawners;
    public List<GameObject> minionSpawnersList;
    public GameObject minionSpawner;

    public GameObject slimeGhostPrefab;

    public static SpawnMinion instance;

    void Awake()
    {
      if (instance != null) {
        Debug.LogWarning("Il n'y a plus d'une instance de SpawnMinion dans la scene");
        return;
      }

      instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
      minionSpawners = GameObject.FindGameObjectsWithTag("MinionSpawner");
      minionSpawnersList = new List<GameObject>(minionSpawners);
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void GetRandomSpawner()
    {
      if (minionSpawnersList.Count == 1)
      {
          minionSpawner = minionSpawnersList[0];
          minionSpawnersList = new List<GameObject>(minionSpawners);
      }
      int index = Random.Range(0, minionSpawnersList.Count);
      minionSpawner = minionSpawnersList[index];
      minionSpawnersList.RemoveAt(index);
    }

    public void MinionSpawnByName(string minionType)
    {
      if (minionType == "slimeGhost") {
        GetRandomSpawner();
        Instantiate(slimeGhostPrefab, minionSpawner.transform.position, Quaternion.identity);
      }
    }

    //Minion Sequence
    public IEnumerator MinionWave(string minionType, int numberOfMinions, float spawnBreak)
    {
        for (int i = 0; i < numberOfMinions*5; i++) {
          MinionSpawnByName(minionType);
          yield return new WaitForSeconds(spawnBreak * 0.1f);
        }
    }

}
