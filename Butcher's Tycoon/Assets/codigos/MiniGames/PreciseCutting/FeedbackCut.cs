using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameFeedback : MonoBehaviour
{
    public void ShowPerfect()
    {
        Debug.Log("Feedback: Corte PERFECT!");
    }

    public void ShowGood()
    {
        Debug.Log("Feedback: Corte GOOD!");
    }

    public void ShowBad()
    {
        Debug.Log("Feedback: Corte BAD!");
    }
}
