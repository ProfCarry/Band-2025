using Band.Extensions;
using System.Collections;
using UnityEngine;

namespace Band.Platform2D.Actions
{
    public class CharacterDamage : CharacterAction
    {
        [SerializeField]
        private float damageForce;

        [SerializeField]
        private float damageTime;

        [SerializeField]
        private int numOfIteractions;

        [SerializeField]
        private LayerMask damageLayerMask;

        protected override void Update()
        {
            
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.IsContainedLayer(damageLayerMask))
            {
                StopAllCoroutines();
                Vector3 vector = collision.GetContact(0).normal;
                vector += this.transform.up + this.transform.right*this.transform.localScale.x*(-1);
                vector = vector.normalized;
                print("Damage collision!");
                vector *= damageForce;
                vector -= vector.normalized * damageForce * Time.deltaTime;
                vector -= this.transform.up * damageForce * Time.deltaTime;
                output = vector;
                StartCoroutine(DamageRepulsion());
            }
            else
            {
                StopAllCoroutines();
                output = Vector3.zero;
            }
        }

        private IEnumerator DamageRepulsion()
        {
            int i = numOfIteractions;
            while (output.magnitude>0)
            {
                yield return new WaitForEndOfFrame();
                output -= this.transform.up *damageForce*Time.deltaTime;
                print(output);
                i--;
            }
            yield return new WaitForEndOfFrame();
            output = Vector3.zero;
        }



    }
}
