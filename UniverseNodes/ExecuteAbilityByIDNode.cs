using Commands;
using HECSFramework.Core;

namespace Strategies
{
    [Documentation(Doc.HECS, Doc.Strategy, "here we execute abilities from nodes")]
    public class ExecuteAbilityByIDNode : InterDecision
    {
        public override string TitleOfNode { get; } = "Execute Ability By ID";

        [AbilityIDDropDown]
        public int AbilityId;

        [ExposeField]
        public bool IgnorePredicates;

        [Connection(ConnectionPointType.In, "<IEntity> Target")]
        public GenericNode<IEntity> Target;

        protected override void Run(IEntity entity)
        {
            entity.Command(new ExecuteAbilityByIDCommand
            {
                AbilityIndex = AbilityId,
                Enable = true,
                Owner = entity,
                IgnorePredicates = this.IgnorePredicates,
                Target = Target?.Value(entity),
            });
            Next.Execute(entity);
        }
    }
}