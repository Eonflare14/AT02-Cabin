using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class should be attached to the main camera.
/// </summary>
public class CameraHandler : MonoBehaviour
{
    [Tooltip("The amount of influence mouse input has on camera movement. Must have a value above 0.")]
    [SerializeField] private float sensitivity;
    [Tooltip("The amount of 'drag' applied to the camera. Must have a value above 0.")]
    [SerializeField] private float drag;
    [Tooltip("The minimum and maximum angle that the camera can move on the y axis.")]
    [SerializeField] private Vector2 verticalClamp = new Vector2(-45, 70);

    [Tooltip("Crossharir object")]
    [SerializeField] private GameObject CrosshairObj;

    [SerializeField] private Sprite[] CrosshairSprites;

    private Image CrosshairImage;
    
    private Vector2 smoothing;
    private Vector2 result;
    private Transform character;
    private bool mouseLookEnabled = false;

    private int rayLengthMetres = 10;
    private Ray ray;
    private RaycastHit rayHit;

    /// <summary>
    /// Use to turn mouse look on and off. To toggle cursor, use ToggleMouseLook method.
    /// </summary>
    public bool MouseLookEnabled { get { return mouseLookEnabled; } set { ToggleMouseLook(value); } }

    //Awake is executed before the Start method
    private void Awake()
    {
        if (transform.parent != null)
        {
            character = transform.parent;
        }
        else 
        {
            Debug.LogWarning($"{name} should be the child of an empty object!");
        }

        /*if(transform.localPosition != Vector3.zero) 
        {
            Debug.LogWarning($"{name} should have a local space of (0,0,0)!");
        }*/
    }

    // Start is called before the first frame update
    private void Start()
    {
        ToggleMouseLook(true, true);

        CrosshairImage = CrosshairObj.GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Mouse Look
        if (mouseLookEnabled == true)
        {
            var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            md = Vector2.Scale(md, new Vector2(sensitivity * drag, sensitivity * drag));
            smoothing.x = Mathf.Lerp(smoothing.x, md.x, 1f / drag);
            smoothing.y = Mathf.Lerp(smoothing.y, md.y, 1f / drag);
            result += smoothing;
            result.y = Mathf.Clamp(result.y, verticalClamp.x, verticalClamp.y);

            transform.localRotation = Quaternion.AngleAxis(-result.y, Vector3.right);
            character.localRotation = Quaternion.AngleAxis(result.x, character.up);
        }
    }

    /// <summary>
    /// Toggles the mouse look on and off.
    /// Can optionally toggle the mouse cursor on and off.
    /// </summary>
    /// <param name="mouseLookActive"></param>
    /// <param name="toggleCursor"></param>
    public void ToggleMouseLook(bool mouseLookActive, bool toggleCursor = false)
    {
        //Mouse Look
        mouseLookEnabled = mouseLookActive;
        if (toggleCursor == true)
        {
            if (mouseLookActive == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
            Cursor.visible = !mouseLookActive;
        }
        //Interaction

        ray = new Ray(transform.position, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * rayLengthMetres, Color.green);
        if (Physics.Raycast(ray, out rayHit, rayLengthMetres) )
        {
            Debug.Log("Ray Hit!");

            if (rayHit.collider.CompareTag("Interact"))
            {
                setCrosshairType(0);
            }
        }
        else
        {
            setCrosshairType(1);
        }

        
    }

    void setCrosshairType (int type)
    {
        CrosshairImage.sprite = CrosshairSprites[type];
    }
}
