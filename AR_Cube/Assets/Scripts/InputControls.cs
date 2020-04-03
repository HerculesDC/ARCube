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
    private ARReferencePointManager m_ARReferencePointManager;

    private Transform diamond;
    TrackableId t;
    private Transform camTransform;
    private static List<ARRaycastHit> hitList = new List<ARRaycastHit>();

    GameObject g;

    void Awake() {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        m_ARRaycastManager = GetComponent<ARRaycastManager>();
        m_ARReferencePointManager = GetComponent<ARReferencePointManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        camTransform = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //ORIGINAL IDEA
        if (m_ARPlaneManager.trackables.count > 0 && !diamond)
        {
            diamond = GameObject.FindGameObjectWithTag("Target").transform;
            if (diamond)
            {
                Pose p = new Pose();
                p.position = diamond.position;
                p.rotation = diamond.rotation;
                t = m_ARReferencePointManager.AddReferencePoint(p).trackableId;
            }
        }
        //This successfully suppresses the creation of new planes
        if (m_ARPlaneManager.trackables.count >= 1)
        {
            m_ARPlaneManager.enabled = false;
        }
        //This will beget revisions
        if (diamond)
        {
            Ray ray = new Ray();
            ray.origin = camTransform.position;
            ray.direction = m_ARReferencePointManager.GetReferencePoint(t).transform.position - camTransform.position;
            if (m_ARRaycastManager.Raycast(ray, hitList))
            {
                if (hitList.Count > 0 && hitList[0].distance < 1.5f)
                {

                    diamond.Rotate(Vector3.up, 10f);
                }
            }
        }
    }
}
