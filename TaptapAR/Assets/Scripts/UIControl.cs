using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [Header("Panel")]
    [Space]
    public GameObject startPanel = null;
    public GameObject scanPanel = null;
    public GameObject learningPanel = null;
    public GameObject summaryPanel = null;
    public GameObject introducePanel = null;

    [Header("Button")]
    [Space]
    public Button animalBtn = null;
    public Button foodBtn = null;
    public Button backHomeBtn = null;
    public Button learningPanel_BackBtn = null;
    public Button howtoplayBtn = null;
    public Button howtoplayPanel_BackBtn = null;
    [Header("Text")]
    [Space]
    public Text tipText = null;

    public static UIControl instance = null;

    private void Awake()
    {
        instance = this;
        ShowPanel("Start");
      
    }

    private void Start()
    {
        animalBtn.onClick.AddListener(() =>
        {
            ModelControl.instance.animalLearning = true;
            ModelControl.instance.enabledAR = true;
            ShowPanel("Scanning");
            AudioManager.instance.PlaySound(AudioManager.BUTTON_CLICK);
        });
        foodBtn.onClick.AddListener(() =>
        {
            ModelControl.instance.animalLearning = false;
            ModelControl.instance.enabledAR = true;
            ShowPanel("Scanning");
            AudioManager.instance.PlaySound(AudioManager.BUTTON_CLICK);

        });
        backHomeBtn.onClick.AddListener(() =>
        {
            ShowPanel("Start");
            AudioManager.instance.PlaySound(AudioManager.BUTTON_CLICK);
        });
        learningPanel_BackBtn.onClick.AddListener(() =>
        {
            ShowPanel("Start");
            ModelControl.instance.Close();
            AudioManager.instance.PlaySound(AudioManager.BUTTON_CLICK);
        });
        howtoplayBtn.onClick.AddListener(() =>
        {
            ShowPanel("Introduce");
            AudioManager.instance.PlaySound(AudioManager.BUTTON_CLICK);
        });
        howtoplayPanel_BackBtn.onClick.AddListener(() =>
        {
            ShowPanel("Start");
            AudioManager.instance.PlaySound(AudioManager.BUTTON_CLICK);
        });
    }

    public void HideAll()
    {
        startPanel.SetActive(false);
        scanPanel.SetActive(false);
        learningPanel.SetActive(false);
        summaryPanel.SetActive(false);
        introducePanel.SetActive(false);
    }


    public void ShowPanel(string name)
    {
        HideAll();
        switch (name)
        {
            case "Start":
                startPanel.SetActive(true);
                break;
            case "Scanning":
                scanPanel.SetActive(true);
                break;
            case "Learning":
                learningPanel.SetActive(true);
                break;
            case "Summary":
                summaryPanel.SetActive(true);
                break;
            case "Introduce":
                introducePanel.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ShowTip(string message)
    {
        StopAllCoroutines();
        tipText.text = message;
        tipText.DOFade(1, 0);
    }

    public void ShowTip(string message,float time,Action callback)
    {
        StopAllCoroutines();
        tipText.text = message;
        tipText.DOFade(1, 0);
        StartCoroutine(DelayCoroutine(time, callback));
    }

    private IEnumerator DelayCoroutine(float time,Action callback)
    {
        yield return new WaitForSeconds(time);
        tipText.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        callback?.Invoke();
    }
}
