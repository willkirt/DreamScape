using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementScript
{
    public static void move(GameObject obj, float speed)
    {
        Rigidbody rb;

        if (obj.GetComponent<Rigidbody>() == null)
        {
            Debug.Log(obj.tag + ", rigid body not found.");
            return;
        }
        else
        {
            rb = obj.GetComponent<Rigidbody>();
            rb.MovePosition(rb.position + obj.transform.forward * speed * Time.deltaTime);
        }
    }

    public static void jump(GameObject obj, float jumpHeight)
    {
        Rigidbody rb;

        if (obj.GetComponent<Rigidbody>() == null)
        {
            Debug.Log(obj.tag + ", rigid body not found.");
            return;
        }
        else
        {
            rb = obj.GetComponent<Rigidbody>();
            rb.AddForce(obj.transform.up * jumpHeight, ForceMode.Impulse);
        }
    }
}
