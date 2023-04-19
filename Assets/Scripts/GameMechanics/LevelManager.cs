using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int level = 0;
    public Text levelText;
    public Animator lvlAnimator;

    public GameObject[] targetList;

    public static LevelManager instance;

    public int rabbitNumber = 1;
    private int lastRabbitNumber;
    private int newRabbitNumber;

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
        level+=1;
        rabbitNumber+=10;
      }

      if(Input.GetKeyDown(KeyCode.M))
      {
        LaunchLevelSequence();
      }

      levelText.text = level.ToString();

      targetList = GameObject.FindGameObjectsWithTag("Target");

      if (targetList.Length == 0) {
        AutoSpawn();
      }

    }

    public void LaunchLevelSequence()
    {
      SpawnMinion.instance.minionSpawners = GameObject.FindGameObjectsWithTag("MinionSpawner");
      SpawnMinion.instance.minionSpawnersList = new List<GameObject>(SpawnMinion.instance.minionSpawners);
      StartCoroutine(SpawnMinion.instance.MinionWave("slimeGhost", rabbitNumber, 3));
    }

    public void AutoSpawn()
    {
      level+=1;
      lvlAnimator.SetTrigger("LvLUp");

      if (level <= 1) {
        rabbitNumber = 1;
      }
      if (level > 1) {
        newRabbitNumber = rabbitNumber + lastRabbitNumber;
        lastRabbitNumber = rabbitNumber;
        rabbitNumber = newRabbitNumber;
      }
      Debug.Log("Population de Lapins sur le niveau = " + rabbitNumber);

      LaunchLevelSequence();
    }
}
