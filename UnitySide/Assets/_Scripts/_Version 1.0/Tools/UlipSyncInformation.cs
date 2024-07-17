using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class uLipSyncInformationUI : MonoBehaviour
{
    [Header("Messages")]
    public TMP_Text infoText;
    public TMP_Text warningText;
    public TMP_Text errorText;
    public TMP_Text successText;

    public void ClearTexts()
    {
        infoText.GetComponent<TMP_Text>().text = "";
        warningText.GetComponent<TMP_Text>().text = "";
        errorText.GetComponent<TMP_Text>().text = "";
        successText.GetComponent<TMP_Text>().text = "";
    }

    public void ClearTextsAfter(float time = 3f)
    {
        StartCoroutine(_ClearTextsAfter(time));
    }

    IEnumerator _ClearTextsAfter(float time)
    {
        yield return new WaitForSeconds(time);
        ClearTexts();
    }

    public void Info(string msg)
    {
        ClearTexts();
        infoText.GetComponent<TMP_Text>().text = msg;
        ClearTextsAfter();
    }

    public void Warn(string msg)
    {
        ClearTexts();
        warningText.GetComponent<TMP_Text>().text = msg;
        ClearTextsAfter();
    }

    public void Error(string msg)
    {
        ClearTexts();
        errorText.GetComponent<TMP_Text>().text = msg;
        ClearTextsAfter();
    }

    public void Success(string msg)
    {
        ClearTexts();
        successText.GetComponent<TMP_Text>().text = msg;
        ClearTextsAfter();
    }
}