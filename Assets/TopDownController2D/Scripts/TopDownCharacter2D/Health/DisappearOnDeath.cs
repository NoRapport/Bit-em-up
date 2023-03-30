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

        public void OnDeath()
        {
            animator.SetTrigger("Death");
            rb.bodyType = RigidbodyType2D.Static;
            StartCoroutine(WaitAndDie());
        }

        public IEnumerator WaitAndDie()
        {
          // We wait before destroying the object in order to properly end all the related effects
          Debug.Log("start waiting");
          yield return new WaitForSeconds(0.8f);
          Debug.Log("end waiting");

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
    }
}
