using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Confirm: MonoBehaviour
{
    // Start is called before the first frame update
    public void btnNo()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void btnYes()
    {
        Application.Quit();
        Debug.Log("Game Close");
    }


    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
