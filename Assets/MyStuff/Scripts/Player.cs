using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum DamageType { Melee_Instance, Melee_Bleeding };
public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject MyCanvas;

    // TEMPORARY
    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2 - 50, 0, 100, 20), "Health: " + Health);
    }
    //
    Timer BleedingTimer = new Timer();
    private bool IsBleeding = false;
    const float MaxHealth = 100.0f;
    private float Health;
    private float PlayerHealth
    {
        set
        {
            Health = value;
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
    private bool IsInvincible = false;
    void Start()
    {
        BleedingTimer.Initialize(1.0f);
        Respawn();
        MyCanvas = Instantiate(MyCanvas);
    }

    void Respawn()
    {
        Checkpoint currentCheckpoint = CheckpointManager.Instance.GetCurrentCheckpoint();
        transform.position = currentCheckpoint.transform.position;
        transform.rotation = currentCheckpoint.transform.rotation;
        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.Init(transform, Camera.main.transform);
        IsBleeding = false;
        Health = MaxHealth;
        StartCoroutine(InvincibilityWindow(1.5f));
    }

    IEnumerator InvincibilityWindow(float waitTime)
    {
        IsInvincible = true;
        yield return new WaitForSeconds(waitTime);
        IsInvincible = false;
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
