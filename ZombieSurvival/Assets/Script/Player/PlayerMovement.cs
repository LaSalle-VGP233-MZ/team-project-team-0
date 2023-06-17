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
    Shotgun,
}
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 _movement;
    [SerializeField] float speed;

    [SerializeField] Camera cam;
    [SerializeField] private Transform prompt;

    private Rigidbody2D rigidBody;

    Animator anime;
    Vector3 mouse;

    private Inputs _input;
    private bool _inTrigger;
    public GameObject interactObject;
    private float barricadeHoldTimer = 2f;
    private float barricadeHoldCount = 0;
    private bool isBarricadeHolding = false;


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
        _input.Actions.InputPressedBar.canceled += InputLeaveF;
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


        SetAnimStates();

        TimerBarricade();

        prompt.transform.rotation = Quaternion.identity;
        prompt.transform.position = new Vector3(transform.position.x,transform.position.y + 1,0);
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
        _input.Actions.InputPressedBar.canceled -= InputLeaveF;
    }

    public void InputPressedF(InputAction.CallbackContext value)
    {
        //if (_inTrigger && interactObject.CompareTag("Breakable"))
        //{
        //    isBarricadeHolding = true;
        //}
        //else if (_inTrigger && interactObject.CompareTag("Gun"))
        //{
        //    PurchaseGun(interactObject);
        //    _inTrigger = false;
        //}
    }

    private void InputLeaveF(InputAction.CallbackContext value)
    {
        barricadeHoldCount = 0;
        isBarricadeHolding = false;
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
        if (collision.CompareTag("Structure") || collision.CompareTag("Gun"))
        {
            HidePrompt();
            _inTrigger = false;
            interactObject = null;
        }
    }

    private void TimerBarricade()
    {
        if (isBarricadeHolding)
        {
            barricadeHoldCount += Time.deltaTime;
        }

        if (barricadeHoldCount >= barricadeHoldTimer && interactObject != null)
        {
            interactObject.GetComponent<Health>().ResetHealth();
            interactObject.GetComponent<SpriteRenderer>().enabled = true;
            interactObject.GetComponent<BoxCollider2D>().enabled = true;
            _inTrigger = false;
            barricadeHoldCount = 0;
            isBarricadeHolding = false;
        }
    }
    //public void PurchaseGun(GameObject gunObject)
    //{
    //    int price = gunObject.GetComponent<GunPurchase>().price;
    //    GunType newGun = gunObject.GetComponent<GunPurchase>().gunType;
    //    if (price <= points)
    //    {
    //        if (newGun == currentGun)
    //        {
    //            statBlocks[(int)currentGun].z = 10;
    //        }
    //        else
    //        {
    //            currentGun = newGun;
    //        }
    //        points -= price;
    //    }
    //}
}