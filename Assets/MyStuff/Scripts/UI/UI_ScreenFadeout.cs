using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UI_ScreenFadeout : MonoBehaviour
{
    private UI_ScreenFadeout() { }
    private static UI_ScreenFadeout TheInstance;
    public static UI_ScreenFadeout Instance
    {
        get
        {
            return TheInstance;
        }
    }

    Image FadeImg;
    public float mFadeSpeed = 2.5f;
    public float mTimerInterval = 0.1f;
    Timer FadeTimer = new Timer();
    bool IsFadingBlack = false;
    bool IsFadingClear = false;

    void Awake()
    {
        FadeTimer.Initialize(mTimerInterval);
        TheInstance = this;
        FadeImg = transform.GetComponent<Image>();
        FadeImg.color = Color.clear;
        //FadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
        FadeImg.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        enabled = false;
    }

    void FixedUpdate()
    {
        if(IsFadingBlack)
        {
            FadeTimer.TimerAction(FadeToBlack);

            if(FadeImg.color.a >= 0.95f)
            {
                IsFadingBlack = false;
                IsFadingClear = true;
            }
        }
        else if(IsFadingClear)
        {
            FadeTimer.TimerAction(FadeToClear);

            if(FadeImg.color.a <= 0.05f)
            {
                FadeImg.color = Color.clear;
                IsFadingClear = false;
                enabled = false;
            }
        }
    }

    public void Fade(float fadeSpeed = 1.0f)
    {
        IsFadingBlack = true;
        enabled = true;
    }

    void FadeToClear()
    {
        // Lerp the colour of the image between itself and transparent.
        FadeImg.color = Color.Lerp(FadeImg.color, Color.clear, mFadeSpeed * Time.deltaTime);
    }

    void FadeToBlack()
    {
        // Lerp the colour of the image between itself and black.
        FadeImg.color = Color.Lerp(FadeImg.color, Color.black, mFadeSpeed * Time.deltaTime);
    }

    //public bool IsDoneFading()
    //{
    //    if(FadeImg.color.a <= 0.05f)
    //    {
    //        FadeImg.color = Color.clear;
    //        FadeImg.enabled = false;
    //    }
    //}

    //void StartScene()
    //{
    //    // Fade the texture to clear.
    //    FadeToClear();

    //    // If the texture is almost clear...
    //    if (FadeImg.color.a <= 0.05f)
    //    {
    //        // ... set the colour to clear and disable the RawImage.
    //        FadeImg.color = Color.clear;
    //        FadeImg.enabled = false;

    //        // The scene is no longer starting.
    //        sceneStarting = false;
    //    }
    //}
}