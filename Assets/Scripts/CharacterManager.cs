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

    /// <summary>
    /// キャラクターの登録を解除する
    /// </summary>
    /// <param name="character">解除するキャラクター</param>
    public void UnregisterCharacter(Character character)
    {
        if (characters.Contains(character))
        {
            characters.Remove(character);
        }
    }
}
