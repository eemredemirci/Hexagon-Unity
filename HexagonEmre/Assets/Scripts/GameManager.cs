using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IEnumerator coroutine;
    bool startDone = false;
    [Header("Column Points")]

    public Transform ColumnTopPoint;
    public Transform ColumnBotPoint;
    public int gameWidth = 8;
    public int gameLenght = 9;
    private Vector3 direction;
    string hexagonName = "hexagon";
    int HexnameCounter = 1;



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

    [Header("Hexagon Model")]
    public GameObject gm;
    private Color randomColor;
    List<Color> colorList = new List<Color>();
    List<GameObject> columnList = new List<GameObject>();


    void Awake()
    {
        //Add the color to list
        colorList.Add(color1);
        colorList.Add(color2);
        colorList.Add(color3);
        colorList.Add(color4);
        colorList.Add(color5);
    }

    void FixedUpdate()
    {
        if (startDone == true)
        {
            //Vector3 fromPosition = ColumnTopPoint.transform.position;
            ////print(fromPosition);

            //Vector3 toPosition = ColumnBotPoint.transform.position;
            ////print(toPosition);

            //direction = toPosition - fromPosition;
            //RaycastHit hit;

            //for (float a = 0; a <= 4.5f; a += 0.56f)
            //{
            //    Vector3 setX = new Vector3(0f + a, 0, 0);
            //    Debug.DrawRay(fromPosition + setX, direction, Color.white);

            //    //if (Physics2D.Linecast(fromPosition + setX, direction,out hit))
            //    //{
            //    //    print(hit.collider.transform);
            //    //}
            //}
        }

    }

    void Update()
    {
        if (startDone == true)
        {
            /*
            //check Is column exist
            for (int i = 1; i <= gameWidth; i++)
            {
                if (GameObject.Find("Column" + i.ToString()) != null)
                {
                    //check Is Column hexagons objects
                    for (int j = 1; i <= gameLenght; i++)
                    {
                        //if the bottom of a hexagon is empty 
                        if (GameObject.Find("hexagon" + i.ToString()) == null)
                        {
                        //hexagonJ doesnt exist do close the companents of 
                        //those who come after

                        }
                    }
                }
            }
            */
        }
    }

    private void pickColors()
    {
        //collider.gameObject.GetComponent<Renderer>().material.color = colorList[Random.Range(0, colorList.Count - 1)];
        randomColor = colorList[Random.Range(0, colorList.Count - 1)];
        randomColor.a = 1f;
        //Debug.Log(randomColor.ToString());
        Debug.ClearDeveloperConsole();
    }

    IEnumerator GameStart()
    {
        //columnRoot object represent game with and its hexagon on same column

        for (int i = 0; i <= gameWidth - 1; i++)
        {
            GameObject root = new GameObject();
            root.name = "Column" + (i + 1).ToString();
            root.transform.position = new Vector2(-1.92f + (i * 0.56f), 2f);
        }
        //Loop creates first hexagons
        for (int k = 1; k < 19; k++)
        {
            Debug.Log("K=" + k.ToString());


            //hexagons are created 4 by 4.
            if (k % 2 == 1)
            {
                int ColnameCounter = 2;

                for (float i = 0; i <= 4f; i += 1.12f)
                {
                    Debug.Log("iiiii=");
                    Vector2 position = new Vector2(-1.36f + i, 2);
                    gm = Instantiate(gm, position, Quaternion.identity);
                    gm.name = hexagonName + HexnameCounter;
                    pickColors();
                    var hexColor = gm.GetComponent<SpriteRenderer>();
                    hexColor.color = randomColor;
                    gm.transform.parent = GameObject.Find("Column" + ColnameCounter.ToString()).transform;
                    ColnameCounter += 2;

                    yield return new WaitForSeconds(0.1f);
                }
            }
            else if (k % 2 == 0)
            {
                int ColnameCounter = 1;
                for (float j = 0; j <= 4f; j += 1.12f)
                {
                    Debug.Log("jjjjjj=");
                    Vector2 position = new Vector2(-1.92f + j, 2);
                    gm = Instantiate(gm, position, Quaternion.identity);
                    gm.name = hexagonName + HexnameCounter;
                    pickColors();
                    var hexColor = gm.GetComponent<SpriteRenderer>();
                    hexColor.color = randomColor;
                    gm.transform.parent = GameObject.Find("Column" + ColnameCounter.ToString()).transform;
                    ColnameCounter += 2;

                    yield return new WaitForSeconds(0.1f);
                    //After instantiate 8 hex +1 nameCounter
                }
                HexnameCounter++;
            }

        }
        startDone = true;
    }

    IEnumerator Start()
    {
        // Start function GameStart as a coroutine
        yield return StartCoroutine("GameStart");
    }

    void GetHexagonsbyColumn()
    {
        //columnList.Add(GameObject.Find("hexagon1"));
    }
}
