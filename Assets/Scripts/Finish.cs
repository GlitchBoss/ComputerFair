using UnityEngine;

public class Finish : MonoBehaviour {

    public BoxCollider2D _collider;
    public bool playerCol;

    float width;
    Transform player;

	//Start is called when the script is enabled
	void Start()
    {
		//Find references
		player = GameObject.FindGameObjectWithTag("Player").transform;

		//Set variables
        width = _collider.size.x;
    }

	//Update is called every frame
	void Update()
    {
		//Ensures the player can't go backward through the finish line
        if (!playerCol)
            return;
        if (player.position.x < transform.position.x - width / 2)
            _collider.isTrigger = true;
        else if (player.position.x > transform.position.x + width / 2)
            _collider.isTrigger = false;
    }

	//OnTriggerEnter2D is called when the Collider2D col enters the trigger
	void OnTriggerEnter2D(Collider2D col)
    {
		//Tell the player or ai car
		//that it crossed the finish line
        if (col.tag == "Player")
        {
            col.GetComponent<Player>().Finish();
        }
        else if (col.tag == "Car")
        {
            col.GetComponent<AICar>().Finish();
        }
    }
}
