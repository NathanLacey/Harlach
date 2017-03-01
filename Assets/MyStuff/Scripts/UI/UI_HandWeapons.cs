using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum ImageType { Sword, Shield, Magic, None }
public class UI_HandWeapons : MonoBehaviour
{
    [SerializeField]
    Text AttackValueLeft;
    [SerializeField]
    Text AttackValueRight;
    [SerializeField]
    Text DefenceValueLeft;
    [SerializeField]
    Text DefenceValueRight;

    [SerializeField]
    Image LeftHandWeapon;

    [SerializeField]
    Image RightHandWeapon;

    [Header("Sprites")]
    [Tooltip("Sword, Shield, Magic, None")]
    [SerializeField]
    List<Sprite> Sprites = new List<Sprite>();
    private UI_HandWeapons() { }

    private static UI_HandWeapons TheInstance;
    public static UI_HandWeapons Instance
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

    public ImageType FolderNameToImageType(string nameOfType)
    {
        ImageType returnImageType = ImageType.None;

        for(int i = 0; i < ItemSpawner.Instance.FolderNames.Count; ++i)
        {
            if(ItemSpawner.Instance.FolderNames[i] == nameOfType)
            {
                returnImageType = (ImageType)i;
            }
        }

        return returnImageType;
    }

    public void SetLeftHandImage(ImageType type)
    {
        int intValueOfType = (int)type;

        if(Sprites.Count > intValueOfType)
        {
            LeftHandWeapon.sprite = Sprites[intValueOfType];
        }
    }

    public void SetRightHandImage(ImageType type)
    {
        int intValueOfType = (int)type;

        if (Sprites.Count > intValueOfType)
        {
            RightHandWeapon.sprite = Sprites[intValueOfType];
        }
    }

    public void SetAttackValueLeft(int value)
    {
        AttackValueLeft.text = value.ToString();
    }

    public void SetAttackValueRight(int value)
    {
        AttackValueRight.text = value.ToString();
    }

    public void SetDefenceValueLeft(int value)
    {
        DefenceValueLeft.text = value.ToString();
    }

    public void SetDefenceValueRight(int value)
    {
        DefenceValueRight.text = value.ToString();
    }
}
