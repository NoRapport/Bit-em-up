using TopDownCharacter2D.FX;
using UnityEngine;
using System.Collections;

namespace TopDownCharacter2D.Health
{
    /// <summary>
    ///     Handles the removal of an entity when it dies
    /// </summary>

    public class DisappearOnDeath : MonoBehaviour
    {
        public Animator animator;
        public Rigidbody2D rb;
        public Collider2D col;
        public Collider2D col2;

        public GameObject lifeUpPrefab;
        public GameObject powerUpPrefab;

        public void OnDeath()
        {
            animator.SetTrigger("Death");
            gameObject.tag = "Dead";
            //rb.bodyType = RigidbodyType2D.Static;
            col.enabled = false;
            col2.enabled = false;
            StartCoroutine(WaitAndDie());
        }

        public IEnumerator WaitAndDie()
        {
          RandomDrop();
          // We wait before destroying the object in order to properly end all the related effects
          yield return new WaitForSeconds(0.8f);

          foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
          {
              if (!(component is AudioSource) && !(component is TopDownFx))
              {
                  component.enabled = false;
              }
          }

          foreach (Renderer component in transform.GetComponentsInChildren<Renderer>())
          {
              if (!(component is ParticleSystemRenderer))
              {
                  component.enabled = false;
              }
          }

          Destroy(gameObject);
        }

        public void RandomDrop()
        {
          float dice = Random.Range(0, 20);
          Debug.Log("Dice = " + dice);

          if (dice >= 15 && dice <= 18) {
            GameObject drop = Instantiate(lifeUpPrefab, transform.position, Quaternion.identity);
          } else if (dice >= 19) {
            GameObject drop = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
          }

        }
    }
}
