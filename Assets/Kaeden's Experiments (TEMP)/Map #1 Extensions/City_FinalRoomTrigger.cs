using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class City_FinalRoomTrigger : MonoBehaviour
{
    public bool flag = false;
    public bool gunflag = false;
    public bool revealflag = false;
    public PlayableDirector final;
    public PlayableDirector guntutorial;
    public PlayableDirector syringeReveal;
    private CameraShake cs;
    private IEnumerator running;
    private AudioManager audio;
    private AudioSource audiosrc;
    void OnEnable()
    {
        InventoryManager.AddedItem += ExecuteEvent;
        Item.PickedUp += GunTutorial;
    }
    private void Start()
    {
        cs = Camera.main.gameObject.GetComponent<CameraShake>();
        audio = GetComponent<AudioManager>();
        audiosrc = GetComponent<AudioSource>();
    }
    private void OnDisable()
    {
        InventoryManager.AddedItem -= ExecuteEvent;
        Item.PickedUp -= GunTutorial;
    }
    public void GunTutorial(object sender, InventoryManager.AllItems e)
    {
        if (!gunflag && InventoryManager.HasItem(InventoryManager.AllItems.City_Gun))
        {
            if (guntutorial != null) guntutorial.Play();
        }
    }
    public void ExecuteEvent(object sender, InventoryManager.AllItems e)
    {
        //when you have gun, alleykey, and syringe
        if (InventoryManager.HasItem(InventoryManager.AllItems.City_Syringe)
            && gunflag
            && InventoryManager.HasItem(InventoryManager.AllItems.City_AlleyKey)
            && !flag)
        {
            StopCoroutine(running);
            flag = true;
            if (final != null) final.Play();
        }
    }
    public void CheckReveal()
    {
        if (InventoryManager.HasItem(InventoryManager.AllItems.City_Gun)
           && InventoryManager.HasItem(InventoryManager.AllItems.City_AlleyKey) && !revealflag)
        {
            if (syringeReveal != null) syringeReveal.Play();
            revealflag = true;
        }
    }
    IEnumerator Shaking()
    {
        yield return new WaitForSeconds(Random.Range(4, 8));
        audiosrc.Stop();
        if (audio != null) audio.PlayAudio(0);
        cs.StartShake(0.5f);
        running = null;
    }
    public void Finished()
    {
        gunflag = true;
    }
    private void Update()
    {
        if (running == null && !flag)
        {
            running = Shaking();
            StartCoroutine(running);
        }
        if (gunflag && InventoryManager.HasItem(InventoryManager.AllItems.City_AlleyKey)) CheckReveal();
        if (!flag) ExecuteEvent(null, InventoryManager.AllItems.City_Paper1);
    }
}
