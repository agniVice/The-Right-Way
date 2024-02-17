using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementConnector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Element"))
        {
            GetComponentInParent<Element>().ConnectWith(collision.GetComponentInParent<Element>());
            collision.GetComponentInParent<Element>().ConnectWith(GetComponentInParent<Element>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Element"))
        {
            GetComponentInParent<Element>()?.DisconnectWith(collision.GetComponentInParent<Element>());
            collision.GetComponentInParent<Element>()?.DisconnectWith(GetComponentInParent<Element>());
        }
    }
}
