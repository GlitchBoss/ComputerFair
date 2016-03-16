//© Joshua Hendershot 2016
using UnityEngine;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour {

    public List<List<GameObject>> notesList;
    public List<GameObject> As;
    public List<GameObject> Bs;
    public List<GameObject> Cs;
    public List<GameObject> Ds;
    public List<GameObject> Es;
    public List<GameObject> Fs;
    public List<GameObject> Gs;
    public int letterToGuess;
    public int timeToGuess;

    int lastNote;
    int note;

	//Start is called when the script is enabled
	void Start()
    {
		//Set variables
		notesList = new List<List<GameObject>>();
        notesList.Add(As);
        notesList.Add(Bs);
        notesList.Add(Cs);
        notesList.Add(Ds);
        notesList.Add(Es);
        notesList.Add(Fs);
        notesList.Add(Gs);
    }

    public void NewNote()
    {
        GameManager.instance.StopTimer();

		//Clear any notes that were enabled
        foreach(List<GameObject> list in notesList)
        {
            foreach(GameObject go in notesList[notesList.IndexOf(list)])
            {
                go.SetActive(false);
            }
        }

		//Choose and show a random note
        ChooseRandomNote();
        ShowNote();
        GameManager.instance.StartTimer(timeToGuess);
    }

    void ShowNote()
    {
		//Show the chosen note
        notesList[letterToGuess][note].SetActive(true);
    }

    void ChooseRandomNote()
    {
		//Choose a random note
        letterToGuess = Random.Range(0, notesList.Count);
        note = Random.Range(0, notesList[letterToGuess].Count);

		//Ensure the same note isn't showed twice in a row
        while(letterToGuess == lastNote)
            letterToGuess = Random.Range(0, 7);
        lastNote = letterToGuess;
    }
}
