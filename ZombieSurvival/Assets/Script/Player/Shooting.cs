using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [Header("Input Alocation")]
    [SerializeField]
    private InputActionReference _input;

    private Vector3 mouse;

    //Gun Damage / Gun Range / Gun Ammo / Rate of Fire
    private float timeTilFire;
    [Header("Camera")]
    [SerializeField] Camera cam;

    [Header("Gun's Variables")]
    [SerializeField] public GunType currentGun = GunType.Pistol;
    [SerializeField] private TextMeshProUGUI ammoDisplay;
    [SerializeField] private TextMeshProUGUI pointsDisplay;
    [SerializeField] private Image ammoIcon;
    [SerializeField] private AudioClip empty;
    [SerializeField] private ParticleSystem gunShotLight;
    [SerializeField] private Sprite[] bulletIcons;
    [SerializeField] private AudioClip[] bulletNoise;

    public Vector4[] statBlocks = { new Vector4(5f, 8f, 10, 0.2f), new Vector4(10f, 14f, 10, 0.05f), new Vector4(20f, 5f, 10, 0.75f) };


    [Header("Character's Earnings")]
    public int points;

    private void OnEnable()
    {
        _input.action.Enable();
        _input.action.performed += PressedFire;
    }

    void Start()
    {
        timeTilFire = 0.0f;
        points = 0;
        ammoDisplay.text = statBlocks[(int)currentGun].z.ToString();
        ammoIcon.sprite = bulletIcons[(int)currentGun];
    }

    // Update is called once per frame
    void Update()
    {
        mouse = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (timeTilFire > -1)
        {
            timeTilFire -= Time.deltaTime;
        }

        ammoDisplay.text = statBlocks[(int)currentGun].z.ToString();
        ammoIcon.sprite = bulletIcons[(int)currentGun];
        pointsDisplay.text = "Points: " + points.ToString();
    }

    private void OnDisable()
    {
        _input.action.Disable();
        _input.action.performed -= PressedFire;
    }

    public void PressedFire(InputAction.CallbackContext value)
    {

        if (timeTilFire <= 0 && statBlocks[(int)currentGun].z > 0)
        {
            gunShotLight.Play();

            Vector3 mouseDir = (mouse - transform.position).normalized * statBlocks[(int)currentGun].y;
            mouseDir.z = 0;

            Debug.DrawRay(transform.position, mouseDir, Color.red, 0.3f);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, mouseDir, statBlocks[(int)currentGun].y * 2, LayerMask.GetMask("Map", "Zombies"));

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Zombie"))
                {
                    hit.collider.GetComponent<Health>().ReduceHealth(statBlocks[(int)currentGun].x);
                    hit.collider.GetComponent<ParticleSystem>().Play();
                    points += 2 * (int)statBlocks[(int)currentGun].x;
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


    }

}
