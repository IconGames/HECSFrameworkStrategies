using HECSFramework.Core;
using HECSFramework.Unity;
using System;

namespace Components
{
    [Serializable, BluePrint]
    public class UntilSuccessStrategyNodeComponent : BaseComponent
    {
        public BaseDecisionNode BaseDecisionNode { get; set; }
    }
}