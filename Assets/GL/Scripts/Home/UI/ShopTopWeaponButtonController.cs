﻿using UnityEngine;
using static GL.Battle.Constants;
using UniRx;
using UnityEngine.UI;

namespace GL.Home.UI
{
    /// <summary>
    /// ショップのトップの武器ボタンを制御するクラス
    /// </summary>
    public sealed class ShopTopWeaponButtonController : MonoBehaviour
    {
        [SerializeField]
        private ShopPanelController shopPanelController;

        [SerializeField]
        private WeaponType weaponType;

        [SerializeField]
        private Button button;

        void Start()
        {
            this.button.OnClickAsObservable()
                .SubscribeWithState(this, (_, _this) => _this.shopPanelController.ShowWeaponList(_this.weaponType))
                .AddTo(this);
        }

        void Reset()
        {
            this.button = this.GetComponent<Button>();
        }
    }
}
