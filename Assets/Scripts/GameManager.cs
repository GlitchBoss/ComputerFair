//© Joshua Hendershot 2016
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	
    public GameObject[] noteBtns;
	public AudioSource[] notes;
    public GameObject[] playerCars;
	public AudioSource correctSFX;
	public int noteToGuess, notesLeft, currentCar;
	public float timer;
    [Range(0, 100)]
    public int gas;
    public int gasToRace = 20;

    [HideInInspector()]
    public NoteManager NM;
    [HideInInspector()]
    public UIManager UIM;
    [HideInInspector()]
	public float timerResetNum;
    [HideInInspector()]
	public bool hasStarted;
	
    Transform spawnPoint;
    int notesRestartNum = 5;
	int numCorrect = 0;

	public static GameManager instance;

	//Awake is called when the script object is initialised
	void Awake()
	{
		//Ensure that there 
		//is only one GameManager instance in the scene
		if (instance == null)
			instance = this;
		if (instance != this)
			Destroy (gameObject);

		StartUp();
	}

	//OnLevelWasLoaded is called after a new scene was loaded
	void OnLevelWasLoaded()
    {
        StartUp();
    }

	void StartUp()
	{
		hasStarted = false;

		//Find references
		gas = PlayerPrefs.GetInt("Gas", 0);
        UIM = GameObject.Find("UIManager").GetComponent<UIManager>();

		//Take different actions depending on the current scene
        switch (SceneManager.GetActiveScene().name)
        {
			case "BothClefs":
			case "TrebleClef":
			case "BassClef":
				//Find references
				NM = GameObject.Find ("NoteManager").GetComponent<NoteManager> ();
				
				//Set variables
				notesRestartNum = notesLeft;
				noteBtns = UIM.noteBtns;
				numCorrect = 0;
                break;
            case "MainMenu":
				//Set variables
				UIM.SetUpGas(gas);
                currentCar = -1;
                notesLeft = notesRestartNum;
                numCorrect = 0;
                break;
            case "Race":
				//Find references
				spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;

				//Spawn whichever car the player chose
                try
                {
                    Instantiate(playerCars[currentCar], 
                        spawnPoint.position, playerCars[currentCar].transform.rotation);
                }
                catch(System.IndexOutOfRangeException)
                {
                }
                break;
        }
    }

	public void StartGame()
	{
        hasStarted = true;

		//Spawn a new note if your in the
		//right scene
		string scene = SceneManager.GetActiveScene().name;
        if(scene == "BothClefs" || scene == "TrebleClef" || scene == "BassClef")
        {
            NM.NewNote();
        }
	}

	public void StartTimer(float time)
	{
		//Set reset variables and start
		//decreasing the time
		timer = time;
		timerResetNum = timer;
        UpdateText();
		InvokeRepeating ("DecreaseTimeRemaining", 1.0f, 1.0f);
		hasStarted = true;
	}

	void DecreaseTimeRemaining()
	{
		//Stop the timer if the time
		//has run out
        if(timer <= 0)
        {
            StopTimer();
            CheckAnswer(-1);
            return;
        }

		//Otherwise decrease the time
		timer--;
		UpdateText ();
	}

	void UpdateText()
	{
		//Show the current time
        string seconds = timer.ToString("00");
		if(UIM.timerText)
			UIM.timerText.text = seconds;
	}

	public void StopTimer()
	{
		if (!hasStarted)
			return;

		//Stop the timer and reset it
		CancelInvoke ("DecreaseTimeRemaining");
		CancelInvoke ("Flash");
		hasStarted = false;
		timer = timerResetNum;
	}

    public void SetCar(int car)
    {
		//Set the current car to
		//the car selected
        currentCar = car;
    }

    public bool CheckAnswer(int noteNum)
    {

        if (noteNum == NM.letterToGuess)
        {
			//Tell the user that they
			//answered correctly, then
			//choose a new note
            correctSFX.Play();
			LoseGas(-5);
            notesLeft--;
            numCorrect++;
			UIM.numCorrectText.text = "Number Correct: " + numCorrect.ToString();
            StopTimer();
            StartCoroutine(NewNote(notesLeft <= 0));
            return true;
        }

		//Stop the timer
        if(noteNum != -1)
            StopTimer();

		//Tell the user they answered wrong
        UIM.IncorrectAnswer();
        return false;
    }
    
    public IEnumerator NewNote(bool end)
    {
        yield return new WaitForSeconds(1.5f);
        if (end)
        {
			//End the game
            EndGame(true);
            yield break;
        }

		//Clear any signs that were enabled
        foreach(GameObject go in noteBtns)
        {
            go.GetComponent<ButtonUtil>().ClearSigns();
        }

		//Choose a new note
        NM.NewNote();
    }

	IEnumerator RestartGame ()
	{
		//Wait for three seconds then restart the game
		yield return new WaitForSeconds (3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    public void LoseGas(int amount)
    {
        gas -= amount;

		//Make sure that gas doesn't go above 100
		//or below 0
        if (gas > 100)
            gas = 100;
        else if (gas < 0)
            gas = 0;

		//Save the gas integer
        PlayerPrefs.SetInt("Gas", gas);
    }

    public void EndGame(bool hasWon)
    {
        hasStarted = false;

		//Take different actions depending on the current scene
		switch (SceneManager.GetActiveScene().name)
        {
            case "BothClefs":
            case "TrebleClef":
            case "BassClef":
				//Finish the game and reset variables
                UIM.Finish(hasWon);
                notesLeft = notesRestartNum;
                break;
            case "Race":
				//Find the player's place
                Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                player.place = UIM.finishedCars.IndexOf(player.gameObject);

				//Finish the game
                UIM.Finish(player.place + 1 == 1);
                hasStarted = false;

				//Loose some gas
                LoseGas(20);
                break;
        }
    }
}
