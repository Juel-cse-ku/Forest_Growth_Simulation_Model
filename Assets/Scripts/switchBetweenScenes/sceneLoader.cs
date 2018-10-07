using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneLoader : MonoBehaviour {

    public void changeScene(string scenename)
    {
        Application.LoadLevel(scenename);
    }
}
