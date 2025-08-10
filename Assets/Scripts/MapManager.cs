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

    public CharacterManager characterManager;

    private const int GENERATE_RATIO_GRASS = 90; // 草ブロックが生成される確率

    /// <summary>
    /// 座標 → Field を紐づける辞書
    /// </summary>
    private Dictionary<(int x, int z), Field> fieldDict = new Dictionary<(int, int), Field>();

    void Start()
    {
        //RandomGenerate();
        SpecifyGenerate();
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

                //生成したブロックの変数を設定
                Field field = obj.GetComponent<Field>();
                field.xPos = (int)pos.x;
                field.zPos = (int)pos.z;

                // 辞書に登録
                fieldDict[(field.xPos, field.zPos)] = field;
            }
        }
    }

    /// <summary>
    /// マップを指定して生成
    /// </summary>
    public void SpecifyGenerate()
    {
        // 0 = 草, 1 = 水
        int[,] mapData = new int[,]
        {
        { 0, 0, 1, 1, 0, 0, 0, 1, 0 },
        { 0, 1, 1, 1, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 1, 0, 1, 0 },
        { 0, 0, 1, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 1, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 1, 1, 0, 0, 0 },
        { 0, 0, 1, 0, 0, 0, 1, 0, 0 },
        { 0, 0, 0, 0, 1, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 1, 0, 0, 0, 0 }
        };

        // サイズを更新
        mapWidth = mapData.GetLength(0);
        mapHeight = mapData.GetLength(1);

        // 古いマップを削除
        foreach (Transform child in mapParent)
        {
            Destroy(child.gameObject);
        }
        fieldDict.Clear();

        // 原点設定
        Vector3 defaultPos = new Vector3(-(mapWidth / 2), 0, -(mapHeight / 2));

        // マップ生成
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                Vector3 pos = defaultPos + new Vector3(i, 0, j);
                GameObject prefab = mapData[i, j] == 0 ? grassBrock : waterBrock;
                GameObject obj = Instantiate(prefab, pos, Quaternion.identity, mapParent);

                //生成したブロックの変数を設定
                Field field = obj.GetComponent<Field>();
                if (prefab == waterBrock)
                {
                    field.IsProhibit = true;
                }
                else
                {
                    field.IsProhibit = false;
                }
                field.xPos = (int)pos.x;
                field.zPos = (int)pos.z;

                // 辞書に登録
                fieldDict[(field.xPos, field.zPos)] = field;
            }
        }
    }

    void Update()
    {

    }

    /// <summary>
    /// 全てのブロックのChoiceOff関数を呼び出す
    /// </summary>
    public void AllChoiceOff()
    {
        foreach (Transform child in mapParent)
        {
            Field field = child.GetComponent<Field>();
            if (field != null)
            {
                field.ChoiceOff();
            }
        }
    }


    /// <summary>
    /// 到達可能なFieldを調べ、MoveOnを呼び出す
    /// </summary>
    public void ResearchReachableField(Character character)
    {
        switch (character.movePattern)
        {
            case Character.MovePattern.Rook:
                LineMove(character, new Vector2Int[]
                {
                new Vector2Int(-1, 0), // 左
                new Vector2Int(1, 0),  // 右
                new Vector2Int(0, -1), // 下
                new Vector2Int(0, 1)   // 上
                });
                break;

            case Character.MovePattern.Bishop:
                LineMove(character, new Vector2Int[]
                {
                new Vector2Int(1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1)
                });
                break;

            case Character.MovePattern.Step3:
                BFSRangeMove(character, 3);
                break;

            case Character.MovePattern.Step4:
                BFSRangeMove(character, 4);
                break;



        }
    }


    /// <summary>
    /// Step移動のための関数(BFSを使用)
    /// </summary>
    /// <param name="character">キャラ情報</param>
    /// <param name="maxDist">動ける距離(マンハッタン距離)</param>
    public void BFSRangeMove(Character character, int maxDist)
    {
        Queue<(int x, int z, int dist)> queue = new Queue<(int, int, int)>();
        HashSet<(int, int)> visited = new HashSet<(int, int)>();

        queue.Enqueue((character.xPos, character.zPos, 0));
        visited.Add((character.xPos, character.zPos));

        // 移動方向ベクトル（左、右、下、上）
        Vector2Int[] directions = new Vector2Int[]
        {
                    new Vector2Int(-1, 0), // 左
                    new Vector2Int(1, 0),  // 右
                    new Vector2Int(0, -1), // 下
                    new Vector2Int(0, 1)   // 上
        };

        while (queue.Count > 0)
        {
            var (x, z, dist) = queue.Dequeue();

            // キャラ判定
            Character c = characterManager.GetCharacterAtPosition(x, z);
            bool isSelf = (c == character);
            bool isEmpty = (c == null);

            if (isEmpty || isSelf)
            {
                SelectFieldAtPosition(x, z);
            }

            // 距離制限
            if (dist >= maxDist) continue;

            // 探索
            foreach (var dir in directions)
            {
                int nx = x + dir.x;
                int nz = z + dir.y;

                // 範囲外チェック
                if (nx < -mapWidth / 2 || nx > mapWidth / 2 ||
                    nz < -mapHeight / 2 || nz > mapHeight / 2)
                    continue;

                if (visited.Contains((nx, nz))) continue;

                if (fieldDict.TryGetValue((nx, nz), out Field field))
                {
                    if (field.IsProhibit) continue;

                    // キャラがいる場合
                    visited.Add((nx, nz));
                    queue.Enqueue((nx, nz, dist + 1));
                }
            }
        }
    }

    /// <summary>
    /// 直線的に移動可能なマスを探索し、選択状態にする共通処理
    /// </summary>
    /// <param name="character">キャラクター</param>
    /// <param name="directions">移動方向ベクトル群</param>
    private void LineMove(Character character, Vector2Int[] directions)
    {
        // 初期位置も移動可能にする
        SelectFieldAtPosition(character.xPos, character.zPos);

        foreach (var dir in directions)
        {
            int x = character.xPos;
            int z = character.zPos;

            while (true)
            {
                x += dir.x;
                z += dir.y;

                if (x < -mapWidth / 2 || x > mapWidth / 2 ||
                    z < -mapHeight / 2 || z > mapHeight / 2)
                    break;

                if (fieldDict.TryGetValue((x, z), out Field field))
                {
                    if (field.IsProhibit)
                        break;

                    Character other = characterManager.GetCharacterAtPosition(x, z);
                    if (other != null && other != character)
                        continue;

                    SelectFieldAtPosition(x, z);
                }
                else
                {
                    break;
                }
            }
        }
    }



    /// <summary>
    /// 指定した座標にあるFieldを選択状態にする
    /// </summary>
    private void SelectFieldAtPosition(int x, int z)
    {
        if (fieldDict.TryGetValue((x, z), out Field field))
        {
            field.MoveOn();
        }
    }

}