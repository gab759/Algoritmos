using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject objetivo;
    public Vector2 speedReference;
    public Transform playerTransform;
    private float timer = 20f;
    private bool isMoving = true;

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.SmoothDamp(transform.position, objetivo.transform.position, ref speedReference, 0.5f);

            if (timer <= 0f)
            {
                StopMovement();
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else
        {
            transform.position = new Vector3(0f, transform.position.y, transform.position.z);

            if (timer <= -10f)
            {
                RestartMovement();
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Node" && isMoving)
        {
            objetivo = collision.gameObject.GetComponent<NodeController>().SelecRandomAdjacent().gameObject;
        }
        if (collision.gameObject.tag == "Player")
        {
            objetivo = playerTransform.gameObject;
            RotateTowardsTarget(playerTransform);
        }
    }

    private void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 180));
    }
    private void StopMovement()
    {
        isMoving = false;
        timer = 10f;
    }

    private void RestartMovement()
    {
        isMoving = true;
        timer = 20f;
    }
}