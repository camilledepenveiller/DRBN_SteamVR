using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn_Menu : MonoBehaviour
{
    //public GameObject UI_inactivate;
    public List<GameObject> UI_inactivate;
    public Button Spawn, Snap, Reset;

    void Start()
    {
        
        
        Spawn.onClick.AddListener(SpawnMenoush);
        Snap.onClick.AddListener(SnapMenoush);
        Reset.onClick.AddListener(ResetMenoush);
        //Reset.onClick.AddListener(() => ButtonClicked(42));


        for (int i = 0; i < UI_inactivate.Count; i++)
        {
            if (UI_inactivate[i].activeSelf == true)
            {
                UI_inactivate[i].SetActive(false);
            }
        }
    }

    public void SpawnMenoush()
    {
        if (UI_inactivate[0].activeSelf == false)
        {
            UI_inactivate[0].SetActive(true);
            UI_inactivate[1].SetActive(false);
            UI_inactivate[2].SetActive(false);
        }
    }

    public void SnapMenoush()
    {
        if (UI_inactivate[1].activeSelf == false)
        {
            UI_inactivate[0].SetActive(false);
            UI_inactivate[1].SetActive(true);
            UI_inactivate[2].SetActive(false);
        }
    }

    public void ResetMenoush()
    {
        if (UI_inactivate[2].activeSelf == false)
        {
            UI_inactivate[0].SetActive(false);
            UI_inactivate[1].SetActive(false);
            UI_inactivate[2].SetActive(true);
        }
    }
}
