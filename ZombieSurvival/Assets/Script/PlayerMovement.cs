using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidBody;
    [SerializeField] private Vector2 _movement;
    [SerializeField] float speed;

    [SerializeField] Camera cam;
    [SerializeField] private Transform prompt;

    Animator anime;
    Vector3 mouse;

    [SerializeField] private float gunDamage = 1;
    [SerializeField] private float gunRange = 4.5f;
    [SerializeField] private float ammo = 10;
    [SerializeField] private float rateOfFire = 0.2f;
    private float timeTilFire = 0f;
    [SerializeField] private AudioClip clip;

    private Inputs _input;
    private bool _inTrigger;
    public GameObject currentBar;
    private float barricadeHoldTimer = 2f;
    private float barricadeHoldCount = 0;


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

        if (timeTilFire > 0)
            timeTilFire -= Time.deltaTime;

        SetAnimStates();
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
        if (_inTrigger && currentBar != null)
        {
            currentBar.GetComponent<Health>().ResetHealth();
            currentBar.GetComponent<SpriteRenderer>().enabled = true;
            currentBar.GetComponent<BoxCollider2D>().enabled = true;
            _inTrigger = false;
        }
    }

    public void PressedFire(InputAction.CallbackContext value)
    {
        if (timeTilFire <= 0 && ammo > 0)
        {
            Vector3 mouseDir = new Vector3(mouse.x - transform.position.x, mouse.y - transform.position.y, 0f).normalized;
            mouseDir *= gunRange;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, mouseDir, gunRange * 2, LayerMask.GetMask("Map", "Zombies"));
            if (hit.collider.gameObject.CompareTag("Zombie"))
            {
                hit.collider.GetComponent<Health>().ReduceHealth(gunDamage);
                ammo++;
            }
            ammo--;
            AudioSource.PlayClipAtPoint(clip, new Vector3(0, 0, 0));
            timeTilFire = rateOfFire;
        }
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
                currentBar = barricade;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Structure"))
        {
            HidePrompt();
            _inTrigger = false;
            currentBar = null;
        }
    }
}