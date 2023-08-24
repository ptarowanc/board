using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInstantiateParent : MonoBehaviour {

    public static IInstantiateParent Instance;
    
    public GameObject m_objParent;
    public GameObject m_objWinnerChip;
    public GameObject[] m_objWinnerChipsPos;

    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
