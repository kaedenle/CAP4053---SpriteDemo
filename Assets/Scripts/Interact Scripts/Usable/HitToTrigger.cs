using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitToTrigger :  MonoBehaviour, IDamagable
{
    public enum WeaponType
    {
        BaseballBat,
        Sign,
        Gun,
        Any
    }

    public enum AttackType
    {
        First,
        Second,
        Any
    }

    public string eventID;
    [SerializeField] public WeaponType weaponNeeded;
    [SerializeField] public AttackType attackTypeNeeded;

    public bool destroyOnTrigger = false;

    public bool gainItem = false;
    [SerializeField] public InventoryManager.AllItems itemGained;

    public GameObject[] enables;
    public GameObject[] disables;

    public InteractiveText interactiveText;
    public bool pauseOnInteract = true;

    private InteractiveUIController UI;
    private bool triggered = false;
    private bool activated;

    new void Start()
    {
        activated = LevelManager.GetInteractiveState(eventID);

        if(activated)
        {
            Activate();
        }

        UI = FindObjectOfType<InteractiveUIController>();
        interactiveText.SetText( InteractiveTextDatabase.GetText( interactiveText.GetID() ) );
    }

    new void Update()
    {
        if(triggered && !UI.IsActive())
        {
            activated = true;
            triggered = false;

            // do stuff
            if(gainItem)
                InventoryManager.AddItem(itemGained);
            LevelManager.ToggleInteractiveState(eventID);
            activated = true;

            // enable/disable and possibility destroy this obj
            Activate();
        }
    }

    public void damage(Vector3 knockback, int damage, float hitstun, float hitstop, int weapon)
    {
        if(!activated)
        {
            Debug.Log("I've been hit with weapon " + weapon);

            if(weapon == (int) weaponNeeded || weaponNeeded == WeaponType.Any)
            {
                
            }

            // trigger the actions, but only after the weapon animation finishes
            StartCoroutine(DelayTrigger());
            // assumes UI is not being used
        }
    }

    IEnumerator DelayTrigger()
    {
        yield return new WaitForSecondsRealtime(1F);
        TriggerDialogue(interactiveText);
        triggered = true;
    }

    void Activate()
    {
        foreach(GameObject obj in enables)
        {
            if(obj == null) continue;

            obj.SetActive(true);
        }        

        foreach(GameObject obj in disables)
        {
            if(obj == null) continue;

            obj.SetActive(false);
        }
        
        if(destroyOnTrigger)
            Destroy(gameObject);
    }

    void TriggerDialogue(InteractiveText txt)
    {
        if(txt.IsEmpty()) return;

        // don't trigger if dialogue is currently active
        if(UI.IsActive()) return;

        int index = UIManager.GetInteractiveIndex(txt.GetID());

        if(txt.OutOfBounds(index)) return;

        // pause now if I've made it this far
        if(pauseOnInteract) EntityManager.DialoguePause();
        UI.StartInteractive(txt.GetUnit(index), pauseOnInteract);

        index = txt.CalcNextIndex(index);

        UIManager.SetInteractiveIndex(txt.GetID(), index);
    }

}
