using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (transform.parent.GetComponent<BoxCollider2D>().enabled == false) {
            if (collision.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerMovement>().DisplayPrompt();
                if (Input.GetKey(KeyCode.F))
                {
                    transform.parent.GetComponent<BoxCollider2D>().enabled = true;
                    transform.parent.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().HidePrompt();
        }
    }
}
