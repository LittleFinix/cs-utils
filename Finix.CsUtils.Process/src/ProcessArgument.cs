using System.IO;
using System;
using System.Diagnostics;

namespace Finix.CsUtils
{
    public class ProcessArgument
    {
        private string Key { get; set; }

        private string? Value { get; set; }

        public ProcessArgument(object key, object? value = null)
        {
            Key = key?.ToString() ?? throw new ArgumentNullException(nameof(key));
            Value = value?.ToString();
        }

        public string EscapedKey => ProcessUtil.EscapeArgument(Key);

        public string? EscapedValue => Value != null ? ProcessUtil.EscapeArgument(Value) : null;

        public string[] ToArray()
        {
            return EscapedValue != null
                ? (new string[] { EscapedKey, EscapedValue })
                : (new string[] { EscapedKey });
        }

        public static implicit operator ValueTuple<string, string?>(ProcessArgument arg)
        {
            return (arg.EscapedKey, arg.EscapedValue);
        }

        public static implicit operator ProcessArgument(ValueTuple<object, object?> arg)
        {
            return new ProcessArgument(arg.Item1, arg.Item2);
        }

        public static implicit operator ProcessArgument(string arg)
        {
            return new ProcessArgument(arg);
        }

        public static implicit operator ProcessArgument(ValueType arg)
        {
            return new ProcessArgument(arg);
        }
    }
}
