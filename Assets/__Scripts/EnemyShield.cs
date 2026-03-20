using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(BlinkColorOnHit))]
public class EnemyShield : MonoBehaviour
{
    [Header("Inscribed")]
    public float health = 10;

    private List<EnemyShield> protectors = new List<EnemyShield>();
    private BlinkColorOnHit blinker;

    void Start()
    {
        blinker = GetComponent<BlinkColorOnHit>();
        blinker.ignoreOnCollisionEnter = true; //This will not yet compile

        if(transform.parent == null) return;
        EnemyShield shieldParent = transform.parent.GetComponent<EnemyShield>();
        if(shieldParent != null)
        {
            shieldParent.AddProtector(this);
        }
    }

    public void AddProtector(EnemyShield shieldChild)
    {
        protectors.Add(shieldChild);
    }

    public bool isActive
    {
        get{return gameObject.activeInHierarchy;}
        private set{gameObject.SetActive(value);}
    }

    public float TakeDamage(float dmg)
    {
        //can we pass damage to a protector EnemyShield?
        foreach(EnemyShield es in protectors)
        {
            if (es.isActive)
            {
                dmg= es.TakeDamage(dmg);
                //if all damage was handled, return 0 damage
                if(dmg ==0) return 0;
            }
        }
        //If the code gets here, then this EnemyShield will blink & take damage
        //make the blinker blink
        blinker.SetColors(); //This will appear underlined in red for now

        health -= dmg;
        if (health <= 0)
        {
            //Deactivate this EnemyShield GameObject
            isActive = false;
            //Return any damage that was not absorbed by this EnemyShield
            return -health;
        }
        return 0;
    }
}
