using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DensityZoneManager : MonoBehaviour
{
    private GameObject[] zones;
    private Bounds[] zonesBounds;
    public int[] densities;
    public int densityMax = 50;
    public GameObject[] colorZones;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        zones = new GameObject[colliders.Length];
        zonesBounds = new Bounds[colliders.Length];
        densities = new int[colliders.Length];

        for (int i = 0; i < colliders.Length; i++)
        {
            zones[i] = colliders[i].gameObject;
            zonesBounds[i] = colliders[i].bounds;
            densities[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = LevelManager.instance.targetList;

        for (int j = 0; j < zonesBounds.Length; j++)
        {
            Bounds bound = zonesBounds[j];
            int enemiesInZone = 0;
            for (int i = 0; i < enemies.Length; i++)
            {
                if (bound.Contains(enemies[i].transform.position))
                    enemiesInZone++;
            }
            densities[j] = enemiesInZone;
        }

        GetZoneWhereDensityIsMoreThan(densityMax);

    }

    public void GetZoneWhereDensityIsMoreThan(int triggerValue)
    {
        for (int i = 0; i < densities.Length; i++)
        {
            if (densities[i] >= triggerValue)
            {
                colorZones[i].SetActive(true);
            } else if (densities[i] < triggerValue) {
                colorZones[i].SetActive(false);
            }
        }
    }


    public List<GameObject> GetEnemiesInZone(GameObject zone)
    {
        Bounds zoneBounds = zone.GetComponent<Collider2D>().bounds;
        List<GameObject> enemiesInZone = new List<GameObject>();

        if (zoneBounds != null)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Target");

            for (int i = 0; i < enemies.Length; i++)
            {
                if (zoneBounds.Contains(enemies[i].transform.position))
                    enemiesInZone.Add(enemies[i]);
            }

        }

        return enemiesInZone;
    }
}
