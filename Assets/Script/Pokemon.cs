using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pokemon : MonoBehaviour
{
    private bool isHit = false;
    private float time = 0;
    private Collision collision;

    void Update() 
    {
        if (isHit) 
        {
            time += Time.deltaTime;
            if (time > 1.3f) 
            {
                Destroy(this.gameObject);
                Destroy(collision.gameObject);
                GameManager.instance.PoketballHit();
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "ball")
        {
            collision = other;
            GetComponent<Animator>().SetTrigger("Hit");
            isHit = true;
            GameManager.instance.isRemove = false;
        }
    }
}
