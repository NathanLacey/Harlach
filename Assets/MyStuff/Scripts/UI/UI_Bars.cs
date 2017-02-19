using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Bars : MonoBehaviour
{
    public const float MaxHealth = 100.0f;
    public const float MaxStamina = 100.0f;
    public const float MaxMana = 100.0f;
    public const float Cost_Stamina_Sword1h = 10.0f;
    public const float Cost_Stamina_Sword2h = 20.0f;

    [SerializeField]
    Image ImgHealth;
    [SerializeField]
    Image ImgStamina;
    [SerializeField]
    Image ImgMana;
    public float HealthBar
    {
        set
        {
            ImgHealth.fillAmount = value/MaxHealth;
        }
    }
    public float StaminaBar
    {
        set
        {
            ImgStamina.fillAmount = value/MaxStamina;
        }
    }
    public float ManaBar
    {
        set
        {
            ImgMana.fillAmount = value/MaxMana;
        }
    }
    private UI_Bars() { }

    private static UI_Bars TheInstance;
    public static UI_Bars Instance
    {
        get
        {
            return TheInstance;
        }
    }

    void Awake()
    {
        TheInstance = this;
    }

    
    
}
