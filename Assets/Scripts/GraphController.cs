using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphController : List
{
    public GameObject nodePrefab;
    public TextAsset nodePositionTxt;
    public string[] arrayNodePosition;
    public string[] currentNodePostion;
    public DoubleLinkedList<GameObject> allNodes;


    public TextAsset nodeConectionsTxt;
    public string[] arrayNodeConection;
    public string[] currentNodeConection;

    public EnemyController enemy;
    void Start()
    {
        CreateNodes();
        CreateConections();
        SelecInitialNode();
    }
    void CreateNodes()
    {
        allNodes = new DoubleLinkedList<GameObject>();
        if (nodePositionTxt != null)
        {
            arrayNodePosition = nodePositionTxt.text.Split('\n');
            for (int i = 0; i < arrayNodePosition.Length; i++)
            {
                currentNodePostion = arrayNodePosition[i].Split(',');
                Vector2 position = new Vector2(float.Parse(currentNodePostion[0]), float.Parse(currentNodePostion[1]));
                GameObject tmp = Instantiate(nodePrefab, position, transform.rotation);
                allNodes.InsertAtEnd(tmp);
            }
        }
        allNodes.PrintAllNodes();
    }

    void CreateConections()
    {
        if (nodeConectionsTxt != null)
        {
            arrayNodeConection = nodeConectionsTxt.text.Split('\n');
            for (int i = 0; i < arrayNodeConection.Length; ++i)
            {
                currentNodeConection = arrayNodeConection[i].Split(",");
                for (int j = 0; j < currentNodeConection.Length; ++j)
                {
                    allNodes.GetElementAt(i).GetComponent<NodeController>().AddAdjacentNode(allNodes.GetElementAt(int.Parse(currentNodeConection[j])).GetComponent<NodeController>());
                    Debug.Log(allNodes.GetElementAt(i));
                }
            }
        }
    }
    void SelecInitialNode()
    {
        int index = Random.Range(0, allNodes.Count);
        enemy.objetivo = allNodes.GetElementAt(index);
    }
}