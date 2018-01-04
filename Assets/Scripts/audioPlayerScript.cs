using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class audioPlayerScript : MonoBehaviour {

    // MUSICS
    public enum allMusics { NONE, TWENTIETH_C, TWENTIETH_C_ZOMBIEMAN, ALARM, LOOP, MELANCHOLY_FAST, PUHEGEN_TEST,
        SYNKKY, TAUSTA_AMBIENT, ZOMBIEMAN, HAUNTING, CREDITS, BASSOPIANOTHEME, HEARTBEATVALINTA, KUUMOTTELU,
        NATUREAMBIENCE, OCEAN };
    public allMusics nowPlaying = allMusics.NONE;
    private allMusics swapVariable = allMusics.NONE;

    [Range(0.0f, 100.0f)]

    public float volumeSlider = 50.0f;

    private AudioSource player;
    public AudioClip twentiethCentuary;
    public AudioClip twentiethZombieman;
    public AudioClip alarm;
    public AudioClip loop;
    public AudioClip melancholyFast;
    public AudioClip puhegenTest;
    public AudioClip synkky;
    public AudioClip taustaAmbient;
    public AudioClip zombieman;
    public AudioClip haunting;
    public AudioClip credits;
    public AudioClip bassoPiano;
    public AudioClip heartbeatValinta;
    public AudioClip kuumottelu;
    public AudioClip natureAmbient;
    public AudioClip ocean;


    // Use this for initialization
    void Start () {
        player = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {

        player.volume = volumeSlider / 100;

        if (swapVariable != nowPlaying)
        {
            switch (nowPlaying)
            {
                case allMusics.NONE:
                    player.Stop();
                    swapVariable = nowPlaying;
                    break;
                case allMusics.TWENTIETH_C:
                    changeToThisSong(twentiethCentuary);
                    break;
                case allMusics.TWENTIETH_C_ZOMBIEMAN:
                    changeToThisSong(twentiethZombieman);
                    break;
                case allMusics.ALARM:
                    changeToThisSong(alarm);
                    break;
                case allMusics.LOOP:
                    changeToThisSong(loop);
                    break;
                case allMusics.MELANCHOLY_FAST:
                    changeToThisSong(melancholyFast);
                    break;
                case allMusics.PUHEGEN_TEST:
                    changeToThisSong(puhegenTest);
                    break;
                case allMusics.SYNKKY:
                    changeToThisSong(synkky);
                    break;
                case allMusics.TAUSTA_AMBIENT:
                    changeToThisSong(taustaAmbient);
                    break;
                case allMusics.ZOMBIEMAN:
                    changeToThisSong(zombieman);
                    break;
                case allMusics.HAUNTING:
                    changeToThisSong(haunting);
                    break;
                case allMusics.CREDITS:
                    changeToThisSong(credits);
                    break;
                case allMusics.BASSOPIANOTHEME:
                    changeToThisSong(bassoPiano);
                    break;
                case allMusics.HEARTBEATVALINTA:
                    changeToThisSong(heartbeatValinta);
                    break;
                case allMusics.KUUMOTTELU:
                    changeToThisSong(kuumottelu);
                    break;
                case allMusics.NATUREAMBIENCE:
                    changeToThisSong(natureAmbient);
                    break;
                case allMusics.OCEAN:
                    changeToThisSong(ocean);
                    break;
                default:
                    break;
            }
        }
	}

    public void stopAllAudio()
    {
        nowPlaying = allMusics.NONE;
    }

    public void changeSong( allMusics music )
    {
        nowPlaying = music;
    }

    void changeToThisSong(AudioClip clip)
    {
        player.Stop();
        player.loop = true;
        player.clip = clip;
        player.Play();
        swapVariable = nowPlaying;
    }

    public void changeVolume(int volumme)
    {
        volumeSlider = volumme;
    }

}
