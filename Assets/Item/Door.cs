using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int arrangeId = 0;       // 配置の識別に使う
    public bool IsGoldDoor = false; // 金のドア

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // カギを持っている
            if (IsGoldDoor)
            {
                if (ItemKeeper.hasGoldKeys > 0)
                {
                    ItemKeeper.hasGoldKeys--;       // 金のカギを 1 つ減らす
                    Destroy(this.gameObject);       // ドアを開ける(削除する)
                    
                    // 配置Idの記録
                    //SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
                }
            }
            else
            {
                if (ItemKeeper.hasSilverKeys > 0)
                {
                    ItemKeeper.hasSilverKeys--;     // 銀のカギを 1 つ減らす
                    Destroy(this.gameObject);       // ドアを開ける(削除する)
                    
                    // 配置Idの記録
                    //SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
                }
            }
        }
    }
}
