using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GunType
{
    Pistol,
    Rifle,
    Shotgun
}
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidBody;
    [SerializeField] private Vector2 _movement;
    [SerializeField] float speed;

    [SerializeField] Camera cam;
    [SerializeField] private Transform prompt;

    Animator anime;
    Vector3 mouse;

    private GunType currentGun = GunType.Pistol;
    //Gun Damage / Gun Range / Gun Ammo / Rate of Fire
    private Vector4[] statBlocks = { new Vector4(5f, 8f, 10, 0.2f), new Vector4(8f, 14f, 10, 0.05f), new Vector4(20f, 5f, 10, 0.75f) };
    private float timeTilFire = 0f;


    [SerializeField] private TextMeshProUGUI ammoDisplay;
    [SerializeField] private TextMeshProUGUI pointsDisplay;

    [SerializeField] private Image ammoIcon;

    [SerializeField] private Sprite[] bulletIcons;
    [SerializeField] private AudioClip[] bulletNoise;
    [SerializeField] private AudioClip empty;

    private Inputs _input;
    private bool _inTrigger;
    public GameObject interactObject;
    private float barricadeHoldTimer = 2f;
    private float barricadeHoldCount = 0;

    private int points = 100;

    private void Awake()
    {
        _input = new Inputs();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Movement.performed += Movement;
        _input.Player.Movement.canceled += StopMovement;
        _input.Actions.InputPressedBar.performed += InputPressedF;
        _input.Actions.InputPressedFire.performed += PressedFire;
        _input.Actions.InputPressedFire.canceled += ReleasedFire;
    }

    void Start()
    {
        _inTrigger = false;
        _movement = Vector2.zero;
        rigidBody = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();

        ammoDisplay.text = statBlocks[(int)currentGun].z.ToString();
        ammoIcon.sprite = bulletIcons[(int)currentGun];
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = _movement * speed;
    }

    void Update()
    {
        mouse = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        float angle = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);

        if (timeTilFire > -1)
            timeTilFire -= Time.deltaTime;

        SetAnimStates();
        ammoDisplay.text = statBlocks[(int)currentGun].z.ToString();
        ammoIcon.sprite = bulletIcons[(int)currentGun];
        pointsDisplay.text =  "Points: " + points.ToString();
    }

    private void SetAnimStates()
    {
        if(_movement.magnitude == 0)
        {
            anime.SetBool("idle", true);
        }
        else
        {
            anime.SetBool("idle", false);
        }
    }

    private void Movement(InputAction.CallbackContext value)
    {
        _movement = value.ReadValue<Vector2>();
    }

    private void StopMovement(InputAction.CallbackContext value)
    {
        _movement = Vector2.zero;
    }

    public void DisplayPrompt()
    {
        prompt.gameObject.SetActive(true);
    }
    public void HidePrompt()
    {
        prompt.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Movement.performed -= Movement;
        _input.Player.Movement.canceled -= StopMovement;
        _input.Actions.InputPressedBar.performed -= InputPressedF;
        _input.Actions.InputPressedFire.performed -= PressedFire;
        _input.Actions.InputPressedFire.canceled -= ReleasedFire;
    }

    public void InputPressedF(InputAction.CallbackContext value)
    {
        if (_inTrigger && interactObject.CompareTag("Structure"))
        {
            interactObject.GetComponent<Health>().ResetHealth();
            interactObject.GetComponent<SpriteRenderer>().enabled = true;
            interactObject.GetComponent<BoxCollider2D>().enabled = true;
            _inTrigger = false;
        }
        else if (_inTrigger && interactObject.CompareTag("Gun"))
        {
            PurchaseGun(interactObject);
            _inTrigger = false;
        }
    }

    public void PressedFire(InputAction.CallbackContext value)
    {
        if (timeTilFire <= 0 && statBlocks[(int)currentGun].z > 0)
        {
            Vector3 mouseDir = new Vector3(mouse.x - transform.position.x, mouse.y - transform.position.y, 0f).normalized;
            mouseDir *= statBlocks[(int)currentGun].y;
            Debug.DrawRay(transform.position, mouseDir, Color.red, 0.3f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, mouseDir, statBlocks[(int)currentGun].y * 2, LayerMask.GetMask("Map", "Zombies"));
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Zombie"))
                {
                    hit.collider.GetComponent<Health>().ReduceHealth(statBlocks[(int)currentGun].x);
                    statBlocks[(int)currentGun].z++;
                    hit.collider.GetComponent<Health>().ReduceHealth(statBlocks[(int)currentGun].x);
                    statBlocks[(int)currentGun].z++;
                    points += (int)statBlocks[(int)currentGun].x;
                }
            }

            statBlocks[(int)currentGun].z--;
            AudioSource.PlayClipAtPoint(bulletNoise[(int)currentGun], new Vector3(0, 0, 0));
            timeTilFire = statBlocks[(int)currentGun].w;
        }
        else if (statBlocks[(int)currentGun].z <= 0)
        {
            AudioSource.PlayClipAtPoint(empty, new Vector3(0, 0, 0));
            timeTilFire = statBlocks[(int)currentGun].w;
        }

        ammoDisplay.text = statBlocks[(int)currentGun].z.ToString();
        ammoIcon.sprite = bulletIcons[(int)currentGun];
    }

    public void ReleasedFire(InputAction.CallbackContext value)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Structure"))
        {
            GameObject barricade = collision.transform.parent.gameObject;
            if (!barricade.GetComponent<BoxCollider2D>().enabled)
            {
                DisplayPrompt();
                _inTrigger = true;
                interactObject = barricade;
            }
        }
        else if (collision.CompareTag("Gun"))
        {
            GameObject gun = collision.gameObject;
            DisplayPrompt();
            _inTrigger = true;
            interactObject = gun;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Structure"))
        {
            HidePrompt();
            _inTrigger = false;
            interactObject = null;
        }
    }
    public void PurchaseGun(GameObject gunObject)
    {
        int price = gunObject.GetComponent<GunPurchase>().price;
        GunType newGun = gunObject.GetComponent<GunPurchase>().gunType;
        if (price <= points)
        {
            if (newGun == currentGun)
            {
                statBlocks[(int)currentGun].z = 10;
            }
            else
            {
                currentGun = newGun;
            }
            points -= price;
        }
    }
}