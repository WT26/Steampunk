using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioTriggerScirpt : MonoBehaviour {

    public bool isAudioMusic = true;
    public bool stopAllAudio = false;
    public bool oneShot = false;
    public playerMovement.allSfx soundToPlay = playerMovement.allSfx.NONE;
    public audioPlayerScript.allMusics musicToPlay = audioPlayerScript.allMusics.NONE;

    public GameObject soundPlayer;
    public GameObject musicPlayer;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player"){
            if (stopAllAudio)
            {
                musicPlayer.SendMessage("stopAllAudio");
            }
            else
            {
                if (isAudioMusic)
                {
                    if (musicToPlay == audioPlayerScript.allMusics.NATUREAMBIENCE)
                    {
                        musicPlayer.SendMessage("changeVolume", 15);
                    }
                    else
                    {
                        musicPlayer.SendMessage("changeVolume", 50);
                    }
                    musicPlayer.SendMessage("changeSong", musicToPlay);
                }
                else
                {
                    other.transform.SendMessage("playOnce", soundToPlay);
                }

                if (oneShot == true)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
