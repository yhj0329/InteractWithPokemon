using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARObjectAction : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject arObject;
    private Animator arAni;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public bool isMove = false;
    public float aniDuration = 0f;

    void Start()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        arObject = GetComponent<ARObjectController>().arObject;
        arAni = arObject.GetComponent<Animator>();
        aniDuration += Time.deltaTime;
        if(aniDuration >= 4f && !isMove && !GetComponent<ARObjectController>().isTouch) {
            switch(Random.Range(0, 3)) {
                case 0:
                    aniDuration = 0;
                    break;
                case 1:
                    arAni.SetTrigger("Hi");
                    aniDuration = 0;
                    break;
                case 2:
                    SetRandomPosition();
                    aniDuration = 0;
                    break;
            }
        }
        if (isMove) Move();
    }

    private void SetRandomPosition()
    {
        Vector2 randomScreenPoint = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));

        if (arRaycastManager.Raycast(randomScreenPoint, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            targetPosition = hitPose.position;
            targetRotation = hitPose.rotation;
            isMove = true;
            arAni.SetBool("Walk", true);
        }
    }

    private void Move()
    {
        
        float step = 0.3f * Time.deltaTime;
        Vector3 direction = targetPosition - arObject.transform.position;

        if (direction.magnitude > 0.1f)
        {
            arObject.transform.position = Vector3.MoveTowards(arObject.transform.position, targetPosition, step);
            arObject.transform.rotation = Quaternion.Slerp(arObject.transform.rotation, Quaternion.LookRotation(direction), step * 5f);
        }
        else
        {
            isMove = false;
            arAni.SetBool("Walk", false);
        }
    }
}
