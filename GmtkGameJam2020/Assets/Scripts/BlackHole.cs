using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField]
    private float suckDistance = 10f;

    [SerializeField]
    private float blackHoleForce = 2f;

    [SerializeField]
    private GameObject explosionEffect;

    [SerializeField]
    private float explosionCooldown = 3f;

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, this.suckDistance);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(Tags.Pig))
            {
                Vector3 blackHoleForceVector = (this.transform.position - collider.transform.position) * this.blackHoleForce;
                blackHoleForceVector.y = 0f;
                collider.attachedRigidbody.AddForce(blackHoleForceVector);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, this.suckDistance);
    }

    public IEnumerator ExplodeCoroutine()
    {
        this.explosionEffect.SetActive(true);
        yield return new WaitForSeconds(this.explosionCooldown);
        this.explosionEffect.SetActive(false);
    }
}
