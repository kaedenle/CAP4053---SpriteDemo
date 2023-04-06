using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject parent;
    public Color transformColor;
    private Color[] originalColor;
    private Text[] affect;
    private Button AttachedButton;
    public bool Disable;
    private GameManager gm;
    //OnPointerDown is also required to receive OnPointerUp callbacks
    /*public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < affect.Length; i++)
             affect[i].color = transformColor;
    }

    //Do this when the mouse click on this selectable UI object is released.
    public void OnPointerUp(PointerEventData eventData)
    {
        for (int i = 0; i < affect.Length; i++)
            affect[i].color = originalColor[i];
    }*/
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Disable) return;
        for (int i = 0; i < affect.Length; i++)
            affect[i].color = transformColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Disable) return;
        for (int i = 0; i < affect.Length; i++)
            affect[i].color = originalColor[i];
    }
    void OnDisable()
    {
        if (affect == null || originalColor == null) return;
        for (int i = 0; i < affect.Length; i++)
            affect[i].color = originalColor[i];
    }
    public void QuitGame()
    {
        ScenesManager.LoadScene(ScenesManager.AllScenes.Menu);
    }
    public void Restart()
    {
        if(gm == null) gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gm != null) gm.RemoveCurrentScene();
        LevelManager.RestartLevel();
        Debug.Log("RESTART!");
        
    }
    // Start is called before the first frame update
    void Start()
    {
        AttachedButton = gameObject?.GetComponent<Button>();
        if (AttachedButton != null) AttachedButton.interactable = !Disable;

        affect = parent != null ? parent.GetComponentsInChildren<Text>() : new Text[0];
        originalColor = new Color[affect.Length];
        for (int i = 0; i < affect.Length; i++)
            originalColor[i] = affect[i].color;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
