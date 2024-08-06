using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//列挙型の自作 （出入り口の位置）※プレイヤーがどこに立つか。
public enum ExitDirection
{ 
    right,      // 右方向
    left,       // 左方向
    down,       // 下方向
    up,         // 上方向
}

//ここからがクラスの本番
public class Exit : MonoBehaviour
{
    public string sceneName;                        //移動先のシーン
    public int doorNumber = 0;                      //ドア番号 デフォルトは0
    public ExitDirection direction = ExitDirection.down;    //ドアをくぐってきたときのプレイヤーの立ち位置 デフォルトは下

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (doorNumber == 100)
            {
               
                // ゲームクリアにする処理
                
            }
            else
            {
                //セーブデータの更新
                //string nowScene = PlayerPrefs.GetString("LastScene");
                //SaveDataManager.SaveArrangeData(nowScene); // 配置データを保存
                
                //シーンの移動 第1引数のシーン名、第2引数のドアNo位置に移動
                RoomManager.ChangeScene(sceneName, doorNumber);
            }
        }
    }
}
