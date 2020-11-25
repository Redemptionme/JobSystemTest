using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMesh : MonoBehaviour
{
    public GameObject obj;

    private List<GameObject> list = new List<GameObject>();
    private List<Renderer> list2 = new List<Renderer>();

    private bool vis = true;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            list.Add(Instantiate(obj));
            list2.Add(list[i].GetComponent<Renderer>());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < list.Count; i++)
        {
            list2[i].enabled = vis;
        }
        vis = !vis;
    }
}
