using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject objetivo;
    public Vector2 speedReference;
    public Transform playerTransform;
    public float fieldOfViewAngle = 60f; 
    public float viewDistance = 5f; 
    public int lineResolution = 50; 
    public LayerMask detectionLayer; 
    private float timer = 20f;
    private bool isMoving = true;
    private LineRenderer lineRenderer;
    private bool isChasing = false; 
    private bool playerDetected = false; 

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = lineResolution + 3;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.useWorldSpace = true; 

        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

        UpdateVisionCone();
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
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
            CheckPlayerVision();
        }
        UpdateVisionCone();
    }

    private void UpdateVisionCone()
    {
        lineRenderer.SetPosition(0, transform.position);

        float halfFOV = fieldOfViewAngle / 2f;
        float angleStep = fieldOfViewAngle / (lineResolution - 1);
        float angle = -halfFOV;

        for (int i = 1; i <= lineResolution + 1; i++)
        {
            float rad = Mathf.Deg2Rad * (angle + transform.eulerAngles.z + 90f);
            Vector3 pos = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * viewDistance;
            lineRenderer.SetPosition(i, transform.position + pos);

            angle += angleStep;
        }
        lineRenderer.SetPosition(lineResolution + 2, transform.position);
    }

    private void CheckPlayerVision()
    {
        float halfFOV = fieldOfViewAngle / 2f;
        float angleStep = fieldOfViewAngle / (lineResolution - 1);
        float angle = -halfFOV;

        playerDetected = false;

        for (int i = 0; i < lineResolution; i++)
        {
            float rad = Mathf.Deg2Rad * (angle + transform.eulerAngles.z + 90f);
            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewDistance, detectionLayer);

            Debug.DrawRay(transform.position, direction * viewDistance, Color.red);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                Debug.Log("Ahora si detecta?");
                playerDetected = true;
                isChasing = true;
                break;
            }

            angle += angleStep;
        }

        if (!playerDetected && isChasing)
        {
            Debug.Log("jugador salio del raycast");
            isChasing = false;
            RestartMovement();
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, 3f * Time.deltaTime);
        RotateTowardsTarget(playerTransform);

        CheckPlayerVision();
    }

    private void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); 
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Node" && isMoving)
        {
            objetivo = collision.gameObject.GetComponent<NodeController>().SelecRandomAdjacent().gameObject;
        }
    }
}