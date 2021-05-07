using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    public AudioClip MainTheme;
    public AudioClip PuzzleCompleted;
    public AudioClip GameFinnish;
    private AudioSource audio;

    public GameObject MainPuzzleControllerObject;
    private MainPuzzleController mpc;

    private bool P1Once =false;
    private bool P2Once =false;
    private bool P3Once =false;
    
    // Start is called before the first frame update
    void Start()
    {
        mpc = MainPuzzleControllerObject.GetComponent<MainPuzzleController>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mpc.Dcounter>4 && !P1Once){
            audio.Stop();
            audio.volume = 0.5f;
            audio.PlayOneShot(PuzzleCompleted);
            P1Once = true;
        }else if(mpc.startP1 && mpc.startP2 && mpc.startP3 && !P2Once)
        {
           audio.Stop();
            audio.volume = 0.5f;
            audio.PlayOneShot(PuzzleCompleted);
            P2Once = true;
        }else if(mpc.gameFinish && !P3Once){
            
            audio.Stop();
            audio.volume = 0.5f;
            audio.PlayOneShot(PuzzleCompleted);
            P3Once = true;
        }else{

            if (!audio.isPlaying) {
                audio.volume = 0.15f;
                audio.PlayOneShot(MainTheme);
            }
            //audio.Play();
        }
    }
}
