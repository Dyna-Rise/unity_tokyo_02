using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKeeper : MonoBehaviour
{
    public static int hasGoldKeys = 0;      // 金カギの数 
    public static int hasSilverKeys = 0;    // 銀カギの数 
    public static int hasArrows = 3;        // 矢の数 
    public static int hasLights = 10;        // ライトの数

    // Start is called before the first frame update
    void Start()
    {
        // アイテムをセーブから読み込む
        //hasGoldKeys = PlayerPrefs.GetInt("GoldKeys");
        //hasSilverKeys = PlayerPrefs.GetInt("SilverKeys");
        //hasArrows = PlayerPrefs.GetInt("Arrows");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // アイテムをセーブする
    public static void SaveItem()
    {
        //PlayerPrefs.SetInt("GoldKeys", hasGoldKeys);
        //PlayerPrefs.SetInt("SilverKeys", hasSilverKeys);
        //PlayerPrefs.SetInt("Arrows", hasArrows);
    }
}
