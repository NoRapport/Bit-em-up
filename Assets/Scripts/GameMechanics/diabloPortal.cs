using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class diabloPortal : MonoBehaviour
{

  public GameObject tvSpawner;
  public GameObject globalLight;


  private void OnTriggerEnter2D(Collider2D collision)
  {
    if(collision.CompareTag("Player"))
    {
        collision.GetComponent<PlayerInput>().enabled = false;
        globalLight.GetComponent<Animator>().SetTrigger("FadeOut");
        StartCoroutine(teleportToTv(collision));
    }
  }

  public IEnumerator teleportToTv(Collider2D _collision)
  {
      yield return new WaitForSeconds(1);
      _collision.transform.position = tvSpawner.transform.position;
      yield return new WaitForSeconds(0.5f);
      _collision.transform.GetComponent<Animator>().SetTrigger("Spawning");
      yield return new WaitForSeconds(1);
      _collision.GetComponent<PlayerInput>().enabled = true;
  }

}
