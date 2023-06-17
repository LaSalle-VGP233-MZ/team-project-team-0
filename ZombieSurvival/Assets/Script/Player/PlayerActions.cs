using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [Header("Input Alocation")]
    [SerializeField]
    private InputActionReference _input;

    [Header("Player Shooting Script")]
    [SerializeField]
    private Shooting _playerS;

    [Header("For Barricade Actions")]
    [SerializeField] private Transform prompt;
    [SerializeField] private AudioClip barricadeNoise;

    public GameObject interactObject;

    private bool _inTrigger;

    private float barricadeHoldTimer;
    private float barricadeHoldCount;
    private bool isBarricadeHolding;
    private float barricadeSoundDelay = 0;
    private Animator animator;


    //----------------------------------Start of Script--------------------------------------------
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        _input.action.Enable();
        _input.action.performed += InputPressedF;
        _input.action.canceled += InputLeaveF;
    }

    void Start()
    {
        barricadeHoldTimer = 2.0f;
        barricadeHoldCount = 0f;
        isBarricadeHolding = false;
    }

    //----------------------------------Update & Triggers-----------------------------------------
    void Update()
    {
        prompt.transform.rotation = Quaternion.identity;
        prompt.transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);

        TimerBarricade();
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

    private void OnDisable()
    {
        _input.action.Disable();
        _input.action.performed -= InputPressedF;
        _input.action.canceled -= InputLeaveF;
    }

    //---------------------------------Input Method----------------------------------
    public void InputPressedF(InputAction.CallbackContext value)
    {
        if (_inTrigger && interactObject.CompareTag("Breakable"))
        {
            isBarricadeHolding = true;
        }
        else if (_inTrigger && interactObject.CompareTag("Gun"))
        {
            PurchaseGun(interactObject);
            _inTrigger = false;
        }
    }

    //------------------------------------------Methods----------------------------------
    private void InputLeaveF(InputAction.CallbackContext value)
    {
        barricadeHoldCount = 0;
        isBarricadeHolding = false;
        animator.SetBool("barricade", false);
    }

    private void TimerBarricade()
    {
        if (isBarricadeHolding)
        {
            barricadeHoldCount += Time.deltaTime;
            animator.SetBool("barricade", true);
            PlayBarricadeSound();
        }

        if (barricadeHoldCount >= barricadeHoldTimer && interactObject != null)
        {
            interactObject.GetComponent<Health>().ResetHealth();
            interactObject.GetComponent<SpriteRenderer>().enabled = true;
            interactObject.GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<Shooting>().points += 10;
            _inTrigger = false;
            barricadeHoldCount = 0;
            isBarricadeHolding = false;
        }
    }

    public void PurchaseGun(GameObject gunObject)
    {
        int price = gunObject.GetComponent<GunPurchase>().price;
        GunType newGun = gunObject.GetComponent<GunPurchase>().gunType;
        if (price <= _playerS.points)
        {
            if (newGun == _playerS.currentGun)
            {
                _playerS.statBlocks[(int)_playerS.currentGun].z = _playerS.statBlocksDefault[(int)_playerS.currentGun].z;
            }
            else
            {
                _playerS.currentGun = newGun;
            }
            _playerS.points -= price;
        }
    }

    public void DisplayPrompt()
    {
        prompt.gameObject.SetActive(true);
    }
    public void HidePrompt()
    {
        prompt.gameObject.SetActive(false);
    }
    private void PlayBarricadeSound()
    {
        if (barricadeSoundDelay <= 0)
        {
            AudioSource.PlayClipAtPoint(barricadeNoise, new Vector3(0, 0, 0));
            barricadeSoundDelay = barricadeNoise.length;
        }
        else
        {
            barricadeSoundDelay -= Time.deltaTime;
        }
    }
}
