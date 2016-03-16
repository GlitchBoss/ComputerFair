//© Joshua Hendershot 2016
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUtil : MonoBehaviour {

    public GameObject correct;
    public GameObject incorrect;

    public GameObject[] selectCircles;

	//All functions here are called by buttons

	public void StartGame()
	{
		//Start the game
		GameManager.instance.StartGame ();
	}

    public void PauseGame()
    {
		//Stop time (has the effect of a 
		//frame rate of zero)
        Time.timeScale = 0.0f;
        Time.fixedDeltaTime = 0.0f;
    }

    public void ResumeGame()
    {
		//Resume time (restores regular frames per second)
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
    }

    public void RestartGame()
    {
		//Restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(string level)
    {
		//Load the specified level
        SceneManager.LoadScene(level);
    }

    public void CheckAnswer(int noteNum)
    {
        bool isCorrect = GameManager.instance.CheckAnswer(noteNum);
		
		//Enable a sign depending on whether
		//the answer was correct or not
        if(isCorrect)
        {
            correct.SetActive(true);
            GameManager.instance.correctSFX.Play();
        }
        else
        {
            incorrect.SetActive(true);
        }
    }
    
    public void ClearSigns()
    {
		//Clear any signs that were enabled
        correct.SetActive(false);
        incorrect.SetActive(false);
    }

    public void SetCar(int car)
    {
		//Clear any signs that were enabled
        foreach(GameObject select in selectCircles)
        {
            select.SetActive(false);
        }

		//Set the selected car
        selectCircles[car].SetActive(true);
        GameManager.instance.SetCar(car);
    }

    public void LoadRace()
    {
		//Load the race if the user has selected a car
        if(GameManager.instance.currentCar != -1)
        {
            LoadLevel("Race");
        }
    }
}
