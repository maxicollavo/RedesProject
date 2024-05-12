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
    public bool isGrounded = true;
    public float crouchSpeed;
    private float startYScale;
    public float crouchYScale;
    bool onLever;
    Rigidbody _rb;
    [SerializeField] GameObject BulletPrefab;
    public Transform PlayerTransf;

    [SerializeField] Animator floorPlayerOneAnim;

    private void Start()
    {
        startYScale = transform.localScale.y;
        crouchYScale = startYScale / 2;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Lever")
        {
            onLever = true;
            Debug.Log("Entro a palanca");
        }

        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
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

        if (onLever)
        {
            if (Input.GetKey(KeyCode.E))
            {
                floorPlayerOneAnim.SetBool("PlayerOneActivatedLever", true);

                if (floorPlayerOneAnim.GetBool("PlayerOneActivatedLever"))
                    floorPlayerOneAnim.SetBool("PlayerOneActivatedLever", false);
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1f;
        }

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        _rb.MovePosition(transform.position + movement * speed * Runner.DeltaTime);
        transform.forward = movement;

        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
        }
        if (Input.GetMouseButtonDown(0)) 
        {
            GameObject Bullet = Instantiate(BulletPrefab, PlayerTransf.position, PlayerTransf.rotation);
            Rigidbody _rb = Bullet.GetComponent<Rigidbody>();
            _rb.AddForce(PlayerTransf.forward * BulletForce, ForceMode.Impulse); 
        }
    }

    public void Jump()
    {
        Debug.Log("Salta");
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }
}
