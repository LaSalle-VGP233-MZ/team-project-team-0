using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input Alocation")]
    [SerializeField]
    private InputActionReference _input;

    [Header("Movement")]
    [SerializeField] float speed;

    [Header("Camera")]
    [SerializeField] Camera cam;

    private Rigidbody2D rigidBody;
    private Vector2 _movement;

    Animator anime;
    Vector3 mouse;

    private void OnEnable()
    {
        _input.action.Enable();
        _input.action.performed += Movement;
        _input.action.canceled += StopMovement;
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

    private void OnDisable()
    {
        _input.action.Disable();
        _input.action.performed -= Movement;
        _input.action.canceled -= StopMovement;
    }
}