
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject cameraMain;
    public GameObject cameraTwo;
    public GameObject cameraThree;
    public GameObject cameraFour;


    AudioListener cameraMainAudioLis;
    AudioListener cameraTwoAudioLis;
    AudioListener cameraThreeAudioLis;
    AudioListener cameraFourAudioLis;

    // Use this for initialization
    void Start()
    {

        //Get Camera Listeners
        cameraMainAudioLis = cameraMain.GetComponent<AudioListener>();
        cameraTwoAudioLis = cameraTwo.GetComponent<AudioListener>();
        cameraThreeAudioLis = cameraThree.GetComponent<AudioListener>();
        cameraFourAudioLis = cameraFour.GetComponent<AudioListener>();

        //Camera Position Set
        cameraPositionChange(PlayerPrefs.GetInt("CameraPosition"));
    }

    // Update is called once per frame
    void Update()
    {
        //Change Camera Keyboard
        switchCamera();
    }

    //UI JoyStick Method
    public void cameraPositonM()
    {
        cameraChangeCounter();
    }

    //Change Camera Keyboard
    void switchCamera()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
        {
            cameraChangeCounter();
        }
    }

    //Camera Counter
    void cameraChangeCounter()
    {
        int cameraPositionCounter = PlayerPrefs.GetInt("CameraPosition");
        cameraPositionCounter++;
        cameraPositionChange(cameraPositionCounter);
    }

    //Camera change Logic
    void cameraPositionChange(int camPosition)
    {
        if (camPosition > 3)
        {
            camPosition = 0;
        }

        //Set camera position database
        PlayerPrefs.SetInt("CameraPosition", camPosition);

        //Set camera position 1
        if (camPosition == 0)
        {
            cameraMain.SetActive(true);
            cameraMainAudioLis.enabled = true;

            cameraTwoAudioLis.enabled = false;
            cameraTwo.SetActive(false);

            cameraThreeAudioLis.enabled = false;
            cameraThree.SetActive(false);

            cameraFourAudioLis.enabled = false;
            cameraFour.SetActive(false);
        }

        //Set camera position 2
        if (camPosition == 1)
        {
            cameraTwo.SetActive(true);
            cameraTwoAudioLis.enabled = true;

            cameraMainAudioLis.enabled = false;
            cameraMain.SetActive(false);

            cameraThreeAudioLis.enabled = false;
            cameraThree.SetActive(false);

            cameraFourAudioLis.enabled = false;
            cameraFour.SetActive(false);
        }

        if (camPosition == 2)
        {
            cameraTwo.SetActive(false);
            cameraTwoAudioLis.enabled = false;

            cameraMainAudioLis.enabled = false;
            cameraMain.SetActive(false);

            cameraThreeAudioLis.enabled = true;
            cameraThree.SetActive(true);

            cameraFourAudioLis.enabled = false;
            cameraFour.SetActive(false);
        }

        if (camPosition == 3)
        {
            cameraTwo.SetActive(false);
            cameraTwoAudioLis.enabled = false;

            cameraMainAudioLis.enabled = false;
            cameraMain.SetActive(false);

            cameraThreeAudioLis.enabled = false;
            cameraThree.SetActive(false);

            cameraFourAudioLis.enabled = true;
            cameraFour.SetActive(true);
        }

    }
}
