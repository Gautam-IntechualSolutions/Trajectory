using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "uperCollider")
        {
            collision.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
            GameManager.Instance.isCollide = true;
        }

        if (collision.collider.name == "obj" && GameManager.Instance.isCollide && collision.collider.name == "backstage")
        {
            collision.transform.Find("uperCollider").GetComponent<BoxCollider>().enabled = true;
            GameManager.Instance.isCollide = false;
        }

        if (collision.collider.name == "downCollider" && GameManager.Instance.isCollide)
        {
            collision.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
            Destroy(collision.transform.parent.gameObject, 0.75f);
            GameManager.Instance.score++;
            GameManager.Instance.activeObjCount--;
            GameManager.Instance.isCollide = false;
        }
    }
}