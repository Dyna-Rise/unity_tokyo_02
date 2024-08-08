using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    public float shootSpeed = 12.0f;    // 矢の速度
    public float shootDelay = 0.25f;    // 発射間隔
    public GameObject bowPrefab;        // 弓のプレハブ ※Instatiateで指定する用
    public GameObject arrowPrefab;      // 矢のプレハブ
    bool inAttack = false;              // 攻撃中フラグ
    GameObject bowObj;                  // 弓のゲームオブジェクト ※Instantiateで生成された弓の実物の情報を格納

    // 攻撃
    public void Attack()
    {
        // 矢を持っている & 攻撃中ではない
        if (ItemKeeper.hasArrows > 0 && inAttack == false)
        {
            ItemKeeper.hasArrows -= 1;  // 矢を減らす
            inAttack = true;            // 攻撃フラグを立てる

            // 矢を撃つ

            //プレイヤーのその時向いているオイラー角をangleZに取得
            PlayerController playerCnt = GetComponent<PlayerController>();
            float angleZ = playerCnt.angleZ; // 回転角度

            // 矢のゲームオブジェクトを作る(進行方向に回転)
            Quaternion r = Quaternion.Euler(0, 0, angleZ);
            GameObject arrowObj = Instantiate(arrowPrefab, transform.position, r);

            // 矢を発射するベクトルを作る
            float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
            float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;
            // 矢に力を加える
            Rigidbody2D body = arrowObj.GetComponent<Rigidbody2D>();
            body.AddForce(v, ForceMode2D.Impulse);
            // 攻撃フラグを下ろす遅延実行
            //Invoke("呼び出したいメソッド名",遅延)
            Invoke("StopAttack", shootDelay);

        }
    }

    // 攻撃停止
    public void StopAttack()
    {
        inAttack = false;       //攻撃フラグを下ろす
    }

    // Start is called before the first frame update
    void Start()
    {
        // 弓をプレイヤーキャラクターに配置
        Vector3 pos = transform.position;
        //Instantiate(対象,位置,回転の様子)→"対象"を生み出すメソッド
        bowObj = Instantiate(bowPrefab, pos, Quaternion.identity);
        bowObj.transform.SetParent(transform); // transform→プレイヤーのtransform 弓の親にプレイヤーキャラクターを設定する
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            //攻撃キーが押された
            Attack();
        }

        //弓の回転と優先順位
        float bowZ = -1; // 弓の Z 値(キャラクターの前にする)
        PlayerController plmv = GetComponent<PlayerController>();

        if (plmv.angleZ > 30 && plmv.angleZ < 150)
        {
            //上向き
            bowZ = 1; // 弓の Z 値(キャラクターの後ろにする)
        }

        // 弓の回転
        bowObj.transform.rotation = Quaternion.Euler(0, 0, plmv.angleZ);
        // 弓の優先順位
        bowObj.transform.position = new Vector3(transform.position.x, transform.position.y, bowZ);
    }
}
