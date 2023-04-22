using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diabloPortal : MonoBehaviour
{

  public GameObject tvSpawner;
  public GameObject globalLight;


  private void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision.CompareTag("Player"))
    {
        globalLight.GetComponent<Animator>().SetTrigger("FadeOut");
        StartCoroutine(teleportToTv(collision));
    }
  }

  public IEnumerator teleportToTv(Collider2D _collision)
  {
      yield return new WaitForSeconds(0.42f);
      _collision.transform.position = tvSpawner.transform.position;
      _collision.transform.GetComponent<Animator>().SetTrigger("Spawning");
  }

}
