using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickScenLoader : MonoBehaviour {

    public void NextScene()
    {
        SceneManager.LoadScene("dev_Bedroom");
    }
}

