using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loudspeakerScript : MonoBehaviour {

    public bool active = false;
    public bool loopTexts = false;
    public int intervalSeconds = 2;

    public string textToSpeak1;
    public string textToSpeak2;
    public string textToSpeak3;
    public string textToSpeak4;
    public string textToSpeak5;
    public List<string> allTexts;

    private int currentTextToSay;
    private int frameCounter;


    // Use this for initialization
    void Start () {
        
        currentTextToSay = 0;

        if (textToSpeak1 != "")
        {
            allTexts.Add(textToSpeak1);
        }
        if (textToSpeak2 != "")
        {
            allTexts.Add(textToSpeak2);
        }
        if (textToSpeak3 != "")
        {
            allTexts.Add(textToSpeak3);
        }
        if (textToSpeak4 != "")
        {
            allTexts.Add(textToSpeak4);
        }
        if (textToSpeak5 != "")
        {
            allTexts.Add(textToSpeak5);
        }

    }

    // Update is called once per frame
    void Update () {

        if (active)
        {
            VikingCrewTools.SpeechbubbleManager.Instance.AddSpeechbubble(transform, allTexts[currentTextToSay], VikingCrewTools.SpeechbubbleManager.SpeechbubbleType.SERIOUS);

            frameCounter++;
            if (frameCounter > (intervalSeconds * 60))
            {
                if (currentTextToSay == allTexts.Count - 1 && loopTexts)
                {
                    currentTextToSay = 0;
                }
                else if (currentTextToSay == allTexts.Count - 1 && !loopTexts)
                {
                    active = false;
                }
                else
                {
                    currentTextToSay++;
                }

                frameCounter = 0;
            }
        }

    }
    
    public void toggleSpeaker()
    {
        if (active)
        {
            active = false;
        }
        else
        {
            active = true;
        }
    }
}
