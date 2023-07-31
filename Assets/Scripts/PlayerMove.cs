using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class should be attatched to the player
/// </summary>


public class PlayerMove : MonoBehaviour
{

    [SerializeField] float moveSpeed;

    private float gravity = -5f;

    private CharacterController c_c;

    // Start is called before the first frame update
    void Start()
    {
        c_c = GetComponent<CharacterController>();

        if(c_c == null)
        {
            Debug.LogWarning("Player should have a valid character controller component");
        }
    }

    // Update is called once per frame
    void Update()
    {

        float Xdirection = Input.GetAxis("Horizontal");
        float Zdirection = Input.GetAxis("Vertical");

        c_c.Move(transform.right * Xdirection * moveSpeed * Time.deltaTime);
        c_c.Move(transform.forward * Zdirection * moveSpeed * Time.deltaTime);

        c_c.Move(transform.up * gravity * Time.deltaTime);
    }
}
