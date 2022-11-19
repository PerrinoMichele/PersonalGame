using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof (SphereCollider))]

public class PlayerController : MonoBehaviour
{
    public List<GameObject> ammo = new List<GameObject>();

    //public GameObject[] ammos = new GameObject[3];
    public GameObject redFruitPrefab;
    public GameObject yellowFruitPrefab;
    
    [SerializeField] float timeBetweenShots;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float moveSpeed;

    private float timer = 0;

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if (timer >= timeBetweenShots)
            {
                Shoot();
            }
        }

        else
        {
            rb.velocity = new Vector3(joystick.Horizontal * moveSpeed, rb.velocity.y, joystick.Vertical * moveSpeed);
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    void Shoot()
    {   
        if (ammo[0] != null)
        {
            GameObject shotFruit = Instantiate(ammo[0], new Vector3(transform.position.x, 1.5f, transform.position.z), Quaternion.identity);
            ammo.Remove(ammo[0]);
            shotFruit.GetComponent<Rigidbody>().AddForce(transform.forward * 300);
            timer = 0;
        }

    }

    void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "RedFruit") //&&ammo[ammo.length] not null
        {
            Destroy(other.gameObject);
            ammo.Add(redFruitPrefab);
        }
        if (other.gameObject.tag == "YellowFruit") //&&ammo[ammo.length] not null
        {
            Destroy(other.gameObject);
            ammo.Add(yellowFruitPrefab);
        }
    }
}