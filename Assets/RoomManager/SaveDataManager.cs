using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataList arrangeDataList;    //配置データ

    // Start is called before the first frame update
    void Start()
    {
        //SaveDataList初期化
        arrangeDataList = new SaveDataList();
        arrangeDataList.saveDatas = new SaveData[] { };
        //シーン名を読み込む
        string stageName = PlayerPrefs.GetString("LastScene");
        //シーン名をキーにして保存データを読み込む
        string data = PlayerPrefs.GetString(stageName);
        if (data != "")
        {
            //--- セーブデータが存在する場合 ---
            //JSONからSaveDataListに変換する
            arrangeDataList = JsonUtility.FromJson<SaveDataList>(data);
            for (int i = 0; i < arrangeDataList.saveDatas.Length; i++)
            {
                SaveData savedata = arrangeDataList.saveDatas[i]; //配列から取り出す
                //タグのゲームオブジェクトを探す
                string objTag = savedata.objTag;
                GameObject[] objects = GameObject.FindGameObjectsWithTag(objTag);
                for (int ii = 0; ii < objects.Length; ii++)
                {
                    GameObject obj = objects[ii]; //配列からGameObjectを取り出す
                    //GameObjectのタグを調べる
                    if (objTag == "Door")       //ドア
                    {
                        Door door = obj.GetComponent<Door>();
                        if (door.arrangeId == savedata.arrangeId)
                        {
                            Destroy(obj);  //arrangeIdが同じなら削除
                        }
                    }
                    else if (objTag == "ItemBox")   //宝箱
                    {
                        ItemBox box = obj.GetComponent<ItemBox>();
                        if (box.arrangeId == savedata.arrangeId)
                        {
                            box.isClosed = false;   //arrangeIddが同じなら開く
                            box.GetComponent<SpriteRenderer>().sprite = box.openImage;
                        }
                    }
                    else if (objTag == "Item")      //アイテム
                    {
                        ItemData item = obj.GetComponent<ItemData>();
                        if (item.arrangeId == savedata.arrangeId)
                        {
                            Destroy(obj);   //arrangeIdが同じなら削除
                        }
                    }
                    else if (objTag == "Enemy")      //敵
                    {
                        EnemyController enemy = obj.GetComponent<EnemyController>();
                        if (enemy.arrangeId == savedata.arrangeId)
                        {
                            Destroy(obj);   //arrangeIdが同じなら削除
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //配置Idのセット
    public static void SetArrangeId(int arrangeId, string objTag)
    {
        if (arrangeId == 0 || objTag == "")
        {
            //記録しない
            return;
        }
        //追加するために１つ多いSaveData配列を作る
        Debug.Log(arrangeDataList.saveDatas.Length);
        SaveData[] newSavedatas = new SaveData[arrangeDataList.saveDatas.Length + 1];
        //データをコピーする
        for (int i = 0; i < arrangeDataList.saveDatas.Length; i++)
        {
            newSavedatas[i] = arrangeDataList.saveDatas[i];
        }
        //SaveData作成
        SaveData savedata = new SaveData();
        savedata.arrangeId = arrangeId; //Idを記録
        savedata.objTag = objTag;       //タグを記録
        //SaveData追加
        newSavedatas[arrangeDataList.saveDatas.Length] = savedata;
        arrangeDataList.saveDatas = newSavedatas;
    }

    //配置データの保存
    public static void SaveArrangeData(string stageName)
    {
        if (arrangeDataList.saveDatas != null && stageName != "")
        {
            //SaveDataListをJSONデータに変換
            string saveJson = JsonUtility.ToJson(arrangeDataList);
            //シーン名をキーにして保存
            PlayerPrefs.SetString(stageName, saveJson);
        }
    }
}
