using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float splash = 1f;
    [SerializeField] private float offset = 90f;
    [HideInInspector] public Transform Target {private get; set;}

    // Update is called once per frame
    void Update() => MoveToTarget();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Target.tag)
            Destroy(gameObject);
    }

    private void MoveToTarget()
    {
        if (Target != null)
        {
            transform.position =  Vector2.MoveTowards(transform.position, Target.position, moveSpeed * Time.deltaTime);
            Vector2 direction = Target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, moveSpeed * Time.deltaTime);
        }
    }
}
