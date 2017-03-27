using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuControls : MonoBehaviour
{
    Slider mSlider;
    UI_SelectHandler mSliderSelect;
    bool isSliderSelectedPrev;
    [SerializeField]
    GameObject mControls;
    [SerializeField]
    float mVolume;
    [SerializeField]
    Player mPlayer;
    bool mActive;
    bool IsActive
    {
        set
        {
            mControls.SetActive(value);
            mActive = value;
            mPlayer.CanvasSetActive(!value);
            if(value == true)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        get
        {
            return mActive;
        }
    }

    void Awake()
    {
        IsActive = false;
        mSlider = mControls.GetComponentInChildren<Slider>();
        mSliderSelect = mSlider.GetComponentInChildren<UI_SelectHandler>();
        mVolume = AudioListener.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            IsActive = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (!IsActive)
            return;

        // If the slider is actively selected, do nothing here,
        // just let the user drag the slider around until they release it.
        if (!mSliderSelect.IsSelected)
        {
            if (isSliderSelectedPrev)
            {
                // Edge event, user has just released the mouse button.
                // Set the timeline value to match the slider position.
                mVolume = mSlider.value;
                AudioListener.volume = mVolume;
            }
            else
            {
                // Not selected, slider follows the timeline.
                mSlider.value = mVolume;
            }
        }

        isSliderSelectedPrev = mSliderSelect.IsSelected;
    }

    public void ButtonExit_Clicked()
    {
#if UNITY_EDITOR
        Debug.Break();
#else
        Application.Quit();
#endif
    }

    public void ButtonResume_Clicked()
    {
        IsActive = false;
    }
}
