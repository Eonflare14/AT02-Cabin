using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class should be attatched to the player, it controlls the movement (including gravity) of the player it is attatched to
/// </summary>
/// <lastUpdated> 2023-08-21 </lastUpdated>


public class PlayerMove : MonoBehaviour
{

    [SerializeField] float moveSpeed;

    private float gravity = -5f;

    private CharacterController c_c;

    void Start()
    {
        c_c = GetComponent<CharacterController>();

        if(c_c == null)
        {
            Debug.LogWarning("Player should have a valid character controller component");
        }
    }

    void Update()
    {

        float Xdirection = Input.GetAxis("Horizontal");
        float Zdirection = Input.GetAxis("Vertical");

        c_c.Move(transform.right * Xdirection * moveSpeed * Time.deltaTime);
        c_c.Move(transform.forward * Zdirection * moveSpeed * Time.deltaTime);

        c_c.Move(transform.up * gravity * Time.deltaTime);
    }
}
