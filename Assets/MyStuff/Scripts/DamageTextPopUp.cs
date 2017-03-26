using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageTextPopUp : MonoBehaviour
{
    
    public Animator mAnimator;
    public Text mText;

	void Start()
    {

	}
	
	
	public void SetText(string damage)
    {
        AnimatorClipInfo[] Clip = mAnimator.GetNextAnimatorClipInfo(0);
        mText = GetComponentInChildren<Text>();
        mText.text = damage;
        Destroy(gameObject, 1.0f/*Clip[0].clip.length*/);
	}
}
