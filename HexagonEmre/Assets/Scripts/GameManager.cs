using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    //----------------touch test
    public Text phaseDisplayText;
    private Touch theTouch;
    private float timeTouchEnded;
    private float displayTime = .5f;
    //---------------------

    private IEnumerator coroutine;
    public bool startDone;

    [Header("Column Points")]

    public int gameWidth = 8;
    public int gameLenght = 9;
    public string hexagonName = "hexagon";

    [Header("Hexagons Colors")]
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
    public Color color5;
    public Color color6;
    public Color color7;
    public Color color8;
    public Color color9;
    List<Color> colorList = new List<Color>();
    public Color randomColor;

    [Header("Hexagon Model")]
    public GameObject hexModel;
    int HexnameCounter = 1;

    public GameObject tempObj;
    public GameObject root;

    Collider2D[] hitTRiple = new Collider2D[2];
    bool isSelected = false;

    void Awake()
    {

        startDone = false;
        //Add the default color to list
        //note: we can use while case
        colorList.Add(color1);
        colorList.Add(color2);
        colorList.Add(color3);
        colorList.Add(color4);
        colorList.Add(color5);
    }

    void Start()
    {
        if (!gameManager)
        {
            gameManager = this;
        }
        startDone = false;
        Application.targetFrameRate = 60;
        // Start function create column as a coroutine
        CreateColumn();
        StartCoroutine(CreateStartSceneHexagon());
    }

    void Update()
    {
        if (startDone == true)
        {
            //StartCoroutine(DropHexagons());

        }

        if (Input.GetMouseButtonDown(0))
        {
            ClickSelect();
        }

        //Touch select

        //if (Input.touchCount > 0)
        //{
        //    theTouch = Input.GetTouch(0);
        //    phaseDisplayText.text = theTouch.phase.ToString();

        //    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //    Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 5f);
        //    if (Physics.Raycast(ray, out RaycastHit hit))
        //    {
        //        Debug.Log(hit.transform.name);
        //        if (hit.collider != null)
        //        {

        //            GameObject touchedObject = hit.transform.gameObject;

        //            phaseDisplayText.text = "Touched " + touchedObject.transform.name;
        //        }
        //    }

        //    if (theTouch.phase == TouchPhase.Ended)
        //    {
        //        timeTouchEnded = Time.time;
        //    }
        //}

        //else if (Time.time - timeTouchEnded > displayTime)
        //{
        //    phaseDisplayText.text = "";
        //}
    }

    void ClickSelect()
    {
        //Converting Mouse Pos to 2D (vector2) World Pos
        Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
        if (hit)
        {
            Debug.Log(hit.transform.name);
            //Debug.Log(hit.transform.position);
            SelectWithinRadius(rayPos, 0.3f);

            //return hit.transform.gameObject;
        }
        else
        {
            //return null
        }
    }

    void SelectWithinRadius(Vector3 center, float radius)
    {
        if (isSelected)
        {
            foreach (var hitCollider in hitTRiple)
            {
                //print(hitCollider.transform.parent.name + hitCollider.name);

                hitCollider.GetComponent<Shapes2D.Shape>().settings.outlineSize = 0;
            }
        }

        hitTRiple = Physics2D.OverlapCircleAll(center, radius);

        foreach (var hitCollider in hitTRiple)
        {
            //print(hitCollider.transform.parent.name + hitCollider.name);
            hitCollider.GetComponent<Shapes2D.Shape>().settings.outlineSize = 0.03f;
        }
        isSelected = true;
    }


    void CreateColumn()
    {
        //create column Parent gameobjects
        for (int i = 0; i <= gameWidth - 1; i++)
        {

            Vector2 pos = new Vector2(-1.92f + (i * 0.56f), 2f);
            tempObj = Instantiate(root, pos, Quaternion.identity);
            tempObj.name = "Column" + (i + 1).ToString();
            tempObj.transform.position = pos;
            tempObj.AddComponent<Column>();
            var script = tempObj.GetComponent<Column>();
            script.enabled = true;
        }

    }

    IEnumerator CreateStartSceneHexagon()
    {
        //Loop creates first hexagons
        for (int k = 1; k < 19; k++)
        {

            //hexagons are created 4 by 4.
            if (k % 2 == 1)
            {
                int ColnameCounter = 2;

                for (float i = 0; i <= 4f; i += 1.12f)
                {

                    Vector2 position = new Vector2(-1.36f + i, 2);
                    hexModel = Instantiate(hexModel, position, Quaternion.identity);
                    hexModel.name = hexagonName + HexnameCounter;
                    pickColors();
                    hexModel.GetComponent<Shapes2D.Shape>().settings.fillColor = randomColor;
                    hexModel.transform.parent = GameObject.Find("Column" + ColnameCounter.ToString()).transform;
                    ColnameCounter += 2;

                    yield return new WaitForSeconds(0.1f);
                }
            }
            else if (k % 2 == 0)
            {
                int ColnameCounter = 1;
                for (float j = 0; j <= 4f; j += 1.12f)
                {

                    Vector2 position = new Vector2(-1.92f + j, 2);
                    hexModel = Instantiate(hexModel, position, Quaternion.identity);
                    hexModel.name = hexagonName + HexnameCounter;
                    pickColors();
                    hexModel.GetComponent<Shapes2D.Shape>().settings.fillColor = randomColor;
                    hexModel.transform.parent = GameObject.Find("Column" + ColnameCounter.ToString()).transform;
                    ColnameCounter += 2;

                    yield return new WaitForSeconds(0.1f);

                }
                HexnameCounter++;
            }

        }
        startDone = true;
    }

    public void pickColors()
    {
        //collider.gameObject.GetComponent<Renderer>().material.color = colorList[Random.Range(0, colorList.Count - 1)];
        randomColor = colorList[Random.Range(0, colorList.Count - 1)];
        randomColor.a = 1f;
        //Debug.Log(randomColor.ToString());
    }

}
