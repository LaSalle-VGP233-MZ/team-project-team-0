using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidBody;
    [SerializeField] float speed;
    [SerializeField] float gunRange;

    [SerializeField] Camera cam;
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
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, speed * Mathf.Sign(Input.GetAxis("Vertical")));
            anime.SetBool("idle", false);
            anime.SetBool("holdingGun", true);
        }
        else
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            rigidBody.velocity = new Vector2(speed * Mathf.Sign(Input.GetAxis("Horizontal")), rigidBody.velocity.y);
            anime.SetBool("idle", false);
            anime.SetBool("holdingGun", true);
        }
        else
        {
            rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
        }

        if (Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") == 0 && !anime.GetBool("idle"))
        {
            anime.SetBool("idle", true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(mouse.x, mouse.y));
            Debug.DrawRay(transform.position, new Vector2(mouse.x, mouse.y), Color.red, 1);
        }
    }
}
