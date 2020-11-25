using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.UI;

// Job adding two floating point values together

public struct MyParallelJob : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<float> a;
    [ReadOnly]
    public NativeArray<float> b;

    public NativeArray<float> result;

    public void Execute(int i)
    {
        //result[i] = Mathf.Pow(a[i],10) * Mathf.Pow(b[i],10);
        result[i] = Mathf.Sqrt(Mathf.Abs(Mathf.Sin(a[i]) * Mathf.Cos(b[i])));
    }
}

public class ParallelForJobTest : MonoBehaviour
{
    public Text text;
    public int calcSize = 1000000;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 20, 100, 40), "Parallel Calc"))
        {
            ParallelCalc();
        }

        if (GUI.Button(new Rect(10, 80, 100, 40), "Direct Calc"))
        {
            DirectCalc();
        }
    }

    private void ParallelCalc()
    {
        // record start time
        float startTime = Time.realtimeSinceStartup;
        var startTimea = DateTime.Now;
        NativeArray<float> a = new NativeArray<float>(calcSize, Allocator.TempJob);

        NativeArray<float> b = new NativeArray<float>(calcSize, Allocator.TempJob);

        NativeArray<float> result = new NativeArray<float>(calcSize, Allocator.TempJob);

        float startTime1 = Time.realtimeSinceStartup;
        var startTime1a = DateTime.Now;
        for (int i = 0; i < calcSize; i++)
        {
            a[i] = 0.005f * i;
            b[i] = 0.007f * i;
        }

        MyParallelJob jobData = new MyParallelJob();
        jobData.a = a;
        jobData.b = b;
        jobData.result = result;

        // Schedule the job with one Execute per index in the results array and only 1 item per processing batch
        JobHandle handle = jobData.Schedule(result.Length, 100);
        // Wait for the job to complete
        handle.Complete();
        // record end time
        float endTime = Time.realtimeSinceStartup;
        var endTimea = DateTime.Now;;
        //Debug.Log("TotalCount:" + result.Length);
        float temp = result[calcSize - 1];
        Debug.Log("The last Value:" + temp);
        Debug.Log("TotalCount:" + result.Length + ",Parallel Use Time:" + (endTime - startTime) + ",Init Use Time:" + (startTime1 - startTime));
        text.text += "ParallelCalc " + (endTime - startTime) + " ";
        Debug.Log("TotalCount:" + result.Length + ",Parallel Use Time:" + (endTimea - startTimea).TotalMilliseconds + ",Init Use Time:" + (startTime1a - startTimea).TotalMilliseconds);

        // Free the memory allocated by the arrays
        a.Dispose();
        b.Dispose();
        result.Dispose();
    }

    private void DirectCalc()
    {
        List<float> result = new List<float>();
        float tempA, tempB;
        float startTime = Time.realtimeSinceStartup;
        var startTimea = DateTime.Now;;
        for (int i = 0; i < calcSize; i++)
        {
            tempA = 0.005f * i;
            tempB = 0.007f * i;
            result.Add(Mathf.Sqrt(Mathf.Abs(Mathf.Sin(tempA) * Mathf.Cos(tempB))));
        }
        float endTime = Time.realtimeSinceStartup;
        var endTimea = DateTime.Now;;
        float temp = result[calcSize - 1];
        Debug.Log("The last Value:" + temp);
        Debug.Log("TotalCount:" + result.Count + ",Direct Use Time:" + (endTime - startTime));
        text.text += "DirectCalc " + (endTime - startTime) + " ";
        Debug.Log("TotalCount:" + result.Count + ",Direct Use Time:" + (endTimea - startTimea).TotalMilliseconds);
    }
}
