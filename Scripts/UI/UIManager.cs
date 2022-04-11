using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject dataPanel;
    [SerializeField]
    GameObject WinterText;
    [SerializeField]
    GameObject PowerPanel;
    [SerializeField]
    GameObject PowerButton;
    [SerializeField]
    GameObject Crosshair;
    Fruit fruitMan;
    public HiveCollector colonyData;
    // Start is called before the first frame update
    void Start()
    {
        fruitMan = gameObject.GetComponent<Fruit>();
        dataPanel.SetActive(false);
        PowerPanel.SetActive(false);
        colonyData = null;
    }

    public void ShowPowers()
    {
        PowerPanel.SetActive(!PowerPanel.activeSelf);
        PowerButton.transform.Rotate(0, 0, 180);
        Crosshair.GetComponent<CrosshairControlls>().State = CrosshairStates.Off; 
    }
    public void SetFruit()
    {
        CrosshairControlls reff = Crosshair.GetComponent<CrosshairControlls>();
        if (reff.State == CrosshairStates.Fruit)
        {
            reff.State = CrosshairStates.Off;
        }
        else
        {
            reff.State = CrosshairStates.Fruit;
        }

    }

    public void SetBoot()
    {
        {
            CrosshairControlls reff = Crosshair.GetComponent<CrosshairControlls>();
            if (reff.State == CrosshairStates.Boot)
            {
                reff.State = CrosshairStates.Off;
            }
            else
            {
                reff.State = CrosshairStates.Boot;
            }

        }
    }

    void Update()
    {
        if (colonyData != null)
        {
            dataPanel.SetActive(true);

            TextMeshProUGUI[] textBoxes = dataPanel.GetComponentsInChildren<TextMeshProUGUI>();
            textBoxes[0].text = colonyData.Color;
            textBoxes[1].text = "Ants - " + colonyData.AntCount;
            textBoxes[2].text = "Fruit - " + colonyData.FruitCount;
            textBoxes[3].text = "Corpses - " + colonyData.CorpseCount;
            textBoxes[4].text = "Total Fruit - " + colonyData.TotalFruitCount;
            textBoxes[5].text = "Most ants - " + colonyData.AntHigh;
        }
        else if (colonyData == null)
        {
            dataPanel.SetActive(false);
        }
        if (fruitMan.Winter)
        {
            WinterText.SetActive(true);
        }
        else
        {
            WinterText.SetActive(false);
        }
    }
}
