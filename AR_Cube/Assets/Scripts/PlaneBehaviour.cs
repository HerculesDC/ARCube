using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_PedestalModel;
    [SerializeField] private GameObject m_DiamondModel;
    
    //Rationale: other scripts will need to access it
    public GameObject Pedestal;
    public GameObject Diamond;

    // Start is called before the first frame update
    void Start()
    {
        //Diamond = Instantiate(m_DiamondModel, transform.position + Vector3.up * 0.55f, Quaternion.Euler(-90f, 0, 0), Pedestal.transform) as GameObject;
        //Diamond.transform.localScale = new Vector3(31.25f, 31.25f, 62.5f);
    }
}
