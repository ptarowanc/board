using UnityEngine;
using System.Collections;

public class AnimationEnd : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void EndAnimation()
    {
        this.gameObject.SetActive(false);       
    }
}
