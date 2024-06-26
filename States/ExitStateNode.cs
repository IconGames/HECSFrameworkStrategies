﻿using System;
using Components;
using HECSFramework.Core;

namespace Strategies
{
    public class ExitStateNode : LogNode, IAddStateNode
    {
        public override string TitleOfNode { get; } = "Exit State";

        [Connection(ConnectionPointType.In, "Input")]
        public BaseDecisionNode Input;

        [Connection(ConnectionPointType.Out, "On Exit")]
        public BaseDecisionNode CallNodesWhenExit;

        private State currentState;

        protected override void Run(Entity entity)
        {
            var context = entity.GetComponent<StateContextComponent>();
            currentState.Stop(entity);
            CallNodesWhenExit?.Execute(entity);
            
            if (context.StatesStack.Count > 0)
            {
                context.StatesStack.Peek().UnPause(entity);
            }
            
            context.ExitStateNodes.Pop().Execute(entity);
        }

        public void AddState(State state)
        {
            currentState = state;
        }
    }
}