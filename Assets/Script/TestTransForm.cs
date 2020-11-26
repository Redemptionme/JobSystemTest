using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

[BurstCompile]
public struct MoveOneWorkJob : IJobParallelForTransform       //线程接口，执行execute方法
{
   

    public void Execute(int index, TransformAccess transform)
    {
        Vector3 pos = transform.localPosition; 

        transform.localPosition = pos + Vector3.one + Vector3.forward * index + Vector3.back/3*index;
    }
}



public class TestTransForm : MonoBehaviour
{
    public GameObject obj;
    public int num = 100;
    private JobHandle handle;
    private TransformAccessArray transformsAccessArray;

    private List<Transform> list = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < num; i++)
        {
            list.Add(Instantiate(obj).transform);

            if ( i > num/2)
            {
                list[i].SetParent(list[i-num/2].transform);
            }
            else
            {
                list[i].SetParent(this.transform);    
            }
        }
        transformsAccessArray = new TransformAccessArray(list.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var starTime = Time.realtimeSinceStartup;
            for(int i = 0;i < list.Count; i++)
            {
                list[0].localPosition += (Vector3.one + Vector3.forward * 3 + Vector3.back/3);
            }

            var endTime = Time.realtimeSinceStartup;
            
            Debug.Log("1cost " + (endTime-starTime));
        }

        if (Input.GetMouseButtonDown(1))
        {
            var starTime = Time.realtimeSinceStartup;
            
            MoveOneWorkJob job = new MoveOneWorkJob();
            handle = job.Schedule(transformsAccessArray);
            
            handle.Complete();
           
            var endTime = Time.realtimeSinceStartup;
            Debug.Log("2cost " + (endTime-starTime));
            
        }
    }

    private void LateUpdate()
    {
        
    }

    private void OnDestroy()
    {
        transformsAccessArray.Dispose();
    }
}
