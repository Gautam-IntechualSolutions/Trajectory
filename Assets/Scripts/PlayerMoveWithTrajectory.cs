using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveWithTrajectory : MonoBehaviour
{
    Vector3 startPos, endPos;
    Rigidbody playerRigidbody;
    public GameObject trajectoryDot;
    GameObject[] trajectoryDots;
    Vector3 forceAtPlayer;
    float forceFactor = 2f;
    int number = 10;
    bool isPress = false;

    // Start is called before the first frame update
    void Start()
    {
        trajectoryDots = new GameObject[number];
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.isKinematic = true;
        playerRigidbody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //start point
            startPos = gameObject.transform.position;
            Debug.Log(" ==== start pos ==== " + startPos);
            for (int i = 0; i < number; i++)
            {
                trajectoryDots[i] = Instantiate(trajectoryDot, gameObject.transform);
            }
        }

        if (Input.GetMouseButton(0))
        {
            //drag point
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(" ==== end Pos ==== " + endPos);
            forceAtPlayer = endPos - startPos;
            for (int i = 0; i < number; i++)
            {
                trajectoryDots[i].transform.position = calculatePosition(i * 0.1f);
            }
            isPress = true;
        }


        if (Input.GetMouseButtonUp(0) && isPress)
        {
            //onleave mouse button
            Debug.Log(" ==== distance between start & end pos ==== " + (startPos - endPos));
            gameObject.transform.position = new Vector3(endPos.x, startPos.y, startPos.z);
            playerRigidbody.isKinematic = false;
            playerRigidbody.useGravity = true;
            playerRigidbody.velocity = new Vector2(-forceAtPlayer.x * forceFactor, -forceAtPlayer.y * forceFactor);
            for (int i = 0; i < number; i++)
            {
                Destroy(trajectoryDots[i]);
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            playerRigidbody.useGravity = false;
            playerRigidbody.velocity = Vector2.zero;
            gameObject.transform.position = new Vector3(0, 2, -4);
        }
    }

    private Vector3 calculatePosition(float elapsedTime)
    {
        return new Vector2(endPos.x, endPos.y) + //X0
                new Vector2(-forceAtPlayer.x * forceFactor, -forceAtPlayer.y * forceFactor) * elapsedTime + //ut
                0.5f * Physics2D.gravity * elapsedTime * elapsedTime;
    }
}