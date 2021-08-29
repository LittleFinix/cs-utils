

using System.Data;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace Finix.CsUtils
{
    public abstract class PropertySerializer
    {
        protected int messageNumber = 1;

        public abstract void WriteScalar(Type expectedType, object value);

        public abstract void WriteProperty(IEnumeratedProperty property);

        public virtual void WriteProperty(IProperty property)
        {
            if (property is IEnumeratedProperty enumeratedProperty)
                WriteProperty(enumeratedProperty);
            else
                WriteProperty(EnumeratedProperty.CreateFrom(null!, property, 0, String.Empty));
        }

        public abstract void WriteDict(IEnumeratedProperty property);

        public abstract void ReadDict(IEnumeratedProperty property);

        public abstract void WriteList(IEnumeratedProperty property);

        public abstract void ReadList(IEnumeratedProperty property);

        public virtual void WalkProperties(IEnumerable<IEnumeratedProperty> properties, Action<IEnumeratedProperty> propAction, Action<IEnumeratedProperty> listAction, Action<IEnumeratedProperty> dictAction)
        {
            foreach (var prop in properties)
            {
                if (prop.IsListType)
                    listAction(prop);
                else if (prop.IsDictionaryType)
                    dictAction(prop);
                else
                    propAction(prop);
            }
        }

        public virtual void WalkProperties(IProperties properties, Action<IEnumeratedProperty> propAction, Action<IEnumeratedProperty> listAction, Action<IEnumeratedProperty> dictAction)
        {
            WalkProperties(properties.EnumerateProperties(recurse: false), propAction, listAction, dictAction);
        }

        protected bool WriteReferenceAndCheck(object? obj)
        {
            var reference = GetReference(obj);
            WriteReference(reference);

            if (!ShouldUpdate(reference))
                return false;

            SetReference(reference, obj);
            return true;
        }

        protected bool ReadReferenceAndCheck(out int reference)
        {
            reference = ReadReference();
            return !HasReference(reference) || ShouldUpdate(reference);
        }

        protected int depth;

        protected virtual void Enter()
        {
            depth++;
        }

        protected virtual void Leave()
        {
            if (--depth == 0)
            {
                messageNumber++;
                visited.Clear();
                // refs.Clear();
                // refsTable.Clear();
            }
        }

        public virtual void Write(Type expectedType, object? obj)
        {
            if (obj is not null && !expectedType.IsAssignableFrom(obj.GetType()))
                throw new ArgumentException("Parameter is not assignable to expected type", nameof(obj));

            Enter();
            if (Property.IsCommonCLRType(expectedType) || obj is string)
                WriteScalar(expectedType, obj);
            else
                WriteObject(expectedType, obj);
            Leave();
        }

        public virtual void Read(Type expectedType, ref object? obj)
        {
            if (obj is not null)
                EnsureExpectedType(expectedType, obj.GetType());

            Enter();
            if (Property.IsComposedType(expectedType))
                ReadObject(expectedType, ref obj);
            else
                ReadScalar(expectedType, ref obj);
            Leave();
        }

        public virtual void WriteObject(Type expectedType, object? obj)
        {
            var reference = GetReference(obj);
            WriteReference(reference);

            if (ShouldUpdate(obj))
            {
                SetReference(reference, obj);
                WalkProperties(new ObjectProperties(obj), WriteProperty, WriteList, WriteDict);
            }
        }

        public virtual void ReadObject(Type expectedType, ref object? obj)
        {
            var reference = ReadReference();

            if (HasReference(reference))
                obj = GetReference(reference);

            if (ShouldUpdate(reference))
            {
                WalkProperties(new ObjectProperties(obj ??= CreateInstance(expectedType, expectedType)), ReadProperty, ReadList, ReadDict);
                SetReference(reference, obj);
            }
        }

        public abstract void ReadScalar(Type expectedType, ref object? value);

        public abstract void ReadProperty(IEnumeratedProperty property);

        public virtual void ReadProperty(IProperty property)
        {
            if (property is IEnumeratedProperty enumeratedProperty)
                ReadProperty(enumeratedProperty);
            else
                ReadProperty(EnumeratedProperty.CreateFrom(null!, property, 0, String.Empty));
        }

        public virtual object? Read(Type type)
        {
            object? obj = null;
            Read(type, ref obj);

            return obj;
        }

        [return: MaybeNull]
        public virtual T Read<T>()
        {
            object? obj = null;
            Read(typeof(T), ref obj);

            return (T?) obj;
        }

        public virtual void Read<T>([MaybeNull] ref T existingObject)
        {
            object? obj = existingObject;
            Read(typeof(T), ref obj);

            existingObject = (T?) obj;
        }

        public virtual void Write<T>([MaybeNull] T obj)
        {
            Write(typeof(T), obj);
        }

        protected int nextRef = 1;

        protected Dictionary<int, WeakReference<object>> refs = new();

        protected HashSet<object> visited = new(ReferenceEqualityComparer.Instance);

        protected ConditionalWeakTable<object, Ref> refsTable = new();

        protected virtual void SetReference(int reference, object? obj)
        {
            if (reference == 0 || obj is null)
                return;

            if (TryGetReference(reference, out var oldObj))
                refsTable.Remove(oldObj);

            visited.Add(obj);
            refs[reference] = new WeakReference<object>(obj);
            refsTable.AddOrUpdate(obj, new() {
                reference = reference,
                messageNumber = messageNumber
            });
        }

        protected virtual bool TryGetReference(int reference, out object? obj)
        {
            obj = null;
            return refs.TryGetValue(reference, out var oldObjRef) && oldObjRef.TryGetTarget(out obj);
        }

        protected virtual bool ShouldUpdate(int reference)
        {
            return reference > 0 && (!TryGetReference(reference, out var r) || ShouldUpdate(r));
        }

        protected virtual bool ShouldUpdate(object? obj)
        {
            return obj is not null && !visited.Contains(obj);
        }

        protected virtual bool HasReference(int reference)
        {
            return reference == 0 || reference > 0 && TryGetReference(reference, out _);
        }

        protected virtual bool HasReference(object? obj)
        {
            return obj is not null && refsTable.TryGetValue(obj, out _);
        }

        protected virtual int GetReference(object? obj)
        {
            return obj is null ? 0 : (refsTable.TryGetValue(obj, out var refref) ? refref.reference : nextRef++);
        }

        protected virtual object? GetReference(int reference)
        {
            return reference == 0 ? null : TryGetReference(reference, out var r) ? r : throw new NullReferenceException();
        }

        protected abstract void WriteReference(int reference);

        protected abstract int ReadReference();

        protected virtual object CreateInstance(Type type, Type expectedType, int count = 0)
        {
#if DEBUG
            Debug.WriteLine($"Creating instance of {type}");
#endif

            if (type.IsArray)
                return Activator.CreateInstance(type, count) ?? throw new InvalidOperationException();

            return Activator.CreateInstance(type, nonPublic: true) ?? throw new InvalidOperationException();
        }

        protected static void EnsureExpectedType(Type expectedType, Type actualType)
        {
            if (!actualType.IsAssignableTo(expectedType))
                throw new DataException($"The actual type '{actualType}' is not assignable to the expected type '{expectedType}'.");
        }

        protected class Ref
        {
            public int messageNumber;
            public int reference;
        }
    }
}
