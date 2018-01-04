using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadOnClick : MonoBehaviour {

    public GameObject loadingImage;

    private Image image;

    private string levelsName;
    private bool loading = false;
    private bool loaded = false;

    private AudioSource audio;
    public AudioClip clip;

    void Start()
    {
        image = loadingImage.GetComponent<Image>();
        Color c = image.color;
        c.a = 0;
        image.color = c;
        audio = gameObject.GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        if (loading)
        {
            Color c = image.color;
            c.a += 0.02f;
            print(c.a);
            if (c.a > 1)
            {
                c.a = 1;
                loaded = true;
                loading = false;
            }
            image.color = c;
        }
        if (loaded)
        {
            SceneManager.LoadScene(levelsName);
        }
    }

    public void LoadScene( int level )
    {
        switch (level)
        {
            case 1:
                audio.PlayOneShot(clip);
                sceneLoaded("level1_island");
                break;
            case 5:
                sceneLoaded("level5_Workers_Residence");
                break;

            default:
                break;
        }
    }

    void sceneLoaded(string levelName)
    {
        loading = true;
        loadingImage.SetActive(true);
        levelsName = levelName;
    }

    public void exitGame()
    {
        print("Quiting the game..");
        Application.Quit();
    }
}
