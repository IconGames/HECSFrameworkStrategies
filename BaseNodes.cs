﻿using Components;
using HECSFramework.Core;
using System;
using UnityEngine;

namespace Strategies
{
    public abstract class BaseDecisionNode : ScriptableObject, IDecisionNode
    {
        public abstract string TitleOfNode { get; }
        public Vector2 coords;

        public abstract void Execute(IEntity entity);
    }

    public abstract class InterDecision : BaseDecisionNode
    {
        [Connection(ConnectionPointType.In, "Input")] [IgnoreDraw] public BaseDecisionNode parent;
        [Connection(ConnectionPointType.Link, "Next")] [IgnoreDraw] public BaseDecisionNode next;
    }

    public abstract class DilemmaDecision : BaseDecisionNode
    {
        [Connection(ConnectionPointType.In, "Input")] public BaseDecisionNode parent;
        [Connection(ConnectionPointType.Link, "Positive")] public BaseDecisionNode positive;
        [Connection(ConnectionPointType.Link, "Negative")] public BaseDecisionNode negative;
    }

    public interface IDecisionNode
    {
        void Execute(IEntity entity);
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ConnectionAttribute : Attribute
    {
        public ConnectionPointType ConnectionPointType;
        public string NameOfField = "Field";

        public ConnectionAttribute(ConnectionPointType connectionPointType, string nameOfField)
        {
            ConnectionPointType = connectionPointType;
            NameOfField = nameOfField;
        }
    }

    public enum ConnectionPointType { In, Out, Link, InSingle }
}