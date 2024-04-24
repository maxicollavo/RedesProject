using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public float speed = 5f;
    float moveHorizontal = 0;
    float moveVertical = 0;
    float jumpForce = 5;
    bool isGrounded = true;
    bool onLever;
    Rigidbody _rb;

    [SerializeField] Animator floorPlayerOneAnim;

    /*private void FixedUpdate()
    {
        moveHorizontal = 0f;
        moveVertical = Input.GetAxis("Vertical");
    }*/

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Debug.Log("Salta");
                isGrounded = false;
                Jump();
            }
        }
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
        //base.Spawned();
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
            moveVertical = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveVertical = -1f;
        }

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        _rb.MovePosition(transform.position + movement * speed * Runner.DeltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                isGrounded = false;
                Jump();
            }
        }
    }

    public void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
