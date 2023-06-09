using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidBody;
    [SerializeField] float speed;

    [SerializeField] Camera cam;
    [SerializeField] private Transform prompt;

    Animator anime;
    Vector3 mouse;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
    }

    void Update()
    {
        mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);

        if (Input.GetAxis("Vertical") != 0)
        {
            rigidBody.velocity = new Vector2(0f, speed * Mathf.Sign(Input.GetAxis("Vertical")));
            anime.SetBool("idle", false);
            anime.SetBool("holdingGun", true);
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            rigidBody.velocity = new Vector2(speed * Mathf.Sign(Input.GetAxis("Horizontal")), 0f);
        }

        if (Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") == 0 && !anime.GetBool("idle"))
        {
            anime.SetBool("idle", true);
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
}
