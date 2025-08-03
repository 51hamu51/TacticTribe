using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    /// <summary>
    /// mapの生成位置
    /// </summary>
	public Transform mapParent;

    /// <summary>
    /// 草ブロック
    /// </summary>
	public GameObject grassBrock;

    /// <summary>
    /// 水ブロック
    /// </summary>
    public GameObject waterBrock;

    /// <summary>
    /// マップの横幅
    /// </summary>
    public int mapWidth = 9;

    /// <summary>
    /// マップの縦幅
    /// </summary>
    public int mapHeight = 9;
    private const int GENERATE_RATIO_GRASS = 90; // 草ブロックが生成される確率

    void Start()
    {
        RandomGenerate();
    }


    /// <summary>
    /// マップをランダム生成する
    /// </summary>
    public void RandomGenerate()
    {

        // ブロック生成位置の基点となる座標を設定
        Vector3 defaultPos = new Vector3(0.0f, 0.0f, 0.0f);
        defaultPos.x = -(mapWidth / 2); // x座標の基点
        defaultPos.z = -(mapHeight / 2); // z座標の基点

        // ブロック生成処理
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                Vector3 pos = defaultPos;
                pos.x += i;
                pos.z += j;

                // ブロックの種類を決定
                int rand = Random.Range(0, 100);
                bool isGrass = false; // 草ブロック生成フラグb
                                      // 乱数値が草ブロック確率値より低ければ草ブロックを生成する
                if (rand < GENERATE_RATIO_GRASS)
                    isGrass = true;

                // オブジェクトを生成
                GameObject obj; // 生成するオブジェクトの参照
                if (isGrass)
                {
                    obj = Instantiate(grassBrock, mapParent); // mapParentの子に草ブロックを生成
                }
                else
                {
                    obj = Instantiate(waterBrock, mapParent); // mapParentの子に水場ブロックを生成
                }
                // オブジェクトの座標を適用
                obj.transform.position = pos;
            }
        }
    }

    /// <summary>
    /// マップを指定して生成
    /// </summary>
    public void SpecifyGenerate()
    {

    }

    void Update()
    {

    }
}