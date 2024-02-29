using System;
using System.Collections.Generic;
using System.Linq;
using HECSFramework.Core;
using Strategies;

namespace Components
{
    [Serializable, Documentation(Doc.Tag, Doc.AI, Doc.State, "Это компонент всю ключевую информацию про состояние персонажа")]
    public class StateContextComponent : BaseComponent, IDisposable
    {
        public StrategyState StrategyState = StrategyState.Stop;
        public State CurrentState;
        public IDecisionNode EarlyUpdateNode;
        public int CurrentStrategyIndex;
        public int CurrentIteration;
        
        public Stack<IDecisionNode> ExitStateNodes = new Stack<IDecisionNode>(3); //выход из текущего стейта, сеттим на входе в стейт
        public Stack<State> StatesStack = new Stack<State>(3);

        public void ExitFromStates()
        {
            StrategyState = StrategyState.Stop;
            ExitStateNodes.Clear();
            foreach (var state in StatesStack.Where(state => state.OnForceStopNode != null))
            {
                state.OnForceStopNode.Execute(Owner);
            }
            StatesStack.Clear();
        }

        public void Dispose()
        {
            ExitFromStates();
            CurrentStrategyIndex = 0;
            CurrentIteration = 0;
            CurrentState = null;
            EarlyUpdateNode = null;
        }
    }
}