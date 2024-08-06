using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;      //Light 2Dを使うのに必要

public class PlayerLightController : MonoBehaviour
{
    Light2D light2d;                        //Light 2D
    PlayerController playerCnt;             //PlayerControllerスクリプト
    float lightTimer = 0.0f;                //ライトの消費タイマー

    // Start is called before the first frame update
    void Start()
    {
        light2d = GetComponent<Light2D>();                              //Light 2Dを取得
        light2d.pointLightOuterRadius = (float)ItemKeeper.hasLights;    //アイテムの数でライト距離を変更
        playerCnt = GameObject.FindObjectOfType<PlayerController>();    //PlayerController取得
    }
    // Update is called once per frame
    void Update()
    {
        //ライトをプレイヤーに合わせて回転させる
        transform.localEulerAngles = new Vector3(0, 0, playerCnt.angleZ - 90);
        if (ItemKeeper.hasLights > 0)                                    //ライトを持っている
        {
            lightTimer += Time.deltaTime;                                //フレーム時間を加算
            if (lightTimer > 10.0f)                                      //10秒経過
            {
                lightTimer = 0.0f;                                       //タイマーリセット
                ItemKeeper.hasLights--;                                  //ライトアイテムを減らす
                light2d.pointLightOuterRadius = ItemKeeper.hasLights;    //アイテムの数でライト距離を変更
            }
        }
    }

    public void LightUpdate()
    {
        light2d.pointLightOuterRadius = ItemKeeper.hasLights;    //アイテムの数でライト距離を変更
    }
}
