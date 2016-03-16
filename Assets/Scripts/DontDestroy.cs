//© Joshua Hendershot 2016
using UnityEngine;

public class DontDestroy : MonoBehaviour {

	//Awake is called when the script object is initialised
	void Awake()
    {
		//Don't destroy this game object
		//when a new scene is loaded
        DontDestroyOnLoad(this);
    }
}
