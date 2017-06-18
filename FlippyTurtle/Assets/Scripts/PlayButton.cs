using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour {

    public enum GameMode
    {
        PAUSE_START,
        PLAYING,
        GAME_OVER
    };

    public static GameMode GAME_MODE;
    public static float GAME_OVER_TIME;

	// Use this for initialization
	void Start () {
        this.restart();

    }

    void restart()
    {
        if(GAME_MODE == GameMode.GAME_OVER)
            this.transform.Translate(new Vector3(-100.0f, 0.0f, 0.0f));

        Time.timeScale = 0;
        GAME_MODE = GameMode.PAUSE_START;
    }
	
	// Update is called once per frame
	void Update () {
		
        //if GAME_OVER has been initiated, check for the end
        // state where everything is deleted and then pause
        if(GAME_MODE == GameMode.GAME_OVER && GAME_OVER_TIME < Time.time - 1)
        {
            this.restart();
        }
    

	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && GAME_MODE == GameMode.PAUSE_START)
        {
            Time.timeScale = 1;
            GAME_MODE = GameMode.PLAYING;
            //Destroy(this.gameObject);

            //shift the play button to the left off the screen.
            // we do not delete it because we use this to update the GAME_OVER state
            this.transform.Translate(new Vector3(100.0f, 0.0f, 0.0f));
        }
    }

    public static void GameOverCall()
    {
        if (GAME_MODE != GameMode.GAME_OVER)
        {
            Debug.Log("GAMAMAMMAMAMEEEEE OVVEEERRR!!!!");
            GAME_MODE = GameMode.GAME_OVER;
            GAME_OVER_TIME = Time.time;
        }
    }

}
