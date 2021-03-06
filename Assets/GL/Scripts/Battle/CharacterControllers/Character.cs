﻿using System;
using GL.Battle.Commands.Bundle;
using GL.Battle.UI;
using GL.Database;
using GL.Events.Battle;
using HK.Framework.EventSystems;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace GL.Battle.CharacterControllers
{
    /// <summary>
    /// キャラクター.
    /// </summary>
    public sealed class Character : MonoBehaviour
    {
        public CharacterRecord Record { get; private set; }

        /// <summary>
        /// ステータスコントローラー
        /// </summary>
        public CharacterStatusController StatusController { get; private set; }

        /// <summary>
        /// 状態異常コントローラー
        /// </summary>
        public CharacterAilmentController AilmentController { get; private set; }
        
        public Constants.CharacterType CharacterType { get; private set; }

        public CharacterUIController UIController { get; private set; }

        /// <summary>
        /// AIコントローラー
        /// </summary>
        /// <remarks>
        /// 敵のみ保持しています
        /// </remarks>
        public AIController AIController { get; private set; }

        public void Initialize(CharacterRecord characterRecord, Implement[] commands, SkillElement[] skillElements, int level, Constants.CharacterType characterType)
        {
            this.Record = characterRecord;
            this.StatusController = new CharacterStatusController(this, characterRecord, commands, skillElements, level);
            this.AilmentController = new CharacterAilmentController(this);
            this.CharacterType = characterType;
            this.UIController = this.GetComponent<CharacterUIController>();
            
            if(this.CharacterType == Constants.CharacterType.Enemy)
            {
                this.AIController = new AIController(this, this.Record.AI);
            }

            Broker.Global.Receive<StartBattle>()
                .Take(1)
                .SubscribeWithState(this, (_, _this) =>
                {
                    _this.StatusController.OnStartBattle();
                })
                .AddTo(this);

            Broker.Global.Receive<StartSelectCommand>()
                .Where(x => x.Character == this)
                .SubscribeWithState(this, (x, _this) =>
                {
                    this.StatusController.AddChargeTurn(1);

                    if (_this.AilmentController.Find(Constants.StatusAilmentType.Paralysis))
                    {
                        Debug.Log("TODO: 麻痺を表現する");
                        _this.InternalEndTurn();
                    }
                    else if (_this.AilmentController.Find(Constants.StatusAilmentType.Sleep))
                    {
                        Debug.Log("TODO: 睡眠を表現する");
                        _this.InternalEndTurn();
                    }
                    else if (_this.AilmentController.Find(Constants.StatusAilmentType.Confuse))
                    {
                        Debug.Log("TODO: 混乱を表現する");
                        var command = BattleManager.Instance.ConfuseCommand;
                        command.Invoke(_this, command.GetTargets(x.Character));
                    }
                    else if (_this.AilmentController.Find(Constants.StatusAilmentType.Berserk))
                    {
                        Debug.Log("TODO: 狂暴を表現する");
                        var command = BattleManager.Instance.BerserkCommand;
                        command.Invoke(_this, command.GetTargets(x.Character));
                    }
                    else
                    {
                        if (_this.CharacterType == Constants.CharacterType.Enemy)
                        {
                            _this.InvokeCommandFromAI();
                        }
                    }

                    Broker.Global.Publish(VisibleRequestSelectCommandPanel.Get(this));
                })
                .AddTo(this);
        }

        /// <summary>
        /// 攻撃を開始する
        /// </summary>
        public IObservable<CharacterUIAnimation.AnimationType> StartAttack(Action onAttack)
        {
            return this.UIController.StartAttack(onAttack);
        }

        /// <summary>
        /// 次のターンに行動する際に行動可能か返す
        /// </summary>
        public bool CanMove
        {
            get
            {
                return
                    !this.AilmentController.Find(Constants.StatusAilmentType.Paralysis) &&
                    !this.AilmentController.Find(Constants.StatusAilmentType.Sleep) &&
                    !this.AilmentController.Find(Constants.StatusAilmentType.Confuse) &&
                    !this.AilmentController.Find(Constants.StatusAilmentType.Berserk);
            }
        }

        /// <summary>
        /// ダメージを受ける
        /// </summary>
        public void TakeDamage(Character invoker, int damage, bool isHit)
        {
            if(isHit)
            {
                this.AilmentController.TakeDamage();
                this.StatusController.HitPoint -= damage;
            }

            Broker.Global.Publish(DamageNotify.Get(this, damage, isHit, invoker));

            if(isHit && this.StatusController.IsDead)
            {
                //this.gameObject.SetActive(false);
                Broker.Global.Publish(DeadNotify.Get(this));
            }
        }

        /// <summary>
        /// 回復する
        /// </summary>
        public void Recovery(int amount)
        {
            var hp = this.StatusController.HitPoint;
            hp = Mathf.Min(hp + amount, this.StatusController.HitPointMax);
            this.StatusController.HitPoint = hp;
            Broker.Global.Publish(RecoveryNotify.Get(this, amount));
        }

        /// <summary>
        /// <paramref name="target"/>との関係性を返す
        /// </summary>
        public Constants.TargetPartyType GetTargetPartyType(Character target)
        {
            return target.CharacterType == this.CharacterType ? Constants.TargetPartyType.Ally : Constants.TargetPartyType.Opponent;
        }

        private void InternalEndTurn()
        {
            BattleManager.Instance.EndTurn(this);
        }

        private void InvokeCommandFromAI()
        {
            var animationController = this.UIController.AnimationController;
            if (!animationController.IsPlay(CharacterUIAnimation.AnimationType.Damage))
            {
                this.AIController.InvokeCommand();
            }
            else
            {
                animationController.OnCompleteAsObservable()
                    .Take(1)
                    .SubscribeWithState(this, (_, _this) => _this.AIController.InvokeCommand())
                    .AddTo(this);
            }
        }
    }
}
