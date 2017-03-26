using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour {

    public void ButtonPlay_Clicked()
    {
        SceneManager.LoadScene("Rooms");
    }
}
