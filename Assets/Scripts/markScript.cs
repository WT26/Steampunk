using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markScript : MonoBehaviour {

    private Quaternion rotation;
    void Start()
    {
        rotation = transform.rotation;

    }
    void LateUpdate()
    {
        transform.rotation = rotation;
    }

}
