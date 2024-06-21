using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceRecordAcivation : MonoBehaviour
{
    public GameObject RecordBouton; 

    private bool _isRecord = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isRecord = !_isRecord;

            if (_isRecord)
            {
                // Active la fonction du bouton
                RecordBouton.GetComponent<Button>().interactable = true;
            }
            else
            {
                // Désactive la fonction du bouton
                RecordBouton.GetComponent<Button>().interactable = false;
            }
        }
    }
}
