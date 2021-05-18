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
    bool HasPlayedP1=false;
    bool HasPlayedP2=false;
    bool HasPlayedP3=false;
    bool P1Start;
    bool P2Start;
    bool P3Start;
    bool once = false;
    bool ICanPlay;
    bool theme = false;
    // Start is called before the first frame update
    void Start()
    {
        p1 = p1Timer.GetComponent<PuzzleTimer>();
        p2 = p2Timer.GetComponent<PuzzleTimer>();
        p3 = p3Timer.GetComponent<PuzzleTimer>();

        mpc = MainPuzzleControllerObject.GetComponent<MainPuzzleController>();
        audio = GetComponent<AudioSource>();
        audio.volume = 0.15f;
        audio.loop = true;
        audio.PlayOneShot(MainTheme);
    }

    // Update is called once per frame
    void Update()
    {
        // these 2 booleans cover p1 p2 p3 !isPlaying and this class function IamPlayingTheThemeSong==true
        ICanPlay = p1.CanIPlayMusic && p2.CanIPlayMusic && IamPlayingTheThemeSong();

        
        if (mpc.Dcounter>4&& !P1Once ){

            audio.Stop();
            theme = false;
            audio.volume = 0.5f;
            audio.loop = false;
            audio.PlayOneShot(PuzzleCompleted);

            if (ICanPlay && mpc.isEmergentBool()) {
                Invoke("p1_finished", 3);
                HasPlayedP1 = true;
                theme = false;
            }
            P1Once = true;
        }else if(mpc.basketCollection>1 && !P2Once || mpc.WaterPumpIsPumping && !P2Once)
        {          
            audio.Stop();
            theme = false;
            audio.volume = 0.5f;
            audio.loop = false;
            audio.PlayOneShot(PuzzleCompleted);

            if (ICanPlay && mpc.isEmergentBool()) {
                Invoke("p2_finished", 3);
                HasPlayedP2 = true;
            }
            P2Once = true;
        }else if(mpc.p3com && !P3Once )
        {
            audio.Stop();
            theme = false;
            audio.volume = 0.5f;
            audio.loop = false;
            audio.PlayOneShot(PuzzleCompleted);

            if (ICanPlay && mpc.isEmergentBool()) {      
                Invoke("p3_finished", 3);
                HasPlayedP3 = true;
                theme = false;
            }
            P3Once = true;
        }else{

            if (!audio.isPlaying)
            {
                audio.volume = 0.15f;
                audio.loop = true;
                audio.PlayOneShot(MainTheme);
                theme = true;
            }
        }

        if (mpc.isEmergentBool()) {
            if (P1Once && !HasPlayedP1 && ICanPlay)
            {
                Invoke("p1_finished", 3);
                HasPlayedP1 = true;
            }
            if (P2Once && !HasPlayedP2 && ICanPlay)
            {
                Invoke("p2_finished", 3);
                HasPlayedP2 = true;
            }
            if (P3Once && !HasPlayedP3 && ICanPlay)
            {
                Invoke("p3_finished", 3);
                HasPlayedP3 = true;
            }
        }

      
    }
    
    public bool IamPlayingTheThemeSong(){
        return audio.loop;

    }
    // Donut
    void p1_finished()
    {
        audio.Stop();
        audio.loop = false;
        audio.volume = 0.5f;
        audio.PlayOneShot(SolutionDonut);
    }
    // Park
    void p2_finished()
    {
        audio.Stop();
        audio.loop = false;
        audio.volume = 0.5f;
        audio.PlayOneShot(SolutionPark);
    }
    // Factory
    void p3_finished()
    {
        audio.Stop();
        audio.loop = false;
        audio.volume = 0.5f;
        audio.PlayOneShot(SolutionFactory);
    }
    
}
