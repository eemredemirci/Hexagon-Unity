using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    public GameObject columnObj;
    GameObject hexObj;
    
    
    List<GameObject> hexagon = new List<GameObject>();
    int droptick = 1;
    int hexCount;

    void Awake()
    {
        columnObj = gameObject;
        //print(GameManager.gameManager.startDone);
    }

    void Start()
    {
        hexCount = GameManager.gameManager.gameLenght;

    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.gameManager.startDone)
        //{
        //    if (hexagon.Count != hexCount)
        //    {
        //        //get first hexagons to list
        //        GetHexagons();
        //    }
        //    else
        //    {
        //        //drop and ins
        //    }
        //}

        //print("start done");
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    print(columnObj);
        //    Debug.Log(hexagon.Count);
        //    foreach (GameObject go in hexagon)
        //    {
        //        Debug.Log(go);
        //    }
        //}
    }

    private void GetHexagons()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            hexagon.Add(child.gameObject);
        }
    }

    void CreateHexagon(int howmany)
    {
        //for(int i=)
        hexObj = Instantiate(GameManager.gameManager.hexModel, this.transform.position, Quaternion.identity);
        hexObj.name = GameManager.gameManager.hexagonName + "9";
        GameManager.gameManager.pickColors();
        var hexColor = hexObj.GetComponent<SpriteRenderer>();
        hexColor.color = GameManager.gameManager.randomColor;
        hexObj.transform.parent = this.transform;
    }

    private IEnumerator DropHexagons()
    {
        //If column child hexes destroyed drop down

        for (int i = 1; i <= GameManager.gameManager.gameWidth; i++)
        {
            transform.GetChild(i);
            
            if (gameObject.transform.Find("hexagon" + i.ToString()).gameObject == null)
            {
                //CreateHexagon(1);
                //disable collider all top of its hexagon
                i++;
                for (int j = i; j <= columnObj.transform.childCount; j++)
                {
                    //print(k);  
                    hexObj = gameObject.transform.Find("hexagon" + j.ToString()).gameObject;
                    //print(hexagon);
                    hexObj.GetComponent<PolygonCollider2D>().enabled = false;
                }

                //wait seconds until drop
                yield return new WaitForSeconds(0.3f);

                //Take the place of destroyed hexagon
                //enable collider 
                //Give the name of the destroyed hexagon
                for (int j = i; i <= GameManager.gameManager.gameLenght - droptick; i++)
                {
                    //print(k);
                    hexObj = gameObject.transform.Find("hexagon" + j.ToString()).gameObject;
                    //print(hexagon);
                    hexObj.GetComponent<PolygonCollider2D>().enabled = true;
                    hexObj.name = GameManager.gameManager.hexagonName + (j - 1).ToString();

                }
            }
        }
    }

    //GameObject GetChildWithName(GameObject obj, string name)
    //{
    //    Transform trans = obj.transform;
    //    Transform childTrans = trans.Find(name);
    //    if (childTrans != null)
    //    {
    //        return childTrans.gameObject;
    //    }
    //    else
    //    {
    //        //print("null");
    //        return null;
    //    }
    //}
}
