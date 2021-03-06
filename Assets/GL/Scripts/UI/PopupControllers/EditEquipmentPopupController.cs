﻿using System.Linq;
using GL.UI;
using GL.UI.PopupControllers;
using GL.User;
using HK.GL.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GL.UI.PopupControllers
{
    /// <summary>
    /// 武器変更のポップアップを制御するクラス
    /// </summary>
    /// <remarks>
    /// <see cref="this.submit"/>は装備したい装備品の<c>InstanceId</c>を渡す
    /// </remarks>
    public sealed class EditEquipmentPopupController : PopupBase
    {
        [SerializeField]
        private EquipmentUIController equipmentUIPrefab;

        [SerializeField]
        private Transform listParent;

        [SerializeField]
        private Button unequipButton;

        [SerializeField]
        private Button closeButton;

        public EditEquipmentPopupController SetupAsWeapon(Player player)
        {
            var u = UserData.Instance;
            u.Equipments.List
                .Where(e => e.EquipmentRecord.Rank <= player.CharacterRecord.Rank)
                .Where(e => player.CanEquipmentType(e.EquipmentRecord.EquipmentType))
                .Where(e => !u.IsEquipedEquipment(e))
                .ForEach(e =>
            {
                this.CreateEquipmentUI(e);
            });

            this.SetupOtherButton();

            return this;
        }

        public EditEquipmentPopupController SetupAsAccessory(Player player)
        {
            var u = UserData.Instance;
            u.Equipments.List
                .Where(e => e.EquipmentRecord.Rank <= player.CharacterRecord.Rank)
                .Where(e => e.EquipmentRecord.EquipmentType == Constants.EquipmentType.Accessory)
                .Where(e => !u.IsEquipedEquipment(e))
                .ForEach(e =>
            {
                this.CreateEquipmentUI(e);
            });

            this.SetupOtherButton();

            return this;
        }

        private void CreateEquipmentUI(Equipment equipment)
        {
            Instantiate(this.equipmentUIPrefab, this.listParent, false)
                .Setup(equipment.EquipmentRecord)
                .Button
                .OnClickAsObservable()
                .SubscribeWithState2(this, equipment, (_, _this, e) => _this.submit.OnNext(e.InstanceId))
                .AddTo(this);
        }

        private void SetupOtherButton()
        {
            this.unequipButton.OnClickAsObservable()
                .SubscribeWithState(this, (_, _this) => _this.submit.OnNext(0))
                .AddTo(this);

            this.closeButton.OnClickAsObservable()
                .SubscribeWithState(this, (_, _this) => _this.submit.OnNext(-1))
                .AddTo(this);
        }
    }
}
