using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poketball : MonoBehaviour
{
    private float time;

    void Start ()
    {
        transform.position += new Vector3(0, -0.2f, 0);
        GetComponent<Rigidbody>().AddForce(transform.up * 125f);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > 5f) Destroy(this);
    }
}
