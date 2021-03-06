﻿using System.Collections.Generic;
using GL.Battle.CharacterControllers.StatusAilments;
using GL.Battle;
using GL.Events.Battle;
using HK.Framework.EventSystems;
using UniRx;
using UnityEngine;

namespace GL.Battle.CharacterControllers
{
    /// <summary>
    /// キャラクターの状態異常を制御するクラス
    /// </summary>
    public sealed class CharacterAilmentController
    {
        public Character Character { get; private set; }

        /// <summary>
        /// 蓄積値
        /// </summary>
        public Resistance Accumulate { get; private set; } = new Resistance();

        /// <summary>
        /// 現在かかっている状態異常
        /// </summary>
        public readonly List<Element> Elements = new List<Element>();

        public CharacterAilmentController(Character character)
        {
            this.Character = character;
            Broker.Global.Receive<EndTurn>()
                .Where(_ => !this.Character.StatusController.IsDead)
                .SubscribeWithState(this, (x, _this) =>
                {
                    if (x.Character == _this.Character)
                    {
                        _this.Elements.ForEach(e => e.EndTurn());
                        _this.Elements.RemoveAll(e => e.CanRemove);
                    }
                    _this.Elements.ForEach(e => e.EndTurnAll(x.Character));
                })
                .AddTo(this.Character);
        }

        /// <summary>
        /// 状態異常を追加する
        /// </summary>
        /// <remarks>
        /// すでにかかっている場合は何もしない
        /// </remarks>
        public bool Add(int remainingTurn, Constants.StatusAilmentType type, float rate)
        {
            if (this.Find(type) || !Calculator.LotteryStatusAilment(this.Character.StatusController, type, rate))
            {
                return false;
            }
            
            this.Elements.Add(Factory.Create(remainingTurn, type, this));
            return true;
        }
        
        /// <summary>
        /// <paramref name="type"/>にかかっているか返す
        /// </summary>
        public bool Find(Constants.StatusAilmentType type)
        {
            return this.Elements.FindIndex(e => e.Type == type) != -1;
        }

        /// <summary>
        /// <see cref="Element"/>を返す
        /// </summary>
        public Element Get(Constants.StatusAilmentType type)
        {
            return this.Elements.Find(e => e.Type == type);
        }

        /// <summary>
        /// ダメージを受けた際の処理
        /// </summary>
        public void TakeDamage()
        {
            this.Elements.ForEach(e => e.TakeDamage());
        }

        public void Remove(Element item)
        {
            this.Elements.Remove(item);
            item.OnRemove();
        }

        private void PublishModifiedStatus()
        {
            Broker.Global.Publish(ModifiedStatus.Get(this.Character));
        }
    }
}
