using UnityEngine;
using UnityStandardAssets.Utility;

public class PlaceTracker : MonoBehaviour
{

    public AICar[] cars;
    public int playersPlace;
    Player player;

    int carsBehindOf = 0;

    void Start()
    {
		//Find references
		cars = FindObjectsOfType<AICar>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.hasStarted)
            return;

		//Get the player's place
        foreach(AICar car in cars)
        {
            carsBehindOf += CheckPosition(car);
        }
        player.place = carsBehindOf + 1;
        playersPlace = carsBehindOf + 1;
        player.UpdateText();
        carsBehindOf = 0;
    }

    int CheckPosition(AICar car)
    {
		//Check the position of the car compared to the player
        if(player.currentLap >= car.currentLap)
        {
            if (player.currentLap > car.currentLap)
                return 0;
            if (player.currentLap == car.currentLap && player.currentWaypoint >= car.currentWaypoint)
            {
                if (player.currentWaypoint > car.currentWaypoint)
                    return 0;
                if (player.currentLap == car.currentLap &&
            player.currentWaypoint == car.currentWaypoint &&
            player.distanceToWaypoint.magnitude <= car.distanceToWaypoint.magnitude)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }

        }
        else
        {
            return 1;
        }
    }
}
