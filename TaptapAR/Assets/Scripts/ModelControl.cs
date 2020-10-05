using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelControl : DefaultTrackableEventHandler
{
    public bool enabledAR = false;


    public Transform foodParent = null;
    public Transform animalParent = null;
    public Transform pointsParent = null;


    private List<GameObject> foodsList = null;
    private List<Transform> pointsList = null;
    private List<GameObject> animalsList = null;

    private GameObject[] foodsArray = null;
    private GameObject[] animalsArray = null;
    private Transform[] pointsArray = null;

    private List<GameObject> waitObjects = null;

    private GameObject curTarget = null;
    public bool animalLearning = false;

    public static ModelControl instance = null;
    private RaycastHit hit;
    private bool learning = false;
    private bool finding = false;

    private const string tip1 = "Please find the ";
    private const string tip2 = "Wrong! Try again!";
    private const string tip3 = "Good Job! ";

    private void Awake()
    {
        instance = this;
        foodsArray = new GameObject[foodParent.childCount];
        for (int i = 0; i < foodsArray.Length; i++)
        {
            foodsArray[i] = foodParent.GetChild(i).gameObject;
        }

        animalsArray = new GameObject[animalParent.childCount];
        for (int i = 0; i < animalsArray.Length; i++)
        {
            animalsArray[i] = animalParent.GetChild(i).gameObject;
        }

        pointsArray = new Transform[pointsParent.childCount];
        for (int i = 0; i < pointsArray.Length; i++)
        {
            pointsArray[i] = pointsParent.GetChild(i);
        }

        foodsList = new List<GameObject>();
        animalsList = new List<GameObject>();
        pointsList = new List<Transform>();
        waitObjects = new List<GameObject>();
    }

    public bool isFound = false;
    protected override void OnTrackingFound()
    {
        if (!enabledAR) return;
        base.OnTrackingFound();
        isFound = true;
        UIControl.instance.ShowPanel("Learning");
        if (!learning)
        {
            learning = true;
        
            waitObjects.Clear();
            if (animalLearning)
            {
                GenerateAnimalOnRandomPoints();
                waitObjects.AddRange(animalsArray);
                RandomTargetObject();
            }
            else
            {
                GenerateFoodOnRandomPoints();
                waitObjects.AddRange(foodsArray);
                RandomTargetObject();
            }
            finding = true;
            UIControl.instance.ShowTip(tip1 + curTarget.gameObject.name + ".");
        }
    }

    protected override void OnTrackingLost()
    {
        if (!enabledAR) return;
        base.OnTrackingLost();
        isFound = false;
        UIControl.instance.ShowPanel("Scanning");
    }

    private void RandomTargetObject()
    {
        int rand = Random.Range(0, waitObjects.Count);
        curTarget = waitObjects[rand];
        waitObjects.RemoveAt(rand);
    }


    public void GenerateFoodOnRandomPoints()
    {
        foodsList.Clear();
        foodsList.AddRange(foodsArray);
        pointsList.Clear();
        pointsList.AddRange(pointsArray);
        while (foodsList.Count > 0)
        {
            GameObject food = foodsList[0];
            int randIndex = Random.Range(0, pointsList.Count);
            food.transform.position = pointsList[randIndex].position;
            food.SetActive(true);
            foodsList.RemoveAt(0);
            pointsList.RemoveAt(randIndex);
        }
    }

    public void GenerateAnimalOnRandomPoints()
    {
        //Debug.Log("animalsArray is null = " + (animalsArray == null));
        animalsList.Clear();
        animalsList.AddRange(animalsArray);
        pointsList.Clear();
        pointsList.AddRange(pointsArray);
        while (animalsList.Count > 0)
        {
            GameObject animal = animalsList[0];
            int randIndex = Random.Range(0, pointsList.Count);
            animal.transform.position = pointsList[randIndex].position;
            animal.SetActive(true);
            animalsList.RemoveAt(0);
            pointsList.RemoveAt(randIndex);
        }
    }



    private void Update()
    {
        if (!isFound || !finding) return;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            bool isHit = Physics.Raycast(ray, out hit, 100);
            if (isHit && hit.collider.tag == "Obj")
            {
                if (hit.collider.gameObject != curTarget)
                {
                    AudioManager.instance.PlaySound(AudioManager.WRONG);
                    UIControl.instance.ShowTip(tip2, 2, () =>
                    {
                        UIControl.instance.ShowTip(tip1 + curTarget.gameObject.name + ".");
                    });
                }
                else
                {
                    AudioManager.instance.PlaySound(AudioManager.RIGHT);
                    finding = false;
                    UIControl.instance.ShowTip(tip3, 2, () =>
                    {
                        if(waitObjects.Count > 0)
                        {
                            RandomTargetObject();
                            finding = true;
                            UIControl.instance.ShowTip(tip1 + curTarget.gameObject.name + ".");
                        }
                        else
                        {
                            Close();
                            UIControl.instance.ShowPanel("Summary");
                        }
          
                    });
                }
            }
        }
    }

    public void Close()
    {
        enabledAR = false;
        learning = false;
        for (int i = 0; i < foodsArray.Length; i++)
        {
            foodsArray[i].SetActive(false);
        }
        for (int i = 0; i < animalsArray.Length; i++)
        {
            animalsArray[i].SetActive(false);
        }
    }

}
