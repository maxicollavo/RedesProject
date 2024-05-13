using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public float BulletForce = 20f;
    public float speed = 5f;
    float moveHorizontal = 0;
    float moveVertical = 0;
    public float jumpForce = 5;
    bool isGrounded;
    public float crouchSpeed;
    private float startYScale;
    public float crouchYScale;
    public float rayDistance;
    bool onLever;
    Rigidbody _rb;
    [SerializeField] GameObject BulletPrefab;
    public Transform PlayerTransf;

    [SerializeField] Animator floorPlayerOneAnim;
    public Transform bulletPoint;

    private void Start()
    {
        startYScale = transform.localScale.y;
        crouchYScale = startYScale / 2;
        isGrounded = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Lever")
        {
            onLever = true;
            Debug.Log("Entro a palanca");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Lever")
        {
            onLever = false;
            Debug.Log("Salio de palanca");
        }
    }

    public override void Spawned() //Se ejecuta siempre que el objeto es instanciado en todos los clientes, preguntar si tengo autoridad
    {
        if (!HasStateAuthority) return;
        _rb = GetComponent<Rigidbody>();
    }

    public override void FixedUpdateNetwork()
    {
        moveVertical = 0f;
        moveHorizontal = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = 1f;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1f;
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        _rb.MovePosition(transform.position + movement * speed * Runner.DeltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
        }

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Ground"))
                isGrounded = true;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Se agacha");
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            Debug.Log("Se levanta");
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        /*if (onLever)
        {
            if (Input.GetKey(KeyCode.E))
            {
                floorPlayerOneAnim.SetBool("PlayerOneActivatedLever", true);

                if (floorPlayerOneAnim.GetBool("PlayerOneActivatedLever"))
                    floorPlayerOneAnim.SetBool("PlayerOneActivatedLever", false);
            }
        }*/
    }

    public void Shoot()
    {
        GameObject Bullet = Instantiate(BulletPrefab, bulletPoint.position, Quaternion.identity);
        Rigidbody _rb = Bullet.GetComponent<Rigidbody>();
        _rb.AddForce(bulletPoint.forward * BulletForce, ForceMode.Impulse);
        Destroy(Bullet, 2.5f);
    }

    public void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
