﻿using System;
using System.Linq;
using log4net;
using NHibernate;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;

namespace CLMS.Framework.Data
{
    public static class NHExtensions
    {
        public static bool IsDirtyEntity(this ISession session, object entity, bool checkCollections = false)
        {
            ISessionImplementor sessionImpl = session.GetSessionImplementation();
            IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;
            EntityEntry oldEntry = persistenceContext.GetEntry(entity);
            if ((oldEntry == null) && (entity is INHibernateProxy))
            {
                var proxy = entity as INHibernateProxy;
                object obj = sessionImpl.PersistenceContext.Unproxy(proxy);
                oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
            }
            if (oldEntry == null)
            {
                return true;
            }
            string className = oldEntry.EntityName;
            IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);
            object[] oldState = oldEntry.LoadedState;
            object[] currentState = persister.GetPropertyValues(entity);
            //Int32[] dirtyProps = oldState.Select((o, i) => (oldState[i] == currentState[i]) ? -1 : i).Where(x => x >= 0).ToArray();
            int[] dirtyProps = oldState?.Select((o, i) =>
            {
                if (oldState[i] == null && currentState[i] == null) return -1;
                return (oldState[i] != null && oldState[i].Equals(currentState[i])) ? -1 : i;
            }).Where(x => x >= 0).ToArray();
            if (dirtyProps?.Length > 0)
                return true;
            if (checkCollections)
            {
                foreach (var o in currentState)
                {
                    var collection = o as IPersistentCollection;
                    if (collection != null && collection.IsDirty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsDirtyProperty(this ISession session, object entity, string propertyName)
        {
            ISessionImplementor sessionImpl = session.GetSessionImplementation();
            IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;
            EntityEntry oldEntry = persistenceContext.GetEntry(entity);
            if ((oldEntry == null) && (entity is INHibernateProxy))
            {
                var proxy = entity as INHibernateProxy;
                object obj = sessionImpl.PersistenceContext.Unproxy(proxy);
                oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
            }
            //NOTE: If the entity is declared as immutable LoadedState is null
            if (oldEntry?.LoadedState == null)
            {
                return false;
            }
            string className = oldEntry.EntityName;
            IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);
            object[] oldState = oldEntry.LoadedState;
            object[] currentState = persister.GetPropertyValues(entity);
            try
            {
                int[] dirtyProps = persister.FindDirty(currentState, oldState, entity, sessionImpl);
                int index = Array.IndexOf(persister.PropertyNames, propertyName);
                bool isDirty = dirtyProps != null && (Array.IndexOf(dirtyProps, index) != -1);
                return (isDirty);
            }
            catch (Exception e)
            {
                LogManager.GetLogger("NH Extensions").Error("IsDirtyProperty error!", e);
            }
            return false;
        }

        public static object GetOriginalEntityProperty(this ISession session, object entity, string propertyName)
        {
            ISessionImplementor sessionImpl = session.GetSessionImplementation();
            IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;
            EntityEntry oldEntry = persistenceContext.GetEntry(entity);
            if ((oldEntry == null) && (entity is INHibernateProxy))
            {
                INHibernateProxy proxy = entity as INHibernateProxy;
                object obj = sessionImpl.PersistenceContext.Unproxy(proxy);
                oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
            }
            if (oldEntry == null)
            {
                return null;
            }
            string className = oldEntry.EntityName;
            IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);
            object[] oldState = oldEntry.LoadedState;
            object[] currentState = persister.GetPropertyValues(entity);
            int[] dirtyProps = persister.FindDirty(currentState, oldState, entity, sessionImpl);
            int index = Array.IndexOf(persister.PropertyNames, propertyName);
            bool isDirty = (dirtyProps != null) && (Array.IndexOf(dirtyProps, index) != -1);
            return (isDirty ? oldState[index] : currentState[index]);
        }

    }
}
