using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Tooltip("Dialogue Text Object")]
    [SerializeField] private GameObject DialogueTextObj;
    [Tooltip("Crosshair object")]
    [SerializeField] private GameObject CrosshairObj;

    [SerializeField] private Sprite[] CrosshairSprites;

    private Image CrosshairImage;
    private TextMeshProUGUI DialogueText;
    
    private Vector2 smoothing;
    private Vector2 result;
    private Transform character;
    private bool mouseLookEnabled = false;
    private bool displayObjectDescription = false;

    private int rayLengthMetres = 100;
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
    }

    // Start is called before the first frame update
    private void Start()
    {
        ToggleMouseLook(true, true);

        CrosshairImage = CrosshairObj.GetComponent<Image>();
        DialogueText = DialogueTextObj.GetComponent<TextMeshProUGUI>();

        clearObjectDialogue();
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

        //Interactions

        CrosshairImage.sprite = CrosshairSprites[1];

        ray = new Ray(transform.position, transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * rayLengthMetres, Color.green);
        if (Physics.Raycast(ray, out rayHit, rayLengthMetres) && rayHit.collider.CompareTag("Interact"))
        {
            CrosshairImage.sprite = CrosshairSprites[0];
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("foiwerwegewrg");
                displayObjectDescription ^= true;//flip bool
                handleObjectDialogue(rayHit.collider);
            }
        }
        else
        {
            displayObjectDescription = false;
            clearObjectDialogue();
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

        //ray = new Ray(transform.position, transform.forward);
        
        
    }

    private void clearObjectDialogue()
    {
        DialogueText.text = "";
    }

    private void handleObjectDialogue(Collider obj)
    {
        if(obj == null)
        {
            Debug.LogWarning("Null Object");
            return;
        }

        string textName = "";
        string textDescription = "";

        if (displayObjectDescription)
        {
            switch (obj.name)
            {
                case "PFB_Bed":
                    textName = "Bed";
                    textDescription = "bed";
                    break;
                case "PFB_Toilet":
                    textName = "Toilet";
                    textDescription = "toilet";
                    break;
                case "PFB_KitchenBench":
                    textName = "Kitchen Bench";
                    textDescription = "bench";
                    break;
                default:
                    textName = obj.name;
                    textDescription = "Unknown Object";
                    break;
            }
        }
        else
        {
            clearObjectDialogue();
        }


        DialogueText.text = string.Format("<b><u>{0}</u></b>\n{1}", textName, textDescription);
    }
}