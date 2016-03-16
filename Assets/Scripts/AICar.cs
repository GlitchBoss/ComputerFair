//© Joshua Hendershot 2016
using UnityEngine;
using UnityStandardAssets.Utility;

public class AICar : MonoBehaviour {

    public float minSpeed;
    public float maxSpeed;
    public float stoppingDistance;
    public int currentWaypoint;
    public int place;
    public int currentLap;

    [HideInInspector]
    public Vector2 distanceToWaypoint;

    Transform target;
    WaypointCircuit waypointCircuit;
    Transform[] waypoints;
    Rigidbody2D rb;
    Vector2 targetPos;
    int laps;
    bool finished;
    bool hasStarted = false;
    float moveSpeed;

	//Start is called when the script is enabled
	void Start()
    {
		//Find references
        waypointCircuit = GameObject.FindGameObjectWithTag("WaypointCircuit").GetComponent<WaypointCircuit>();
        rb = GetComponent<Rigidbody2D>();
        laps = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().laps;

		//Set variables
        waypoints = waypointCircuit.Waypoints;
        currentWaypoint = 0;
        target = waypoints[currentWaypoint];
        finished = false;
        hasStarted = false;
        moveSpeed = Random.Range(minSpeed, maxSpeed);
    }

    public void Finish()
    {
        if (finished)
            return;

		//Tell the GameManager you finished the race if
		//you completed all laps
        if (currentLap >= laps)
        {
            finished = true;
            GameManager.instance.UIM.finishedCars.Add(gameObject);
        }
        else
        {
            currentLap++;
        }
    }

    public void StartGame()
    {
        hasStarted = true;
    }

	//Update is called every frame
	void Update()
    {
        if (!hasStarted)
            return;
        if (finished)
        {
            transform.rotation = Quaternion.identity;
            return;
        }

		//Get distance to the current waypoint
        targetPos = (Vector2) target.position;
        distanceToWaypoint = targetPos - (Vector2)transform.position;

        if(distanceToWaypoint.magnitude <= stoppingDistance)
        {
            NextWaypoint();
            return;
        }

		//Look towards the current waypoint
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		//Move the way your pointing
        rb.AddForce(gameObject.transform.right * moveSpeed);
    }

    void NextWaypoint()
    {
		//Set currentWaypoint to the next waypoint in line
        try
        {
            if (currentWaypoint < waypoints.Length)
                currentWaypoint++;
            else if (currentWaypoint >= waypoints.Length - 1)
                currentWaypoint =0;
            target = waypoints[currentWaypoint];
        }
        catch (System.IndexOutOfRangeException)
        {
        }

    }
}
