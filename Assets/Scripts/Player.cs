using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

public class Player : MonoBehaviour {

    public float moveSpeed;
    public float turnSpeed;
    public float accelerationSpeed;
    public int laps;
    public Text lapsText;
    public Text placeText;
    public bool finished;

    public int place;
    public int currentLap;
    public int currentWaypoint;
    [HideInInspector]
    public Vector2 distanceToWaypoint;

    Rigidbody2D rb;
    WaypointCircuit waypointCircuit;
    Transform[] waypoints;
    Vector2 targetPos;
    Transform target;
    bool hasStarted = false;
    static float stoppingDistance = 1.5f;

    public void Finish()
    {
        if (currentLap >= laps)
        {
            currentLap++;
            finished = true;
            GameManager.instance.UIM.finishedCars.Add(gameObject);
            GameManager.instance.EndGame(place == 1);
        }
        else
        {
            currentLap++;
            lapsText.text = string.Format("Lap {0}/{1}", currentLap.ToString(), laps.ToString());
        }
    }

    public void StartGame()
    {
        hasStarted = true;
    }

    void Start()
    {
		//Find references
		rb = GetComponent<Rigidbody2D>();
        waypointCircuit = GameObject.FindGameObjectWithTag("WaypointCircuit").GetComponent<WaypointCircuit>();
        lapsText = GameObject.Find("Laps").GetComponent<Text>();
        placeText = GameObject.Find("Place").GetComponent<Text>();

        lapsText.text = string.Format("Lap 1/{0}", laps.ToString());
        placeText.text = "Place 5/5";
        finished = false;
        waypoints = waypointCircuit.Waypoints;
        currentWaypoint = 0;
        target = waypoints[currentWaypoint];
        hasStarted = false;
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.hasStarted || !hasStarted)
            return;

		//Get the position of the player's finger
        var touchPosition = Vector3.zero;
        if (Input.touchCount > 0)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.GetTouch(0).position.x,
                Input.GetTouch(0).position.y, 0));

        }
        else
            return;

		//Look toward that finger
        Quaternion rot = Quaternion.LookRotation(transform.position - touchPosition, Vector3.forward);
        transform.rotation = rot;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        rb.angularVelocity = 0;

		//Move toward that finger
        rb.AddForce(gameObject.transform.up * moveSpeed);

		//Get/Set the current waypoint
        targetPos = (Vector2)target.position;
        distanceToWaypoint = targetPos - (Vector2)transform.position;
        if (distanceToWaypoint.magnitude <= stoppingDistance)
        {
            NextWaypoint();
        }
    }

    void NextWaypoint()
    {
        if (currentWaypoint < waypoints.Length - 1)
            currentWaypoint++;
        else if (currentWaypoint >= waypoints.Length - 1)
            currentWaypoint = 0;
        target = waypoints[currentWaypoint];

    }

    public void UpdateText()
    {
        placeText.text = string.Format("Place {0}/5", place.ToString());
    }
}
