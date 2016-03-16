//© Joshua Hendershot 2016
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CountDown : MonoBehaviour
{
    public GameObject howTo;

    Text text;

	////Start is called when the script is enabled
	void Start()
    {
		//Find references
		text = GetComponent<Text>();

        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
		//Count down from three, then
		//tell everyone to start the game
        text.text = "3";
        yield return new WaitForSeconds(1);
        text.text = "2";
        yield return new WaitForSeconds(1);
        text.text = "1";
        yield return new WaitForSeconds(1);
        text.text = "Go!";
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
        {
            go.SendMessage("StartGame", SendMessageOptions.DontRequireReceiver);
        }
        howTo.SetActive(false);
        yield return new WaitForSeconds(1);
        text.text = "";
    }
}
