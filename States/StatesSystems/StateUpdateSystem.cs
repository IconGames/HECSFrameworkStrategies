﻿using Components;
using HECSFramework.Core;
using Strategies;
using System;

namespace Systems
{
    public class StateUpdateSystem : BaseSystem, IUpdatable, IInitable<State>
    {
        private State state;
        private StateDataComponent dataComponent;
        private HECSMask StateContextComponentMask = HMasks.GetMask<StateContextComponent>();

        public override void InitSystem()
        {
            Owner.TryGetHecsComponent(out dataComponent);
        }

        public void UpdateLocal()
        {
            if (dataComponent.State != StrategyState.Run)
                return;

            dataComponent.UpdateCollection();

            var states = dataComponent.EntitiesInCurrentState;
            var count =  states.Count;

            for (int i = 0; i < count; i++)
            {
                var needed = states[i];

                try
                {
                    if (needed.GetHECSComponent<StateContextComponent>(ref StateContextComponentMask).StrategyState != StrategyState.Run) continue;
                    state.Update.Execute(needed);
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void Init(State state)
        {
            this.state = state;
        }
    }
}
