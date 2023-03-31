using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int level = 0;
    public Text levelText;

    public static LevelManager instance;

    void Awake()
    {
      if (instance != null) {
        Debug.LogWarning("Il n'y a plus d'une instance de LevelManager dans la scene");
        return;
      }

      instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.L))
      {
        level+=5;
        Debug.Log(level);
      }

      if(Input.GetKeyDown(KeyCode.M))
      {
        LaunchLevelSequence();
      }

      levelText.text = level.ToString();
    }

    public void LaunchLevelSequence()
    {
      SpawnMinion.instance.minionSpawners = GameObject.FindGameObjectsWithTag("MinionSpawner");
      SpawnMinion.instance.minionSpawnersList = new List<GameObject>(SpawnMinion.instance.minionSpawners);
      StartCoroutine(SpawnMinion.instance.MinionWave("slimeGhost", level, 3));
    }
}
