using UnityEngine;
using System.Collections;

public class WeaponCamera : MonoBehaviour
{
    [SerializeField]
    RenderTexture renderTarget;

    void OnGUI()
    {
        //GUI.DrawTexture(Rect(0, 0, Screen.width, Screen.height), renderTarget.);
    }
}
