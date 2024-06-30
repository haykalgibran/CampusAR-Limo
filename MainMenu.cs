using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void Confirm()
    {
        SceneManager.LoadScene("Confirm"); 
    }
    
    public void Mulai()
    {
        SceneManager.LoadScene("Mulai"); 
    }

    public void Tentang()
    {
        SceneManager.LoadScene("Tentang"); 
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial"); 
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
