using Radical.Reflection;
using Radical.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radical.Helpers
{
    public class CommandLine
    {
        static CommandLine _current;

        /// <summary>
        /// Gets the current command line instance.
        /// </summary>
        /// <returns></returns>
        public static CommandLine GetCurrent()
        {
            if (_current == null)
            {
                _current = new CommandLine(Environment.GetCommandLineArgs());
            }
            return _current;
        }

        const char SEPARATOR = '=';

        readonly IEnumerable<string> args;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine"/> class.
        /// </summary>
        /// <param name="args">The current command line arguments.</param>
        public CommandLine(IEnumerable<string> args)
        {
            Ensure.That(args).Named(nameof(args)).IsNotNull();

            this.args = args;
        }

        /// <summary>
        /// Given a command line argument removes leading / or -, and if any,
        /// removes the argument value.
        /// </summary>
        /// <param name="fullArgument">The full argument.</param>
        /// <returns>Just the argument key.</returns>
        static string Normalize(string fullArgument)
        {
            if (fullArgument.StartsWith("/") || fullArgument.StartsWith("-"))
            {
                fullArgument = fullArgument.Substring(1);
            }

            var idx = fullArgument.IndexOf(SEPARATOR);
            if (idx != -1)
            {
                fullArgument = fullArgument.Substring(0, idx);
            }

            return fullArgument;
        }

        /// <summary>
        /// Determines whether the current command contains the specified argument.
        /// </summary>
        /// <param name="arg">The argument to search for.</param>
        /// <returns>
        ///     <c>true</c> if the current command contains the specified argument; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string arg)
        {
            var query = args.Where(s => Normalize(s).Equals(arg, StringComparison.CurrentCultureIgnoreCase));
            return query.Any();
        }

        string GetValue(string argumentValuePair)
        {
            var fullValue = args.Single(s =>
            {
                var sc = StringComparison.CurrentCultureIgnoreCase;
                return Normalize(s).Equals(argumentValuePair, sc);
            });

            var idx = fullValue.IndexOf(SEPARATOR);

            if (idx < 0)
            {
                return null;
            }

            return fullValue.Substring(idx + 1);
        }

        /// <summary>
        /// Tries to safely get a value given the argument name of 
        /// a key/value command line argument.
        /// </summary>
        /// <typeparam name="T">The expected type of the argument value</typeparam>
        /// <param name="arg">The argument name.</param>
        /// <param name="value">The current argument value.</param>
        /// <returns><c>True</c> if the operation succeeded, otherwise <c>false</c>.</returns>
        public bool TryGetValue<T>(string arg, out T value)
        {
            if (Contains(arg))
            {
                var v = GetValue(arg);
                if (!string.IsNullOrEmpty(v))
                {
                    try
                    {
                        var tt = typeof(T);
                        var isNullable = tt.IsGenericType && tt.GetGenericTypeDefinition() == typeof(Nullable<>);
                        if (isNullable)
                        {
                            tt = Nullable.GetUnderlyingType(tt);
                        }

                        if (tt.IsEnum)
                        {
                            var enumValue = Enum.Parse(tt, v, true);
                            value = (T)enumValue;
                        }
                        else
                        {
                            var converted = Convert.ChangeType(v, tt);
                            value = (T)converted;
                        }

                        return true;
                    }
                    catch
                    {
                        //NOP: if something goes badly during the conversion attempt nothing needs to be done.
                    }
                }
            }

            value = default;
            return false;
        }

        public T As<T>() where T : class, new()
        {
            var properties = typeof(T)
                .GetProperties()
                .Where(pi => pi.IsAttributeDefined<CommandLineArgumentAttribute>())
                .Select(pi =>
                {
                    var attribute = pi.GetAttribute<CommandLineArgumentAttribute>();

                    return new
                    {
                        Property = pi,
                        Argument = attribute.ArgumentName,
                        attribute.IsRequired,
                        attribute.Aliases
                    };
                });

            var instance = new T();

            foreach (var property in properties)
            {
                if (!Contains(property.Argument) && !property.Aliases.Any(alias => Contains(alias)) && property.IsRequired)
                {
                    var msg = string.Format("The command line argument '{0}' is required.", property.Argument);
                    throw new ArgumentException(msg, property.Argument);
                }
                else if (Contains(property.Argument) || property.Aliases.Any(alias => Contains(alias)))
                {
                    var lookFor = property.Argument;
                    if (!Contains(lookFor))
                    {
                        lookFor = property.Aliases.First(alias => Contains(alias));
                    }

                    var value = GetValue(lookFor);
                    if (!string.IsNullOrEmpty(value))
                    {
                        var t = property.Property.PropertyType;
                        var isNullable = Nullable.GetUnderlyingType(t) != null;
                        if (isNullable)
                        {
                            t = Nullable.GetUnderlyingType(t);
                        }

                        if (t.IsEnum)
                        {
                            var enumValue = Enum.Parse(t, value, true);
                            property.Property.SetValue(instance, enumValue, null);
                        }
                        else
                        {
                            var converted = Convert.ChangeType(value, t);
                            if (t == typeof(string))
                            {
                                var temp = (string)converted;
                                if (temp.IndexOf(' ') != -1 && temp.StartsWith("\"") && temp.EndsWith("\""))
                                {
                                    converted = temp.Trim('"');
                                }
                            }

                            property.Property.SetValue(instance, converted, null);
                        }
                    }
                    else if (property.Property.PropertyType.Is<bool>())
                    {
                        property.Property.SetValue(instance, Contains(property.Argument), null);
                    }
                }
            }

            return instance;
        }

        public static string AsArguments<T>(T source)
        {
            var properties = typeof(T)
                .GetProperties()
                .Where(pi => pi.IsAttributeDefined<CommandLineArgumentAttribute>())
                .Select(pi => new { Property = pi, Argument = pi.GetAttribute<CommandLineArgumentAttribute>().ArgumentName });

            var builder = new StringBuilder();
            foreach (var p in properties)
            {
                var value = p.Property.GetValue(source, null).ToString();
                if (value.IndexOf(' ') != -1)
                {
                    value = string.Format("\"{0}\"", value);
                }
                builder.AppendFormat("-{0}{1}{2}", p.Argument, SEPARATOR, value);
                builder.Append(' ');
            }

            return builder.ToString().TrimEnd(' ');
        }
    }
}
