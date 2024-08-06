using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;      //移動スピード
    int direction = 0;              //移動方向
    float axisH;                    //横軸
    float axisV;                    //縦軸
    public float angleZ = -90.0f;   //回転角度
    Rigidbody2D rbody;              //Rigidbody2D
    Animator animator;              //Animator
    bool isMoving = false;          //移動中フラグ
    // ダメージ対応
    public static int hp = 3;       //プレイヤーのHP
    public static string gameState; //ゲームの状態
    bool inDamage = false;          //ダメージ中フラグ

    // p1 から p2 の角度を返す
    float GetAngle(Vector2 p1, Vector2 p2)
    {
        float angle;
        if (axisH != 0 || axisV != 0)
        {
            // 移動中であれば角度を更新する
            // p1 から p2 への差分(原点を 0 にするため)
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            // アークタンジェント 2 関数で角度 ( ラジアン ) を求め
            float rad = Mathf.Atan2(dy, dx);
            // ラジアンを度に変換して返す
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            // 停止中であれば以前の角度を維持
            angle = angleZ;
        }
        return angle;
    }

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();    //Rigidbody2Dを得る
        animator = GetComponent<Animator>();    //Animatorを得る
        // ゲームの状態をプレイ中にする
        gameState = "playing";
        // HPの更新
        hp = PlayerPrefs.GetInt("PlayerHP");
    }

    // Update is called once per frame
    void Update()
    {
        // ゲーム中以外とダメージ中は何もしない
        if (gameState != "playing" || inDamage)
        {
            return;
        }
        if (isMoving == false)
        {
            axisH = Input.GetAxisRaw("Horizontal"); //左右キー入力
            axisV = Input.GetAxisRaw("Vertical");   //上下キー入力
        }
        // キー入力から移動角度を求める
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt);
        // 移動角度から向いている方向とアニメーション更新
        int dir;
        if (angleZ >= -45 && angleZ < 45)
        {
            // 右向き
            dir = 3;
        }
        else if (angleZ >= 45 && angleZ <= 135)
        {
            // 上向き
            dir = 2;
        }
        else if (angleZ >= -135 && angleZ <= -45)
        {
            // 下向き
            dir = 0;
        }
        else
        {
            // 左向き
            dir = 1;
        }
        if (dir != direction)
        {
            direction = dir;
            animator.SetInteger("Direction", direction);
        }
    }

    void FixedUpdate()
    {
        // ゲーム中以外は何もしない
        if (gameState != "playing")
        {
            return;
        }
        if (inDamage)
        {
            // ダメージ中点滅させる
            float val = Mathf.Sin(Time.time * 50);
            if (val > 0)
            {
                // スプライトを表示
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                // スプライトを非表示
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            return; // ダメージ中は操作による移動させない
        }
        // 移動速度を更新する
        rbody.velocity = new Vector2(axisH, axisV).normalized * speed;
    }

    public void SetAxis(float h, float v)
    {
        axisH = h;
        axisV = v;
        if (axisH == 0 && axisV == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    // 接触
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GetDamage(collision.gameObject);
        }
    }

    // ダメージ
    void GetDamage(GameObject enemy)
    {
        if (gameState == "playing")
        {
            hp--; // HP を減らす
            // HPの更新
            PlayerPrefs.SetInt("PlayerHP", hp);
            if (hp > 0)
            {
                // 移動停止
                rbody.velocity = new Vector2(0, 0);
                // 敵キャラの反対方向にヒットバックさせる
                Vector3 v = (transform.position - enemy.transform.position).normalized;
                rbody.AddForce(new Vector2(v.x * 4, v.y * 4), ForceMode2D.Impulse);
                // ダメージフラグ ON
                inDamage = true;
                Invoke("DamageEnd", 0.25f);
            }
            else
            {
                // ゲームオーバー
                GameOver();
            }
        }
    }
    // ダメージ終了
    void DamageEnd()
    {
        inDamage = false; gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
    // ゲームオーバー
    void GameOver()
    {
        gameState = "gameover";
        // ゲームオーバー演出
        GetComponent<CircleCollider2D>().enabled = false;           // プレイヤーあたりを消す
        rbody.velocity = new Vector2(0, 0);                         // 移動停止
        rbody.gravityScale = 1;                                     // 重力を戻す
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);     // プレイヤーを上に少し跳ね上げる
        animator.SetBool("IsDead", true);                           // アニメーションを切り替える
        Destroy(gameObject, 1.0f);                                  // 1 秒後にプレイヤーを消す

        // BGM停止
        SoundManager.soundManager.StopBgm();
        // SE再生(ゲームオーバー)
        SoundManager.soundManager.SEPlay(SEType.GameOver);
    }
}
