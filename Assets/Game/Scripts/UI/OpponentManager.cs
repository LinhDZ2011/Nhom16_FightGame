using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class OpponentManager : MonoBehaviour 
{
    public GameObject[] opponentsCharacters;

    private void Start()
    {
        if(opponentsCharacters.Length == 0)
        {
            Debug.LogError("No opponent assigned in the OpponentManager.");
            return;
        }

        ActivateRandomOpponent();
    }
    void ActivateRandomOpponent()
    {
        int randomIndex = Random.Range(0, opponentsCharacters.Length);

        for(int i = 0; i < opponentsCharacters.Length; i++)
        {
            if (i == randomIndex)
                opponentsCharacters[i].SetActive(true);
            
            else
                opponentsCharacters[i].SetActive(false);
        }
    }

}
