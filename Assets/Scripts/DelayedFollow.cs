using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedFollow : MonoBehaviour
{
    public GameObject hand;
    public Vector3 offset;
    public Vector3 desiredPos;

    private void Update()
    {
        desiredPos = new Vector3(hand.transform.position.x, 0, hand.transform.position.z);
        transform.position  = Vector3.Lerp(transform.position, desiredPos + offset, 10f * Time.deltaTime);
    }
}
