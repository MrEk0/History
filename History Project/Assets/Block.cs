using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] float forceMultiplier;
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask boxMask;
}
