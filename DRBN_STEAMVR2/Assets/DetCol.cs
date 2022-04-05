using UnityEngine;
using System.Collections;

public class DetCol : MonoBehaviour
{
    private Vector3 center;
    private float radius = 1.5f;

    
    void Collisions(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        Debug.Log("  length of hitColliders  " + hitColliders.Length);

        foreach (var hitCollider in hitColliders)
        {
            Debug.Log("  hitCollider  "+hitCollider.name);
        }
    }

    void Update()
    {
        center = this.gameObject.transform.position;
        Collisions(center,radius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(center,radius);
    }

}

