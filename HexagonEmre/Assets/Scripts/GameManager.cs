using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IEnumerator coroutine;

    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
    public Color color5;
    public Color color6;
    public Color color7;
    public Color color8;
    public Color color9;
    public GameObject gm;
    private Color randomColor;

    void Awake()
    {

    }

    IEnumerator GameStart()
    {
        for (int k = 1; k < 19; k++)
        {
            if (k % 2 == 1)
            {
                for (float i = 0; i <= 4f; i += 1.12f)
                {
                    Vector2 position = new Vector2(-1.36f + i, 2);
                    Instantiate(gm, position, Quaternion.identity);

                    //var hexColor = gm.GetComponent<SpriteRenderer>();
                    //hexColor.color = randomColor;
                    yield return new WaitForSeconds(0.3f);
                }
            }
            else
            {
                for (float i = 0; i <= 4f; i += 1.12f)
                {
                    Vector2 position = new Vector2(-1.92f + i, 2);
                    Instantiate(gm, position, Quaternion.identity);

                    //var hexColor = gm.GetComponent<SpriteRenderer>();
                    //hexColor.color = randomColor;
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
