using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class GunPurchase : MonoBehaviour
{
    [SerializeField] public GunType gunType;
    [SerializeField] private Sprite pistolSprite;
    [SerializeField] private Sprite rifleSprite;
    [SerializeField] private Sprite shotGunSprite;
    [SerializeField] private int pistolCost;
    [SerializeField] private int arCost;
    [SerializeField] private int shotGunCost;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Transform prompt;


    public int price;
    private bool _inTrigger;
    public GameObject currentGunObject;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        switch (gunType)
        {
            case GunType.Pistol:
                GetComponentInChildren<SpriteRenderer>().sprite = pistolSprite;
                price = pistolCost;
                break;
            case GunType.Rifle:
                GetComponentInChildren<SpriteRenderer>().sprite = rifleSprite;
                price = arCost;
                break;
            case GunType.Shotgun:
                GetComponentInChildren<SpriteRenderer>().sprite = shotGunSprite;
                price = shotGunCost;    
                break;
        }
        priceText.text = price.ToString();
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Structure"))
    //    {
    //        GameObject gun = collision.transform.parent.gameObject;
    //        if (!gun.GetComponent<BoxCollider2D>().enabled)
    //        {
    //            DisplayPrompt();
    //            _inTrigger = true;
    //            currentGunObject = gun;
    //        }
    //    }
    //}
    //public void InputPressedF(InputAction.CallbackContext value)
    //{
    //    if (_inTrigger && currentGunObject != null)
    //    {
    //        player.GetComponent<PlayerMovement>().PurchaseGun(gunType, price);
    //        _inTrigger = false;
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Gun"))
    //    {
    //        HidePrompt();
    //        _inTrigger = false;
    //        currentGunObject = null;
    //    }
    //}
    //public void DisplayPrompt()
    //{
    //    prompt.gameObject.SetActive(true);
    //}
    //public void HidePrompt()
    //{
    //    prompt.gameObject.SetActive(false);
    //}
}
