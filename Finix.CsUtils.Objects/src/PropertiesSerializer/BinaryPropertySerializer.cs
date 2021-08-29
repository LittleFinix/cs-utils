using System.Collections.Generic;
using System;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace Finix.CsUtils
{
    public class BinaryPropertySerializer : PropertySerializer
    {
        public BinaryWriter Writer { get; }

        public BinaryReader Reader { get; }

        public BinaryPropertySerializer(Stream stream)
        {
            Writer = new BinaryWriter(stream);
            Reader = new BinaryReader(stream);
        }

        private Type? ReadTypeInfo()
        {
            var typeAvailable = Reader.ReadBoolean();

            if (typeAvailable)
                return Type.GetType(Reader.ReadString(), throwOnError: true)!;
            else
                return null;
        }

        private void WriteTypeInfo(Type? type)
        {
            Writer.Write(type is not null);

            if (type is not null)
                Writer.Write(type.FullName!);
        }

        public override void WriteScalar(Type expectedType, object value)
        {
#if DEBUG
            Debug.WriteLine($"Writing {expectedType} '{value}'");
#endif

            var type = value.GetType();

            EnsureExpectedType(expectedType, type);

            if (!(type.IsPrimitive || type == typeof(string)) || !(expectedType.IsPrimitive || expectedType == typeof(string) || expectedType == typeof(object)))
                throw new ArgumentException("Call to WriteScalar must give primitive or string parameter.", nameof(value));

            if (expectedType != type)
                WriteTypeInfo(type);
            else
                WriteTypeInfo(null);

            switch (value)
            {
                case char c:
                    Writer.Write(c);
                    break;

                case bool bln:
                    Writer.Write(bln);
                    break;

                case byte b:
                    Writer.Write(b);
                    break;

                case sbyte sb:
                    Writer.Write(sb);
                    break;

                case short s:
                    Writer.Write(s);
                    break;

                case ushort us:
                    Writer.Write(us);
                    break;

                case int i:
                    Writer.Write7BitEncodedInt(i);
                    break;

                case uint ui:
                    Writer.Write(ui);
                    break;

                case long l:
                    Writer.Write7BitEncodedInt64(l);
                    break;

                case ulong ul:
                    Writer.Write(ul);
                    break;

                case string s:
                    Writer.Write(s);
                    break;

                case float f:
                    Writer.Write(f);
                    break;

                case double d:
                    Writer.Write(d);
                    break;

                case decimal dec:
                    Writer.Write(dec);
                    break;

                default:
                    throw new NotSupportedException($"Type {type} is not supported.");
            }
        }

        public override void WriteDict(IEnumeratedProperty property)
        {
            if (!WriteReferenceAndCheck(property.Value))
                return;

            var type = property.ValueType;

            var keyType = type.GenericTypeArguments.Length > 0 ? type.GenericTypeArguments[0] : typeof(object);
            var valType = type.GenericTypeArguments.Length > 1 ? type.GenericTypeArguments[1] : typeof(object);

            if (property.Value is null)
                return;

            var realType = property.Value.GetType();

            EnsureExpectedType(type, realType);

            if (type != realType)
                WriteTypeInfo(realType);
            else
                WriteTypeInfo(null);

            foreach (var (key, value) in Dict.Iterate(property.Value))
            {
                Writer.Write(true);
                Write(keyType, key);
                Write(valType, value);
            }

            Writer.Write(false);
        }

        public override void ReadDict(IEnumeratedProperty property)
        {
            if (!ReadReferenceAndCheck(out var reference))
            {
                property.Value = GetReference(reference);
                return;
            }

            var type = property.ValueType;

            var keyType = type.GenericTypeArguments.Length > 0 ? type.GenericTypeArguments[0] : typeof(object);
            var valType = type.GenericTypeArguments.Length > 1 ? type.GenericTypeArguments[1] : typeof(object);

            var realType = ReadTypeInfo() ?? type;

            if (property.Value is not null && !Property.IsDictionaryType(property.Value.GetType()))
                property.Value = null;

            property.Value ??= CreateInstance(realType, type);

            var dict = Dict.Proxy(property.Value);
            dict.Clear();
            // Dict.Clear(dict);

            while (Reader.ReadBoolean())
            {
                object? key = null, value = null;

                Read(keyType, ref key);
                Read(valType, ref value);


                dict[key!] = value;
            }

            SetReference(reference, property.Value);
        }

        private static Type GetListItemType(Type type)
        {
            return type.GetElementType() ?? (type.GenericTypeArguments.Length > 0
                ? type.GenericTypeArguments[0] : typeof(object));
        }

        public override void WriteList(IEnumeratedProperty property)
        {
            if (!WriteReferenceAndCheck(property.Value))
                return;

            var type = property.ValueType;

            var realType = property.Value!.GetType();
            var itemType = GetListItemType(realType);

            EnsureExpectedType(type, realType);

            if (type != realType)
                WriteTypeInfo(realType);
            else
                WriteTypeInfo(null);

            var list = (IList) property.Value;

            Writer.Write7BitEncodedInt(list.Count);

            foreach (var item in list)
                Write(itemType, item);
        }

        public override void ReadList(IEnumeratedProperty property)
        {
            if (!ReadReferenceAndCheck(out var reference))
            {
                property.Value = GetReference(reference);
                return;
            }

            var type = property.ValueType;

            var realType = ReadTypeInfo() ?? type;
            var itemType = GetListItemType(realType);

            EnsureExpectedType(type, realType);

            var count = Reader.Read7BitEncodedInt();
            var oldList = property.Value is not null && property.Value is IList l ? l : null;

            if (oldList is null || oldList.IsReadOnly)
                property.Value = null;

            property.Value ??= CreateInstance(realType, type, count);
            dynamic list = property.Value;

            for (int i = 0; i < count; i++)
            {
                var obj = oldList is not null ? oldList[i] : list[i];

                Read(itemType, ref obj);

                list[i] = obj;
            }

            SetReference(reference, property.Value);
        }

        public override void ReadScalar(Type expectedType, ref object? value)
        {
            var type = ReadTypeInfo() ?? expectedType;

            EnsureExpectedType(expectedType, type);

#if DEBUG
            Debug.WriteLine($"Reading {type}");
#endif

            if (!(type.IsPrimitive || type == typeof(string)) || !(expectedType.IsPrimitive || expectedType == typeof(string) || expectedType == typeof(object)))
                throw new ArgumentException("Call to ReadScalar must use primitive or string type.", nameof(value));

            if (type == typeof(char))
                value = Reader.ReadChar();
            else if (type == typeof(bool))
                value = Reader.ReadBoolean();
            else if (type == typeof(byte))
                value = Reader.ReadByte();
            else if (type == typeof(sbyte))
                value = Reader.ReadSByte();
            else if (type == typeof(short))
                value = Reader.ReadInt16();
            else if (type == typeof(ushort))
                value = Reader.ReadUInt16();
            else if (type == typeof(int))
                value = Reader.Read7BitEncodedInt();
            else if (type == typeof(uint))
                value = Reader.ReadUInt32();
            else if (type == typeof(long))
                value = Reader.Read7BitEncodedInt64();
            else if (type == typeof(ulong))
                value = Reader.ReadUInt64();
            else if (type == typeof(string))
                value = Reader.ReadString();
            else if (type == typeof(float))
                value = Reader.ReadSingle();
            else if (type == typeof(double))
                value = Reader.ReadDouble();
            else if (type == typeof(decimal))
                value = Reader.ReadDecimal();
            else
                throw new NotSupportedException($"Type {type} is not supported.");
        }

        public override void WriteProperty(IEnumeratedProperty property)
        {
            if (property.IsCommonCLRType)
                WriteScalar(property.ValueType, property.Value);
            else if (property.IsComposedType)
                WriteObject(property.ValueType, property.Value);
        }

        public override void ReadProperty(IEnumeratedProperty property)
        {
            var obj = property.Value;

            if (property.IsCommonCLRType)
                ReadScalar(property.ValueType, ref obj);
            else if (property.IsComposedType)
                ReadObject(property.ValueType, ref obj);

            property.Value = obj;
        }

        protected override void WriteReference(int reference)
        {
#if DEBUG
            Debug.WriteLine($"Writing Reference {reference}");
#endif

            Writer.Write7BitEncodedInt(reference);
        }

        protected override int ReadReference()
        {
            var reference = Reader.Read7BitEncodedInt();

#if DEBUG
            Debug.WriteLine($"Reading Reference {reference}");
#endif

            return reference;
        }
    }
}
