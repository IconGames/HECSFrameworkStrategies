using HECSFramework.Core;

namespace Strategies
{
    public sealed class  CompareIntNode : DilemmaDecision
    {
        [Connection(ConnectionPointType.In, "<int> A")]
        public GenericNode<int> ValueA;
        
        [Connection(ConnectionPointType.In, "<int> B")]
        public GenericNode<int> ValueB;

        [ExposeField]
        public Operations Operations;

        public override string TitleOfNode { get; } = "CompareIntNode";

        protected override void Run(Entity entity)
        {
            switch (Operations)
            {
                case Operations.InEqual:
                    if (ValueA.Value(entity) == ValueB.Value(entity))
                    {
                        Positive.Execute(entity);
                        return;
                    }
                    Negative.Execute(entity);
                    break;
                case Operations.InMore:
                    if (ValueA.Value(entity) > ValueB.Value(entity))
                    {
                        Positive.Execute(entity);
                        return;
                    }
                    Negative.Execute(entity);
                    break;
                case Operations.InLess:
                    if (ValueA.Value(entity) < ValueB.Value(entity))
                    {
                        Positive.Execute(entity);
                        return;
                    }
                    Negative.Execute(entity);
                    break;
                case Operations.MoreOrEqual:
                    if (ValueA.Value(entity) >= ValueB.Value(entity))
                    {
                        Positive.Execute(entity);
                        return;
                    }
                    Negative.Execute(entity);
                    break;
                case Operations.LessOrEqual:
                    if (ValueA.Value(entity) <= ValueB.Value(entity))
                    {
                        Positive.Execute(entity);
                        return;
                    }
                    Negative.Execute(entity);
                    break;
            }
        }
    }

    public enum Operations { InEqual, InMore, InLess, MoreOrEqual, LessOrEqual }
}