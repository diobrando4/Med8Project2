using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
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
    bool videoIsplaying;
    public GameObject videoObj;
    bool dougnatDone =false;
    bool ParkDone =false;
    //Raw Image to Show Video Images [Assign from the Editor]
    public RawImage image;
    //Video To Play [Assign from the Editor]
    public VideoClip IntroVideo;
    public VideoClip Donut_finnished;
    public VideoClip Park_finnish;
    public VideoClip videoToPlay_Factory;
    public VideoClip EmergentParkIntro;
    public VideoClip EmergentFactoryIntro;

    bool EmergentFactoryIntroBool = false;
    bool EmergentParkIntroBool = false;
    private VideoPlayer videoPlayer;
    private VideoSource videoSource;

    public VideoPlayer introplayer;
    bool introIsplaying = true;
    //Audio
    private AudioSource audioSource;
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        p1 = p1Timer.GetComponent<PuzzleTimer>();
        p2 = p2Timer.GetComponent<PuzzleTimer>();
        p3 = p3Timer.GetComponent<PuzzleTimer>();

        Application.runInBackground = true;
        //videoObj.SetActive(false);
        image.enabled = false;

        mpc = MainPuzzleControllerObject.GetComponent<MainPuzzleController>();
        audio = GetComponent<AudioSource>();
        audio.volume = 0.15f;
        audio.loop = true;
        audio.PlayOneShot(MainTheme);
        introplayer.clip = IntroVideo;
        introIsplaying = true;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Intro is playing " + introplayer.time);

        if(introplayer.clip == IntroVideo && !introplayer.isPlaying && introIsplaying && introplayer.time>2 || Input.GetKey("return"))
        {

           
                introIsplaying = false;
                //  introplayer.enabled = false;
                panel.SetActive(false);
            
        }
        Debug.Log("should factory be played ? " +( !introIsplaying && mpc.isEmergentBool() && !EmergentFactoryIntroBool && !introplayer.isPlaying));
        //factory start
        if ( !introIsplaying  && mpc.isEmergentBool() && !EmergentFactoryIntroBool && !introplayer.isPlaying)
        {

            panel.SetActive(true);
            EmergentFactoryIntroBool = true;
            //  introplayer.enabled = true;
            introplayer.clip = EmergentFactoryIntro;
            Debug.Log("playing park!!____ ");
            introplayer.time = 0;

        }
       
        //park start

        if (!introplayer.isPlaying && mpc.isEmergentBool() && EmergentParkIntroBool)
        {

            panel.SetActive(true);
            EmergentParkIntroBool = true;
            //  introplayer.enabled = true;
            introplayer.clip = EmergentParkIntro;
            Debug.Log("playing park!!____ ");

        }
        

        // these 2 booleans cover p1 p2 p3 !isPlaying and this class function IamPlayingTheThemeSong==true
        ICanPlay = p1.CanIPlayMusic && p2.CanIPlayMusic && IamPlayingTheThemeSong() && !introplayer.isPlaying;

      /*  if(!videoIsplaying){
            StartCoroutine(playVideo_Donut());
        }*/
        if (mpc.Dcounter>4&& !P1Once ){

            audio.Stop();
            theme = false;
            audio.volume = 0.5f;
            audio.loop = false;
            audio.PlayOneShot(PuzzleCompleted);

            // Dougnat cutscne
            image.enabled = true;
           // StartCoroutine(playVideo_Donut());

            if (ICanPlay) {
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

            // park cutscene
            image.enabled = true;
           // StartCoroutine(playVideo_Park());


            if (ICanPlay) {
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

            if (ICanPlay) {      
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
        // park off
        if (introplayer.clip == EmergentParkIntro && !introplayer.isPlaying && EmergentParkIntro && introplayer.time > 2 && mpc.isEmergentBool() || Input.GetKey("return") && mpc.isEmergentBool() && EmergentParkIntro)
        {
            EmergentParkIntroBool = false;
            //  introplayer.enabled = false;
            panel.SetActive(false);
        }
        // factory off
        if (introplayer.clip == EmergentFactoryIntro && !introplayer.isPlaying && EmergentFactoryIntroBool && introplayer.time > 2 && EmergentFactoryIntroBool && mpc.isEmergentBool() || Input.GetKey("return") && mpc.isEmergentBool() && EmergentFactoryIntroBool)
        {
            EmergentParkIntroBool = true;
            //  introplayer.enabled = false;
            panel.SetActive(false);
        }
        //  Debug.Log("P1Once " + P1Once + " dougnatDone " + dougnatDone);
        if (P1Once  && !dougnatDone && !mpc.isEmergentBool())
        {

            panel.SetActive(true);
          //  introplayer.enabled = true;
            introplayer.clip = Donut_finnished;
            Debug.Log("playing donut!!____ ");

        }

        if (P2Once && !ParkDone && !mpc.isEmergentBool())
        {

            panel.SetActive(true);
            //  introplayer.enabled = true;
            introplayer.clip = Park_finnish;
            Debug.Log("playing park!!____ ");

        }

        if (introplayer.clip == Donut_finnished && !introplayer.isPlaying && introplayer.time > 2 && !dougnatDone && P1Once && !mpc.isEmergentBool() || Input.GetKey("return") && P1Once && !mpc.isEmergentBool())
        {
            Debug.Log("stop playing donuts");
            dougnatDone = true;
          //  introplayer.enabled = false;
            panel.SetActive(false);
        }

        if (introplayer.clip == Park_finnish && !introplayer.isPlaying && introplayer.time > 2 && !ParkDone && P2Once && !mpc.isEmergentBool()  || Input.GetKey("return") && P2Once && !mpc.isEmergentBool())
        {
            Debug.Log("stop playing park");
            ParkDone = true;
            //  introplayer.enabled = false;
            panel.SetActive(false);
        }


        if (P1Once && !HasPlayedP1 &&  ICanPlay)
        {
            Invoke("p1_finished", 3);
            HasPlayedP1 = true;
        }
        if (P2Once && !HasPlayedP2 &&  ICanPlay)
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

    IEnumerator playVideo_Donut()
    {
        //Add VideoPlayer to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;

        //We want to play from video clip not from url
        //  videoPlayer.source = videoSource.VideoClip;

        videoPlayer.Prepare();

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        
        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        

        //Set video To Play then prepare Audio to prevent Buffering
  

       
        //videoPlayer.clip = videoToPlay_Donut;
        
        videoPlayer.Prepare();

        //Wait until video is prepared
       

        //Debug.Log("Done Preparing Video");

        //Assign the Texture from Video to RawImage to be displayed
        image.texture = videoPlayer.texture;
      
            //Play Video
            videoPlayer.Play();

            //Play Sound
            audioSource.Play();
            videoIsplaying = true;
            //Debug.Log("Playing Video");
            while (videoPlayer.isPlaying)
            {
                videoIsplaying = true;
                Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
                yield return null;
            }

            //Debug.Log("Done Playing Video");
            //videoObj.SetActive(false);
        
        image.enabled = false;
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
