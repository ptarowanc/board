using UnityEngine;
using System.Collections;

public class CMovingObject : MonoBehaviour {

	public Vector3 begin;
	public Vector3 to;
	public float duration = 0.1f;
    public bool rotate = true;
	SpriteRenderer sprite_renderer;

	void Awake()
	{
		this.sprite_renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
	}


	public void run()
	{
		StopAllCoroutines();
		StartCoroutine(run_moving());
	}


	IEnumerator run_moving()
	{
		//this.sprite_renderer.sortingOrder = CSpriteLayerOrderManager.Instance.Order;

		float begin_time = Time.time;
		while (Time.time - begin_time <= duration)
		{
			float t = (Time.time - begin_time) / duration;

			float x = EasingUtil.easeInExpo(begin.x, to.x, t);
			float y = EasingUtil.easeInExpo(begin.y, to.y, t);
            float r = EasingUtil.easeInCirc(0, 4, t);
            transform.position = new Vector3(x, y, begin.z);
            if(rotate)
            {
                float rotAmount = 90f * r;
                float curRot = transform.localRotation.eulerAngles.z;
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));
            }
            
            yield return 0;
		}
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.position = to;
	}
}
