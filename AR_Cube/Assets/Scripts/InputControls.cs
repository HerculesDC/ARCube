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
    private ARInputManager m_ARInputManager;
    private ARCameraManager m_ARCameraManager;

    private Transform m_CameraTransform;

    private ARPlane m_PlaneObject;
    private ARReferencePoint m_DiamondPoint;
    //private ARReferencePoint m_PedestalPoint

    [SerializeField] private GameObject m_DiamondModel;
    [SerializeField] private GameObject m_PedestalModel;
    private GameObject VisibleDiamond;

    private static List<ARRaycastHit> m_HitList = new List<ARRaycastHit>();

    void Awake() {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        m_ARRaycastManager = GetComponent<ARRaycastManager>();
        m_ARReferencePointManager = GetComponent<ARReferencePointManager>();
        m_ARInputManager = GetComponent<ARInputManager>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        m_ARCameraManager = GetComponentInChildren<ARCameraManager>();
        m_CameraTransform = m_ARCameraManager.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (m_ARPlaneManager.trackables.count > 0 && !m_PlaneObject) {

            Ray ray = new Ray();
            ray.origin = m_CameraTransform.position;
            ray.direction = m_CameraTransform.position + m_CameraTransform.forward;

            if (m_ARRaycastManager.Raycast(ray, m_HitList, TrackableType.Planes)) {

                TrackableId planeId = m_HitList[0].trackableId;
                m_PlaneObject = m_ARPlaneManager.GetPlane(planeId);

                if (m_PlaneObject && !m_DiamondPoint) {

                    Pose pedestalPose = new Pose(m_PlaneObject.transform.position + (Vector3.up * 0.2f), Quaternion.identity);
                    ARReferencePoint pedestalPoint = m_ARReferencePointManager.AddReferencePoint(pedestalPose);
                    GameObject pedestal = Instantiate(m_PedestalModel, pedestalPoint.transform.position, pedestalPoint.transform.rotation, pedestalPoint.transform);
                    pedestal.transform.localScale = new Vector3(0.4f, 0.2f, 0.4f);

                    Pose pose = new Pose(m_PlaneObject.transform.position + Vector3.up, Quaternion.identity);
                    m_DiamondPoint = m_ARReferencePointManager.AddReferencePoint(pose);

                    if (m_DiamondPoint && !VisibleDiamond) {

                        VisibleDiamond = GameObject.Instantiate(m_DiamondModel, m_DiamondPoint.transform.position, Quaternion.Euler(-90, 0, 0), m_DiamondPoint.transform);
                        VisibleDiamond.transform.localScale = Vector3.one * 25f;
                    }
                }
            }
        }

        if (m_ARPlaneManager.trackables.count >= 1)
        {
            m_ARPlaneManager.enabled = false;
        }

        Touch touch;

        if (VisibleDiamond) {
            if(Vector3.Distance(m_DiamondPoint.transform.position, m_CameraTransform.position) < 1f){
                if (Input.touchCount > 0 && (touch = Input.GetTouch(0)).phase == TouchPhase.Moved) {
                        m_DiamondPoint.transform.Rotate(Vector3.up * -touch.deltaPosition.x);
                }
            }
        }
    }
}
