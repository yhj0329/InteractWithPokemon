using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectRemover : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;

    [SerializeField]
    private GameObject arObject;

    private GameObject obj;
    private float cooldown = 0;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isRemove) {
            cooldown += Time.deltaTime;
            if (cooldown < 2f) return;

            if (Input.touchCount > 0)
            {
                obj = Instantiate(arObject);
                obj.transform.position = arCamera.transform.position;
                obj.transform.rotation = Quaternion.Euler(arCamera.transform.rotation.eulerAngles - new Vector3(0, 180f, 0));
                obj.GetComponent<Rigidbody>().AddForce(arCamera.transform.forward * 175f);
                cooldown = 0;
            }
        }
    }
}
