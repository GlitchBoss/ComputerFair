using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

	void Awake()
    {
		//Don't destroy this game object when a new scene is loaded
        DontDestroyOnLoad(this);
    }
}
