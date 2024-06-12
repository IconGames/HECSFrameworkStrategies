using System;
using Commands;
using Components;
using HECSFramework.Core;
using Strategies;

namespace Systems
{
    [Serializable]
    [RequiredAtContainer(typeof(AIStrategyComponent))]
    [Documentation(Doc.NPC, Doc.AI, Doc.HECS, "This is default system for operate strategies on NPC")]
    public class AINPCSystem : BaseSystem, IAINPCSystem
    {
        private Strategy currentStrategy;
        private bool isNeedDecision;

        [Required]
        public AIStrategyComponent aIStrategyComponent;

        public StateContextComponent StateContextComponent;

        public void CommandReact(NeedDecisionCommand command)
        {
            Owner.GetComponent<StateContextComponent>().ExitFromStates();
            StateContextComponent.CurrentIteration++;
            isNeedDecision = true;
        }

        public override void InitSystem()
        {
            StateContextComponent = Owner.GetOrAddComponent<StateContextComponent>();
            currentStrategy = aIStrategyComponent.Strategy;
            currentStrategy?.Init();

            if (currentStrategy != null)
                Owner.GetComponent<StateContextComponent>().CurrentStrategyIndex = currentStrategy.StrategyIndex;

            if (aIStrategyComponent.ManualStart)
            {
                aIStrategyComponent.IsStopped = true;
                isNeedDecision = false;
                return;
            }

            if (currentStrategy == null)
                aIStrategyComponent.IsStopped = true;

            isNeedDecision = true;
        }

        public void CommandReact(ChangeStrategyCommand command)
        {
            currentStrategy?.ForceStop(Owner);
            currentStrategy = command.Strategy;
            command.Strategy.Init();

            Owner.GetOrAddComponent<StateContextComponent>().ExitFromStates();
            Owner.GetComponent<StateContextComponent>().CurrentStrategyIndex = currentStrategy.StrategyIndex;
            StateContextComponent.CurrentIteration++;
            isNeedDecision = true;
        }

        public void UpdateLocal()
        {
            if (!isNeedDecision || aIStrategyComponent.IsStopped) return;
            isNeedDecision = false;
            StateContextComponent.CurrentIteration++;
            currentStrategy.Execute(Owner);
        }

        public void CommandReact(SetDefaultStrategyCommand command)
        {
            currentStrategy?.ForceStop(Owner);
            currentStrategy = aIStrategyComponent.Strategy;
            StateContextComponent.CurrentIteration++;
            Owner.GetOrAddComponent<StateContextComponent>().ExitFromStates();
            Owner.GetComponent<StateContextComponent>().CurrentStrategyIndex = currentStrategy.StrategyIndex;
            isNeedDecision = true;
        }

        public override void Dispose()
        {
            if (Owner.TryGetComponent(out StateContextComponent stateContextComponent))
                stateContextComponent.Dispose();

            isNeedDecision = false;
            aIStrategyComponent.IsStopped = false;
        }

        public void CommandReact(ForceStartAICommand command)
        {
            StateContextComponent.CurrentIteration++;
            aIStrategyComponent.IsStopped = false;
            isNeedDecision = true;
        }

        public void CommandReact(ForceStopAICommand command)
        {
            currentStrategy?.ForceStop(Owner);
            Owner.GetOrAddComponent<StateContextComponent>().ExitFromStates();
            StateContextComponent.CurrentIteration++;
            isNeedDecision = false;
            aIStrategyComponent.IsStopped = true;
        }
    }

    public interface IAINPCSystem : ISystem, IUpdatable,
        IReactCommand<NeedDecisionCommand>,
        IReactCommand<SetDefaultStrategyCommand>,
        IReactCommand<ChangeStrategyCommand>,
        IReactCommand<ForceStopAICommand>,
        IReactCommand<ForceStartAICommand>
    {
    }
}