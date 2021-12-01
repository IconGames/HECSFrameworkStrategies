﻿using HECSFramework.Core;
using HECSFramework.Documentation;
using Strategies;
using System;
using System.Collections.Generic;

namespace Components
{
    [Serializable, Documentation(Doc.AI, Doc.Strategy, Doc.State, "Это основной компонент стейта, он содержит сущности которые сейчас находятся в этом стейте, этот компонент лежит внутри ентити внутри " + nameof(State))]
    public class StateDataComponent : BaseComponent 
    {
        private List<IEntity> entitiesInCurrentState;
        public ReadonlyList<IEntity> EntitiesInCurrentState;
        private Queue<IEntity> addQueue = new Queue<IEntity>(4);
        private Queue<IEntity> removeQueue = new Queue<IEntity>(4);
        private List<IEntity> onPause = new List<IEntity>(4);

        public void Init()
        {
            entitiesInCurrentState = new List<IEntity>();
            EntitiesInCurrentState = new ReadonlyList<IEntity>(entitiesInCurrentState);
        }

        public void AddToState(IEntity entity)
        {
            addQueue.Enqueue(entity);
        }

        public void RemoveFromState(IEntity entity)
        {
            removeQueue.Enqueue(entity);
        }

        public void UpdateCollection()
        {
            while (addQueue.Count > 0)
                entitiesInCurrentState.Add(addQueue.Dequeue());

            while (removeQueue.Count > 0)
                entitiesInCurrentState.Remove(removeQueue.Dequeue());
        }

        public void Pause(IEntity entity)
        {
            if (entitiesInCurrentState.Remove(entity))
                onPause.Add(entity);
        }

        public void UnPause(IEntity entity)
        {
            if (onPause.Remove(entity))
                entitiesInCurrentState.Add(entity);
        }
    }
}