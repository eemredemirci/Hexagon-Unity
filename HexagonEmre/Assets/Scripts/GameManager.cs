using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IEnumerator coroutine;

    [Header("Column Points")]

    public GameObject col1top;
    public GameObject col1bot;
    public GameObject col2top, col2bot;
    public GameObject col3top, col3bot;
    public GameObject col4top, col4bot;
    public GameObject col5top, col5bot;
    public GameObject col6top, col6bot;
    public GameObject col7top, col7bot;
    public GameObject col8top, col8bot;

    //Column Vectors
    private Vector2 col1,col2,col3,col4,col5,col6,col7,col8,col9;

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

    Vector2 direction;
    void Awake()
    {
        //Add the color to list
        colorList.Add(color1);
        colorList.Add(color2);
        colorList.Add(color3);
        colorList.Add(color4);
        colorList.Add(color5);

        //draw the colun vector
        

        //Vector2 dir = (col1bot.transform.position - col1top.transform.position).normalized;
        //Debug.DrawLine(col1, col1 + dir*10, Color.red, Mathf.Infinity);

        //Debug.DrawLine(col1top,  , Color.red, Mathf.Infinity);
        //col1 = (col2top.transform.position-col1top.transform.position).normalized;
        //Debug.Log(col1.ToString());
        ////var col = col1top.transform.position - col1bot.transform.position.y;

    }

    
    void Update()
    {
        RaycastHit hit;

        Vector2 fromPosition = col1top.transform.position;
        Vector2 toPosition = col1bot.transform.position;
        direction = toPosition - fromPosition;

        if (Physics.Raycast(col1top.transform.position, direction, out hit))
        {
            print("ray just hit the gameobject: " + hit.collider.gameObject.name);
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
        //for loop creates first hexagons 
        for (int k = 1; k < 19; k++)
        {
            if (k % 2 == 1)
            {
                for (float i = 0; i <= 4f; i += 1.12f)
                {
                    Vector2 position = new Vector2(-1.36f + i, 2);

                    Instantiate(gm, position, Quaternion.identity);
                    pickColors();
                    var hexColor = gm.GetComponent<SpriteRenderer>();
                    hexColor.color = randomColor;
                    
                    yield return new WaitForSeconds(0.3f);
                }
            }
            else
            {
                for (float i = 0; i <= 4f; i += 1.12f)
                {
                    Vector2 position = new Vector2(-1.92f + i, 2);

                    Instantiate(gm, position, Quaternion.identity);
                    pickColors();
                    var hexColor = gm.GetComponent<SpriteRenderer>();
                    hexColor.color = randomColor;

                    yield return new WaitForSeconds(0.1f);
                }
            }

        }

    }

    IEnumerator Start()
    {
        // Start function GameStart as a coroutine
        yield return StartCoroutine("GameStart");
    }
}
