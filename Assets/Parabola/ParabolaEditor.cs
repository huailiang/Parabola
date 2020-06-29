#if UNITY_EDITOR
using System;
using UnityEngine;

public class ParabolaEditor : MonoBehaviour
{
    [Header("起始点")]
    public Transform original;
    [Header("结束点")]
    public Transform destination;
    [Header("向上速度")]
    public int vy;
    [Header("轨迹点数")]
    public int count;
    [Header("运动时间")]
    public float time;

    
    private double acc;
    private Vector2 h_dir;
    private float h_speed;
    private bool play;
    private float start;
    [HideInInspector]
    public GameObject go;

    private void TodoStart()
    {
        if (go == null)
            go = GameObject.Instantiate(original.gameObject);
        go.transform.position = original.position;

        acc = RuntimeParabola(out h_dir, out h_speed);
        play = true;
        start = Time.time;
    }


    void Update()
    {
        if (play)
        {
            float t = Time.time - start;
            Vector2 st = new Vector2(original.position.x, original.position.z);
            var h1 = st + h_dir * h_speed * t;
            var v1 = original.position.y - (float)(0.5f * acc * t * t) + vy * t;
            go.transform.position = new Vector3(h1.x, v1, h1.y);
            
            if (t >= time)
            {
                play = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            TodoStart();
        }
    }


    private void OnDrawGizmos()
    {
        if (original && destination)
        {
            Gizmos.color = Color.red;
            CalParabola(out var v_s);
            int len = v_s.Length;
            for (int i = 0; i < len - 1; i++)
            {
                Gizmos.DrawLine(v_s[i], v_s[i + 1]);
            }
            Gizmos.color = Color.cyan;
            for (int i = 0; i < len; i++)
            {
                Gizmos.DrawSphere(v_s[i], 1);
            }
        }
    }

    public double RuntimeParabola(out Vector2 h_dir, out float h_speed)
    {
        float h = h_dis(out var dir, out Vector2 start);
        h_dir = dir;
        h_speed = h / time;
        float s = destination.position.y - original.position.y;
        return Math.Abs((vy * time - s) / (0.5 * time * time));
    }

    private void CalParabola(out Vector3[] ps)
    {
        float h = h_dis(out var dir, out Vector2 start);
        float vx = h / time;
        float s = destination.position.y - original.position.y;
        ps = new Vector3[count + 1];
        double a = Math.Abs((vy * time - s) / (0.5 * time * time));
        for (int i = 0; i < count + 1; i++)
        {
            float t = (time * i) / count;
            var h1 = start + dir * vx * t;
            var v1 = original.position.y - (float)(0.5 * a * (t * t)) + vy * t;
            ps[i] = new Vector3(h1.x, v1, h1.y);
        }
    }


    private float h_dis(out Vector2 dir, out Vector2 start)
    {
        start = new Vector2(original.position.x, original.position.z);
        var end = new Vector2(destination.position.x, destination.position.z);
        dir = (end - start).normalized;
        return Vector2.Distance(end, start);
    }

}
#endif