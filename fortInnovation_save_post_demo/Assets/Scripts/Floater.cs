using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public float startingPositionY = 1.01f;
    private bool test = false;

    private void FixedUpdate()
    {
        if (transform.position.y < startingPositionY)
        {
            float displacementMultiplier = Mathf.Clamp01((startingPositionY - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
        }
    }
}
