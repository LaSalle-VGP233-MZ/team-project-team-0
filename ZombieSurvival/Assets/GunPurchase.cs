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

    [HideInInspector]public int price;

    // Start is called before the first frame update
    void Start()
    {
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
}
