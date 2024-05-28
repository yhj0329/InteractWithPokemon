using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pokemon : MonoBehaviour
{
    private bool isHit = false;
    private float time = 0;
    private Collision collision;

    public GameObject[] effects;

    void Awake() 
    {
        GameObject obj = Instantiate(effects[0], transform.position, transform.rotation);
        Destroy(obj, 3f);
    }

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
                GameObject obj = Instantiate(effects[1], transform.position, transform.rotation);
                Destroy(obj, 2f);
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
