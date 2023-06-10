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

    private Inputs _input;

    private GameObject currentBar;
    private void Awake()
    {
        _input = new Inputs();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Movement.performed += Movement;
        _input.Player.Movement.canceled += StopMovement;
    }

    void Start()
    {
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
    }




    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTrigger"))
        {
            DisplayPrompt();
            if (Input.GetKey(KeyCode.F))
            {
                transform.parent.GetComponent<BoxCollider2D>().enabled = true;
                transform.parent.GetComponent<SpriteRenderer>().enabled = true;
                currentBar = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerTrigger"))
        {
            HidePrompt();
        }
    }


}

//Tagged
//{
//    mouse = cam.ScreenToWorldPoint(Input.mousePosition);
//    float angle = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;
//    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);

//    if (Input.GetAxis("Vertical") != 0)
//    {
//        rigidBody.velocity = new Vector2(0f, speed * Mathf.Sign(Input.GetAxis("Vertical")));
//        anime.SetBool("idle", false);
//        anime.SetBool("holdingGun", true);
//    }

//    if (Input.GetAxis("Horizontal") != 0)
//    {
//        rigidBody.velocity = new Vector2(speed * Mathf.Sign(Input.GetAxis("Horizontal")), 0f);
//    }

//    if (Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") == 0 && !anime.GetBool("idle"))
//    {
//        anime.SetBool("idle", true);
//    }
//}