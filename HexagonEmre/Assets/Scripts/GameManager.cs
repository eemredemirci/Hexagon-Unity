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
    private Vector2 touchStartCoordinates;
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
    List<Color> allHexagons = new List<Color>();

    public GameObject tempObj;
    public GameObject root;


    bool isSelected = false;
    private Vector2[] rayHelper = new Vector2[6];
    private Vector2 rayPos = new Vector2();
    GameObject[] selectedObj = new GameObject[3];

    void Awake()
    {
        if (!gameManager)
        {
            gameManager = this;
        }
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

        if (InputUtils.MouseDownOrTap())
        {
            Vector2 pos = InputUtils.MouseOrTapPosition();
            Vector3 worldPos = InputUtils.InputToWorldPosition(pos);
            ClickSelect(worldPos);
            //Instantiate(tapEffect, worldPos, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }

        //Touch Phase debug
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            phaseDisplayText.text = theTouch.phase.ToString();
            if (theTouch.phase == TouchPhase.Ended)
            {
                timeTouchEnded = Time.time;
            }
        }
        else if (Time.time - timeTouchEnded > displayTime)
        {
            phaseDisplayText.text = "";
        }

    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ClickSelect(Vector3 worldPos)
    {
        //Converting Mouse Pos to 2D (vector2) World Pos
        if (isSelected)
        {
            foreach (var selected in selectedObj)
            {
                selected.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        rayPos = worldPos;
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

        // vertex to vertex lenght 1.0529
        // side lenght = 0.486782
        //apotem (between mid point of hex to side mid) = 0.4216
        rayHelper[0] = new Vector2(rayPos.x - 0.281f, rayPos.y - 0.162f);
        rayHelper[1] = new Vector2(rayPos.x - 0.281f, rayPos.y + 0.162f);
        rayHelper[2] = new Vector2(rayPos.x, rayPos.y + 0.325f);
        rayHelper[3] = new Vector2(rayPos.x + 0.281f, rayPos.y + 0.162f);
        rayHelper[4] = new Vector2(rayPos.x + 0.281f, rayPos.y - 0.162f);
        rayHelper[5] = new Vector2(rayPos.x, rayPos.y - 0.325f);

        GameObject[] hitHelper = new GameObject[6];
        if (hit.transform != null)
        {
            //Get First Hex
            selectedObj[0] = hit.collider.gameObject;
            //diff between mouse and hex position. 
            //To calc how click far from hex mid point to detect selection

            //print((rayPos.x - selectedObj[0].transform.position.x).ToString() + " coordinate diff " + (rayPos.y - selectedObj[0].transform.position.y).ToString());

            //get raycasts' hits
            for (int i = 0; i <= 5; i++)
            {
                RaycastHit2D obj = Physics2D.Raycast(rayHelper[i], Vector2.zero, 0f);
                if (obj.transform != null)
                {
                    hitHelper[i] = obj.transform.gameObject;
                    //Debug.LogWarning("hit Helper Selection  " + i + "   " + hitHelper[i] + " " + hitHelper[i].transform.parent);
                }
            }

            //Get 2nd and 3rd hexagons from star raycast
            if (hitHelper[3] == selectedObj[0] && hitHelper[4] == selectedObj[0])
            {
                selectedObj[1] = hitHelper[0].transform.gameObject;
                selectedObj[2] = hitHelper[1].transform.gameObject;
            }
            else if (hitHelper[4] == selectedObj[0] && hitHelper[5] == selectedObj[0])
            {
                selectedObj[1] = hitHelper[1].transform.gameObject;
                selectedObj[2] = hitHelper[2].transform.gameObject;
            }
            else if (hitHelper[5] == selectedObj[0] && hitHelper[0] == selectedObj[0])
            {
                selectedObj[1] = hitHelper[2].transform.gameObject;
                selectedObj[2] = hitHelper[3].transform.gameObject;
            }
            else if (hitHelper[0] == selectedObj[0] && hitHelper[1] == selectedObj[0])
            {
                selectedObj[1] = hitHelper[3].transform.gameObject;
                selectedObj[2] = hitHelper[4].transform.gameObject;
            }
            else if (hitHelper[1] == selectedObj[0] && hitHelper[2] == selectedObj[0])
            {
                selectedObj[1] = hitHelper[4].transform.gameObject;
                selectedObj[2] = hitHelper[5].transform.gameObject;
            }
            else if (hitHelper[2] == selectedObj[0] && hitHelper[3] == selectedObj[0])
            {
                selectedObj[1] = hitHelper[5].transform.gameObject;
                selectedObj[2] = hitHelper[0].transform.gameObject;
            }
            OutlineHighlighter();
        }

    }

    void OutlineHighlighter()
    {
        foreach (var selected in selectedObj)
        {
            print(selected.name);
            selected.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }
        isSelected = true;
        StartCoroutine(Locationchanger(true));
    }

    void CreateColumn()
    {
        //create column Parent gameobjects
        for (int i = 0; i <= gameWidth - 1; i++)
        {
            Vector2 pos = new Vector2(-1.92f + (i * 0.56f), 2.4f);
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
                    Vector2 position = new Vector2(-1.36f + i, 2.4f);
                    tempObj = Instantiate(hexModel, position, Quaternion.identity);
                    tempObj.name = hexagonName + HexnameCounter;
                    pickColors();
                    tempObj.GetComponent<SpriteRenderer>().color = randomColor;
                    tempObj.transform.parent = GameObject.Find("Column" + ColnameCounter.ToString()).transform;
                    ColnameCounter += 2;

                    yield return new WaitForSeconds(0.1f);
                }
            }
            else if (k % 2 == 0)
            {
                int ColnameCounter = 1;
                for (float j = 0; j <= 4f; j += 1.12f)
                {
                    Vector2 position = new Vector2(-1.92f + j, 2.4f);
                    tempObj = Instantiate(hexModel, position, Quaternion.identity);
                    tempObj.name = hexagonName + HexnameCounter;
                    pickColors();
                    tempObj.GetComponent<SpriteRenderer>().color = randomColor;
                    tempObj.transform.parent = GameObject.Find("Column" + ColnameCounter.ToString()).transform;
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
        randomColor = colorList[Random.Range(0, colorList.Count)];
        //Debug.Log(randomColor.ToString());
    }


    IEnumerator Locationchanger(bool CW)
    {
        Vector2 pos0, pos1, pos2;

        foreach (var selected in selectedObj)
        {
            selected.transform.GetComponent<PolygonCollider2D>().enabled = false;
            selected.transform.GetComponent<Rigidbody2D>().simulated = false;

        }

        pos0 = selectedObj[0].transform.position;
        pos1 = selectedObj[1].transform.position;
        pos2 = selectedObj[2].transform.position;
        if (CW)
        {
            selectedObj[0].transform.position = Vector2.Lerp(pos0, pos1, Time.time);
            selectedObj[1].transform.position = Vector2.Lerp(pos1, pos2, Time.time);
            selectedObj[2].transform.position = Vector2.Lerp(pos2, pos0, Time.time);
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            selectedObj[2].transform.position = Vector2.Lerp(pos2, pos1, Time.time);
            selectedObj[1].transform.position = Vector2.Lerp(pos1, pos0, Time.time);
            selectedObj[0].transform.position = Vector2.Lerp(pos0, pos2, Time.time);
            yield return new WaitForSeconds(0.3f);
        }
        foreach (var selected in selectedObj)
        {
            selected.transform.GetComponent<PolygonCollider2D>().enabled = true;
            selected.transform.GetComponent<Rigidbody2D>().simulated = true;
        }
    }

    //void DragDetect(int platform)
    //{

    //    Vector2 afterTouchCoordinates = Input.GetTouch(0).position;

    //    if (platform == 1)
    //    {

    //    }
    //    float distX = afterTouchCoordinates.x - touchStartCoordinates.x;
    //    float distY = afterTouchCoordinates.y - touchStartCoordinates.y;

    //    //dokunma başlangıç ve bitiş koordinatları üzerinden rotasyon tetiklenmesini kontrol eder.
    //    if (distX > 0 || distY > 0)
    //    {
    //        Locationchanger(true);

    //    }
    //    else
    //    {
    //        Locationchanger(false);
    //    }
    //    theTouch = Input.GetTouch(0);
    //    //phaseDisplayText.text = distX.ToString() + distY.ToString();

    //}

}
