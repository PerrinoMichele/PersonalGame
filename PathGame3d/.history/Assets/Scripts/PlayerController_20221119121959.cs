using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof (SphereCollider))]

public class PlayerController : MonoBehaviour
{
    public GameObject fruitPrefab;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float moveSpeed;

    void FixedUpdate()
    {
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Shoot();
        }

        else
        {
            rb.velocity = new Vector3(joystick.Horizontal * moveSpeed, rb.velocity.y, joystick.Vertical * moveSpeed);
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    void Shoot()
    {
        Instantiate(fruitPrefab, new Vector3(0,0,0), Quaternion.identity);
    }
}