using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] int chainNumber;
    [SerializeField] GameObject chainPrefab;
    [SerializeField] Rigidbody2D hook;

    private HingeJoint2D hingeJoint;

    private void Start()
    {
        GenetateRope();
    }

    private void GenetateRope()
    {
        Rigidbody2D previousRB = hook;

        for (int i = 0; i < chainNumber; i++)
        {
            GameObject chain = Instantiate(chainPrefab, transform);
            hingeJoint = chain.GetComponent<HingeJoint2D>();
            hingeJoint.connectedBody = previousRB;

            previousRB = chain.GetComponent<Rigidbody2D>();
        }
    }
}
