using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    public AudioClip MainTheme;
    public AudioClip PuzzleCompleted;
    public AudioClip GameFinnish;
    public AudioClip SolutionDonut, SolutionPark, SolutionFactory;
    private AudioSource audio;

    public GameObject MainPuzzleControllerObject;
    private MainPuzzleController mpc;

    private bool P1Once =false;
    private bool P2Once =false;
    private bool P3Once =false;

    public GameObject p1Timer;
    public GameObject p2Timer;
    public GameObject p3Timer;
    PuzzleTimer p1;
    PuzzleTimer p2;
    PuzzleTimer p3;

    bool P1Start;
    bool P2Start;
    bool P3Start;
    bool once = false;
    // Start is called before the first frame update
    void Start()
    {
        p1 = p1Timer.GetComponent<PuzzleTimer>();
        p2 = p2Timer.GetComponent<PuzzleTimer>();
        p3 = p3Timer.GetComponent<PuzzleTimer>();

        mpc = MainPuzzleControllerObject.GetComponent<MainPuzzleController>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mpc.Dcounter>4&& !P1Once ){
            audio.Stop();
            audio.volume = 0.5f;
            audio.PlayOneShot(PuzzleCompleted);
            Invoke ("p1_finished", 2);
            P1Once = true;
        }else if(mpc.basketCollection>1 && !P2Once || mpc.WaterPumpIsPumping && !P2Once)
        {
           
            audio.Stop();
            audio.volume = 0.5f;
            audio.PlayOneShot(PuzzleCompleted);
            Invoke ("p2_finished", 2);
            P2Once = true;
        }else if(mpc.p3com && !P3Once )
        {
            audio.Stop();
            audio.volume = 0.5f;
            audio.PlayOneShot(PuzzleCompleted);
            Invoke ("p3_finished", 2);
            P3Once = true;
        }else{

            if (!audio.isPlaying) {
                audio.volume = 0.15f;
                audio.PlayOneShot(MainTheme);
            }
            //audio.Play();
        }
       /* if (!once)
        {
            P1Start = p1.MyPuzzle;
            P2Start = p2.MyPuzzle;
            P3Start = p3.MyPuzzle;
            once = true;
        }*/
    }
    // Donut
    void p1_finished()
    {
        audio.volume = 1f;
        audio.PlayOneShot(SolutionDonut);
    }
    // Park
    void p2_finished()
    {
        audio.volume = 1f;
        audio.PlayOneShot(SolutionPark);
    }
    // Factory
    void p3_finished()
    {
        audio.volume = 1f;
        audio.PlayOneShot(SolutionFactory);
    }
}
