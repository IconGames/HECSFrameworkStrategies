﻿using System;
using Components;
using HECSFramework.Core;
using Strategies;

[Documentation(Doc.HECS, Doc.Strategy, "This node get base value of counter, if counter doesnt have base value, this node throw exeption")]
public class GetBaseFloatValueFromCounterNode : GenericNode<float>
{
    public override string TitleOfNode { get; } = "GetBaseFloatValueFromCounterNode";

    [Connection(ConnectionPointType.Out, "<float> Out")]
    public BaseDecisionNode Out;

    [DropDownIdentifier("CounterIdentifierContainer")]
    public int CounterID = 0;

    private HECSMask countersHolderMask = HMasks.GetMask<CountersHolderComponent>();

    public override void Execute(IEntity entity)
    {
    }

    public override float Value(IEntity entity)
    {
        if (entity.TryGetHecsComponent(countersHolderMask, out CountersHolderComponent countersHolderComponent))
        {
            if (countersHolderComponent.TryGetCounter<IBaseValue<float>>(CounterID, out var counter))
            {
                return counter.GetBaseValue;
            }
        }
        throw new Exception($"{entity.ID} doesnt have counters holder component");
    }
}
