using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D _compRigidbody2D;
    private Vector2 moveInput;

    private void Awake()
    {
        _compRigidbody2D = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();
    }

    void FixedUpdate()
    {
        _compRigidbody2D.velocity = moveInput * moveSpeed;
    }
}
