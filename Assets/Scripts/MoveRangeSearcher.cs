using UnityEngine;
using System.Collections.Generic;

public class MoveRangeSearcher : MonoBehaviour
{
    public MapManager mapManager;

    public CharacterManager characterManager;


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
    private void BFSRangeMove(Character character, int maxDist)
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
                if (nx < -mapManager.mapWidth / 2 || nx > mapManager.mapWidth / 2 ||
                    nz < -mapManager.mapHeight / 2 || nz > mapManager.mapHeight / 2)
                    continue;

                if (visited.Contains((nx, nz))) continue;

                if (mapManager.fieldDict.TryGetValue((nx, nz), out Field field))
                {
                    if (field.IsProhibit) continue;

                    Character other = characterManager.GetCharacterAtPosition(x, z);
                    if (other != null && other.IsEnemy)
                        break;

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

                if (x < -mapManager.mapWidth / 2 || x > mapManager.mapWidth / 2 ||
                    z < -mapManager.mapHeight / 2 || z > mapManager.mapHeight / 2)
                    break;

                if (mapManager.fieldDict.TryGetValue((x, z), out Field field))
                {
                    if (field.IsProhibit)
                        break;

                    Character other = characterManager.GetCharacterAtPosition(x, z);
                    if (other != null && other.IsEnemy)
                        break;

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
        if (mapManager.fieldDict.TryGetValue((x, z), out Field field))
        {
            field.MoveOn();
        }
    }
}
