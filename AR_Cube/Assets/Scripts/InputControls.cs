using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager)), RequireComponent(typeof(ARRaycastManager))]
public class InputControls : MonoBehaviour
{

    private ARPlaneManager m_ARPlaneManager;
    private ARRaycastManager m_ARRaycastManager;

    private Transform diamond;

    void Awake() {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        m_ARRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Not sure whether this if statement is working
        if (m_ARPlaneManager.trackables.count > 0 && !diamond) {
            diamond = GameObject.FindGameObjectWithTag("Target").transform;
        }
        //This successfully suppresses the creation of new planes
        if (m_ARPlaneManager.trackables.count >= 1) {
            m_ARPlaneManager.enabled = false;
        }
        //This will beget revisions
        //I was aware that the usual approach wouldn't work, but it was worth a try
        if (diamond) {
            if ((diamond.position - transform.position).sqrMagnitude < 1) {
                Debug.Log("D:" + diamond.position + ", C:" + transform.position);
                if (Input.touchCount > 0) {
                    Touch t = Input.GetTouch(0); 
                    diamond.transform.Rotate(Vector3.up * t.deltaPosition.x);
                }
            }
        }
    }
}
