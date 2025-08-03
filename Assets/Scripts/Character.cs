using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    /// <summary>
    /// 初期X位置
    /// </summary>
    public int initPos_X;

    /// <summary>
    /// 初期Z位置
    /// </summary>
    public int initPos_Z;

    /// <summary>
    /// カメラ
    /// </summary>
    public GameObject MapCamera;

    void Start()
    {
        ///初期位置に配置
        Vector3 pos = new Vector3();
        pos.x = initPos_X;
        pos.y = 0.5f;
        pos.z = initPos_Z;
        transform.position = pos;
    }

    void Update()
    {
    }
}