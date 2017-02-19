using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum DamageType { Melee_Instance, Melee_Bleeding };
public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject MyCanvas;
    [SerializeField]
    Animator mAnimator;
    [SerializeField]
    public AttackController mAttackControllerLeft;
    [SerializeField]
    public AttackController mAttackControllerRight;
    
    Timer BleedingTimer = new Timer();
    Timer StaminaRegen = new Timer();

    [SerializeField]
    private float StaminaRegenWait;
    private bool IsBleeding = false;
    private bool CanRegenStamina = false;
    private float Health;
    private float PlayerHealth
    {
        set
        {
            Health = value;
            UI_Bars.Instance.HealthBar = Health;
            if(Health <= 0.0f)
            {
                Respawn();
            }
        }
        get
        {
            return Health;
        }
    }
    private float Stamina;
    private float PlayerStamina
    {
        set
        {
            if (value > UI_Bars.MaxStamina)
                return;
            if(value < Stamina)
            {
                StopAllCoroutines();
                StartCoroutine(StaminaRegenWindow(StaminaRegenWait));
            }
            Stamina = value;
            UI_Bars.Instance.StaminaBar = Stamina;
        }
        get
        {
            return Stamina;
        }
    }
    private bool IsInvincible = false;
    private bool IsAttacking = false;
    private Transform RightHand;
    private Transform LeftHand;

    void Start()
    {
        BleedingTimer.Initialize(1.0f);
        StaminaRegen.Initialize(0.1f);
        MyCanvas = Instantiate(MyCanvas);
        Respawn();
        LeftHand = transform.GetChild(0).GetChild(0);
        RightHand = transform.GetChild(0).GetChild(1);
        // For testing
        if (RightHand.childCount > 0)
        {
            mAttackControllerRight = RightHand.GetChild(0).GetComponent<AttackController>();
            mAttackControllerRight.transform.GetComponent<Animator>().SetBool("FloatingItem", false);
            UI_HandWeapons.Instance.SetRightHandImage(ImageType.Sword);
            mAttackControllerRight.LoadAnimator("Animators/SwordAnimator");
        }
        //
    }

    void Respawn()
    {
        Checkpoint currentCheckpoint = CheckpointManager.Instance.GetCurrentCheckpoint();
        transform.position = currentCheckpoint.transform.position;
        transform.rotation = currentCheckpoint.transform.rotation;
        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.Init(transform, Camera.main.transform);
        IsBleeding = false;
        PlayerHealth = UI_Bars.MaxHealth;
        PlayerStamina = UI_Bars.MaxStamina;
        StartCoroutine(InvincibilityWindow(1.5f));
    }

    IEnumerator InvincibilityWindow(float waitTime)
    {
        IsInvincible = true;
        yield return new WaitForSeconds(waitTime);
        IsInvincible = false;
    }

    IEnumerator StaminaRegenWindow(float waitTime)
    {
        CanRegenStamina = false;
        yield return new WaitForSeconds(waitTime);
        CanRegenStamina = true;
    }

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            MyCanvas.SetActive(!MyCanvas.activeSelf);
        }

        if(IsBleeding)
        {
            BleedingTimer.TimerAction(BleedingDamage);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && CanAttack("RightHand"))
        {
            //Debug.Log("left click attack");
            mAttackControllerRight.Attack();
            UseWeapon("RightHand");
            
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1) && CanAttack("LeftHand"))
        {
            //Debug.Log("left click attack");
            mAttackControllerLeft.Attack();
            UseWeapon("LeftHand");
        }
        else if(CanRegenStamina == true)
        {
            StaminaRegen.TimerAction(RegenerateStamina);
        }
    }

    void RegenerateStamina()
    {
        ++PlayerStamina;
    }

    bool CanAttack(string hand)
    {
        if(hand == "LeftHand")
        {
            if (mAttackControllerLeft == null)
                return false;

            if (mAttackControllerLeft.tag == "sword1h")
            {
                return PlayerStamina > UI_Bars.Cost_Stamina_Sword1h;
            }
            else if (mAttackControllerLeft.tag == "sword2h")
            {
                return PlayerStamina > UI_Bars.Cost_Stamina_Sword2h;
            }
        }
        else if (hand == "RightHand")
        {
            if (mAttackControllerRight == null)
                return false;

            if (mAttackControllerRight.tag == "sword1h")
            {
                return PlayerStamina > UI_Bars.Cost_Stamina_Sword1h;
            }
            else if(mAttackControllerRight.tag == "sword2h")
            {
                return PlayerStamina > UI_Bars.Cost_Stamina_Sword2h;
            }
        }

        Debug.Log("[Player::CanAttack] Invalid parameter");
        return false;
    }

    void UseWeapon(string hand)
    {
        if (hand == "LeftHand")
        {
            if (mAttackControllerLeft.tag == "sword1h")
            {
                PlayerStamina -= UI_Bars.Cost_Stamina_Sword1h;
            }
            else if(mAttackControllerLeft.tag == "sword2h")
            {
                PlayerStamina -= UI_Bars.Cost_Stamina_Sword2h;
            }
        }
        else if (hand == "RightHand")
        {
            if (mAttackControllerRight.tag == "sword1h")
            {
                PlayerStamina -= UI_Bars.Cost_Stamina_Sword1h;
            }
            else if (mAttackControllerRight.tag == "sword2h")
            {
                PlayerStamina -= UI_Bars.Cost_Stamina_Sword2h;
            }
        }
    }

    void BleedingDamage()
    {
        Damage(1.0f, DamageType.Melee_Instance, 0.3f);
        if(Random.Range(0, 5) == 0)
        {
            IsBleeding = false;
        }
    }

    public void Damage(float amount, DamageType damageType, float invincibilityTime = 1.0f)
    {
        if (IsInvincible)
            return;
        StartCoroutine(InvincibilityWindow(invincibilityTime));

        switch (damageType)
        {
            case DamageType.Melee_Instance:
                PlayerHealth -= amount;
                break;
            case DamageType.Melee_Bleeding:
                PlayerHealth -= amount;
                IsBleeding = true;
                break;
            default:
                Debug.Log("[Player::Damage] Invalid damage type");
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Trap" && collider.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Active"))
        {
            Damage(collider.GetComponentInParent<Trap>().damageAmount, collider.GetComponentInParent<Trap>().damageType, 1.0f);
        }
    }
}
