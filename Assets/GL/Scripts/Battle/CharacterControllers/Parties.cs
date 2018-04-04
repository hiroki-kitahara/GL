﻿using System.Collections.Generic;
using GL.Scripts.Battle.Systems;

namespace GL.Scripts.Battle.CharacterControllers
{
    /// <summary>
    /// バトルに参加しているパーティ
    /// </summary>
    public sealed class Parties
    {
        public Party Player { private set; get; }

        public Party Enemy { private set; get; }

        /// <summary>
        /// 今回のバトルに参加しているキャラクター全てのリスト
        /// </summary>
        public List<Character> AllMember { private set; get; }

        public Parties(Party player, Party enemy)
        {
            this.Player = player;
            this.Enemy = enemy;
            this.AllMember = new List<Character>();
            this.AllMember.AddRange(this.Player.Members);
            this.AllMember.AddRange(this.Enemy.Members);
        }

        /// <summary>
        /// 味方パーティを返す
        /// </summary>
        public Party Ally(Character character)
        {
            return character.CharacterType == Constants.CharacterType.Player
                ? this.Player
                : this.Enemy;
        }

        /// <summary>
        /// 敵対しているパーティを返す
        /// </summary>
        public Party Opponent(Character character)
        {
            return character.CharacterType == Constants.CharacterType.Player
                ? this.Enemy
                : this.Player;
        }

        /// <summary>
        /// 勝敗結果を返す
        /// </summary>
        /// <remarks>
        /// 両者とも全て死亡した場合は敵の勝利とする
        /// </remarks>
        public Constants.BattleResult Result
        {
            get
            {
                var isPlayerAlive = this.Player.Members.FindIndex(c => !c.Status.IsDead) != -1;
                var isEnemyAlive = this.Enemy.Members.FindIndex(c => !c.Status.IsDead) != -1;
                if(isPlayerAlive && isEnemyAlive)
                {
                    return Constants.BattleResult.Unsettlement;
                }
                if(isEnemyAlive)
                {
                    return Constants.BattleResult.EnemyWin;
                }

                return Constants.BattleResult.PlayerWin;
            }
        }
    }
}