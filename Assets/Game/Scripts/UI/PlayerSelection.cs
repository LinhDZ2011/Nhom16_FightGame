using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    public GameObject playerCharacters;
    private GameObject[] allCharacters;
    private int currentIndex = 0;

    void Start()
    {
        allCharacters = new GameObject[playerCharacters.transform.childCount];

        for (int i = 0; i < playerCharacters.transform.childCount; i++)
        {
            allCharacters[i] = playerCharacters.transform.GetChild(i).gameObject;
            allCharacters[i].SetActive(false);
        }

        if(PlayerPrefs.HasKey("SelectedCharacterIndex"))
        {
            currentIndex = PlayerPrefs.GetInt("SelectedCharacterIndex");
        }

        ShowCurrentCharacters();
    }

    void ShowCurrentCharacters()
    {
        foreach (GameObject character in allCharacters)
        {
            character.SetActive(false);
        }

        if (currentIndex >= 0 && currentIndex < allCharacters.Length)
        {
            allCharacters[currentIndex].SetActive(true);

            // In SelectCharacter scene, disable FightingController so preview character does not react to inputs
            // In battle scenes (Map1, Map2, Map3), ensure FightingController is enabled
            bool isSelectionScene = SceneManager.GetActiveScene().name == "SelectCharacter";
            FightingController fc = allCharacters[currentIndex].GetComponent<FightingController>();
            if (fc != null)
            {
                fc.enabled = !isSelectionScene;
            }
        }
    }

    public void NextCharacter()
    {
        currentIndex = (currentIndex - 1 + allCharacters.Length) % allCharacters.Length;

        ShowCurrentCharacters();
    }   

    public void PreviousCharacter()
    {
        currentIndex = (currentIndex + 1) % allCharacters.Length;
        ShowCurrentCharacters();
    }

    public void OnYesButtonClick(string sceneName)
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneName);
    }



}
