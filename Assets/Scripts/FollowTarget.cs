//© Joshua Hendershot 2016
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public float dampTime = 0.15f;
    public bool smooth;

    Vector3 velocity = Vector3.zero;

    float normSpeed;

	//Start is called when the script is enabled
	void Start()
    {
		//Find references
		target = GameObject.FindGameObjectWithTag("Player").transform;

        if (target)
        {
            transform.position = target.position + offset;
        }
    }

	//FixedUpdate is called every fixed framerate frame
	void FixedUpdate()
    {
        if (smooth)
        {
            if (transform.position != target.position + offset)
            {
				//Smooth follow the target
                Vector3 destination = Vector3.SmoothDamp(transform.position,
                                                          target.position + offset,
                                                          ref velocity,
                                                          dampTime);
                transform.position = new Vector3(destination.x,
                                                 destination.y,
                                                 transform.position.z);
            }
        }
    }
}
