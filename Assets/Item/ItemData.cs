using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイテムの種類
public enum ItemType
{
    arrow,      // 矢
    GoldKey,    // 金のカギ
    Silverkey,  // 銀のカギ
    life,       // ライフ
    light,      // ライト
}

public class ItemData : MonoBehaviour
{
    public ItemType type;
    public int count = 1;
    public int arrangeId = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //接触
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (type == ItemType.GoldKey)
            {
                //金のカギ
                ItemKeeper.hasGoldKeys += 1;
            }
            else if (type == ItemType.Silverkey)
            {
                //銀のカギ
                ItemKeeper.hasSilverKeys += 1;
            }
            else if (type == ItemType.arrow)
            {
                //矢
                ArrowShoot shoot = collision.gameObject.GetComponent<ArrowShoot>();
                ItemKeeper.hasArrows += count;
            }
            else if (type == ItemType.life)
            {
                //ライフ
                if (PlayerController.hp < 3)
                {
                    // HP が 3 以下の場合加算する
                    PlayerController.hp++;
                    // HPの更新
                    PlayerPrefs.SetInt("PlayerHP", PlayerController.hp);
                }
            }
            else if (type == ItemType.light)
            {
                //ライト
                ItemKeeper.hasLights += 1;
                GameObject.FindObjectOfType<PlayerLightController>().LightUpdate();
            }
            //アイテム取得演出
            // 当たりを消す
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            // アイテムの Rigidbody2D を取ってくる
            Rigidbody2D itemBody = GetComponent<Rigidbody2D>();
            // 重力を戻す
            itemBody.gravityScale = 2.5f;
            // 上に少し跳ね上げる演出
            itemBody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
            // 0.5 秒後に削除
            Destroy(gameObject, 0.5f);

            // 配置Idの記録
            SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
        }
    }
}
