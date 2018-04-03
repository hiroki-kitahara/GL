﻿using GL.Scripts.Battle.CharacterControllers;
using UnityEngine;
using UnityEngine.Assertions;
using HK.Framework.EventSystems;
using HK.GL.Events.Battle;

namespace HK.GL.Battle
{
    /// <summary>
    /// バトルを管理するヤーツ.
    /// </summary>
    [RequireComponent(typeof(BehavioralOrderController))]
    public sealed class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance { private set; get; }

        public Parties Parties { private set; get; }

        public BehavioralOrderController BehavioralOrder { private set; get; }

        void Awake()
        {
            Assert.IsNull(Instance);
            Instance = this;

            this.BehavioralOrder = this.GetComponent<BehavioralOrderController>();
        }

        void OnDestroy()
        {
            Instance = null;
        }

        public void Initialize(Parties paties)
        {
            this.Parties = paties;
            Broker.Global.Publish(StartBattle.Get());
            this.NextTurn();
        }

        public void NextTurn()
        {
            this.BehavioralOrder.Elapse(this.Parties);
            var order = this.BehavioralOrder.Simulation(this.Parties, Constants.TurnSimulationNumber);
            Broker.Global.Publish(Events.Battle.NextTurn.Get(order));
        }

        public void EndTurn()
        {
            this.BehavioralOrder.EndTurn(this.Parties);

            var battleResult = this.Parties.Result;
            if(battleResult == Constants.BattleResult.Unsettlement)
            {
                Broker.Global.Publish(Events.Battle.EndTurn.Get());
                this.NextTurn();
            }
            else
            {
                Broker.Global.Publish(EndBattle.Get(battleResult));
            }
        }
    }
}
