using UnityEngine;
using System.Collections;

public class DamageTextController : MonoBehaviour
{
    [SerializeField]
    DamageTextPopUp mPopUpText;
    [SerializeField]
    GameObject mCanvas;

	public void Initialize()
    {
        //mCanvas = GameObject.Find("Canvas");
        //mPopUpText = Resources.Load<DamageTextPopUp>("Misc/DamageParent");
        //Debug.Log(mPopUpText.ToString());
	}


    public void SpawnText(string text, Transform Position)
    {
        mPopUpText = Resources.Load<DamageTextPopUp>("Misc/DamageParent");
        mCanvas = GameObject.Find("Canvas");
        DamageTextPopUp SpawnedText = Instantiate(mPopUpText);
        SpawnedText.transform.SetParent(mCanvas.transform, false);
        SpawnedText.transform.position = Position.transform.position;
        SpawnedText.transform.position += new Vector3(0.0f,3.1f,0.0f);
        //SpawnedText.transform.LookAt(Position.transform.position + Camera.main.transform.position * Vector3.forward, Vector3.up);
        //SpawnedText.transform.LookAt(Camera.main.transform.position);
        SpawnedText.SetText(text);
    }
}
