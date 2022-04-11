using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI antColText;
    [SerializeField]
    TextMeshProUGUI startingAntText;
    [SerializeField]
    TextMeshProUGUI startingPlantText;
    [SerializeField]
    TextMeshProUGUI speedText;
    [SerializeField]
    TextMeshProUGUI strengthText;
    [SerializeField]
    TextMeshProUGUI energyText;
    [SerializeField]
    TextMeshProUGUI maxAntsText;
    [SerializeField]
    GameObject spreadSlider;
    [SerializeField]
    TextMeshProUGUI spreadDistanceText;
    void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Winter", 0);
        PlayerPrefs.SetInt("Colony", 2);
        PlayerPrefs.SetInt("Plants", 5);
        PlayerPrefs.SetInt("Ants", 5);
        PlayerPrefs.SetInt("MaxAnts", 300);
        PlayerPrefs.SetFloat("Strength", 1);
        PlayerPrefs.SetFloat("Speed", 5);
        PlayerPrefs.SetFloat("Energy", 200);
        PlayerPrefs.SetInt("SpawnPattern", 0);
        PlayerPrefs.SetInt("Spread", 5);
        PlayerPrefs.SetInt("Spawn", 0);
    }
    void Update()
    {
        if (PlayerPrefs.GetInt("SpawnPattern") != 0)
        {
            spreadSlider.SetActive(true);
        }
        else
        {
            spreadSlider.SetActive(false);
        }
    }
    public void SetAntSpawn(int value)
    {
        PlayerPrefs.SetInt("Spawn", value);
    }
    public void SetSpread(float distance)
    {
        PlayerPrefs.SetInt("Spread", Mathf.FloorToInt((distance)));
        spreadDistanceText.text = "Spread Distance: " + distance;
    }
    public void SetSpawnPattern(int value)
    {
        PlayerPrefs.SetInt("SpawnPattern", value);
    }
    public void SetColonies(float colonies)
    {
        PlayerPrefs.SetInt("Colony", Mathf.FloorToInt(colonies));
        antColText.text = "Ant Colonies: " + colonies;
    }
    public void SetMaxAnts(float maxAnts)
    {
        PlayerPrefs.SetInt("MaxAnts", Mathf.FloorToInt(maxAnts));
        maxAntsText.text = "Max Ants: " + maxAnts;
    }
    public void SetPlants(float plants)
    {
        PlayerPrefs.SetInt("Plants", Mathf.FloorToInt(plants));
        startingPlantText.text = "Starting Plants: " + plants;
    }
    public void SetAnts(float ants)
    {
        PlayerPrefs.SetInt("Ants", Mathf.FloorToInt(ants));
        startingAntText.text = "Starting Ants: " + ants;
    }
    public void SetSpeed(float speed)
    {
        PlayerPrefs.SetFloat("Speed", speed);
        speedText.text = "Ant Speed: " + Math.Round(speed, 2);
    }
    public void SetEnergy(float energy)
    {
        PlayerPrefs.SetFloat("Energy", energy);
        energyText.text = "Ant Energy: " + Math.Round(energy, 2);
    }
    public void SetStrength(float strength)
    {
        PlayerPrefs.SetFloat("Strength", strength);
        strengthText.text = "Ant Strength: " + Math.Round(strength, 2);
    }
    public void SetWinter(bool winter)
    {
        if (winter)
        {
            PlayerPrefs.SetInt("Winter", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Winter", 0);
        }

    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }


}
