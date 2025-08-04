using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    /// <summary>
    /// 管理しているキャラクター一覧
    /// </summary>
    public List<Character> characters = new List<Character>();

    /// <summary>
    /// キャラクターを登録する
    /// </summary>
    /// <param name="character">登録するキャラクター</param>
    public void RegisterCharacter(Character character)
    {
        if (!characters.Contains(character))
        {
            characters.Add(character);
        }
    }

    /// <summary>
    /// キャラクターの位置を取得（インデックス指定）
    /// </summary>
    public Vector2Int GetCharacterPos(int index)
    {
        if (index < 0 || index >= characters.Count) return Vector2Int.zero;

        Character ch = characters[index];
        return new Vector2Int(ch.xPos, ch.zPos);
    }

    /// <summary>
    /// 指定キャラクターを移動させる
    /// </summary>
    public void MoveCharacter(int index, int x, int z)
    {
        if (index < 0 || index >= characters.Count) return;

        characters[index].MovePosition(x, z);
    }

    /// <summary>
    /// 指定した座標にいるキャラクターを返す（見つからなければ null）
    /// </summary>
    public Character GetCharacterAtPosition(int x, int z)
    {
        foreach (Character ch in characters)
        {
            if (ch.xPos == x && ch.zPos == z)
            {
                return ch;
            }
        }
        return null;
    }
}
