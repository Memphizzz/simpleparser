using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace X_ToolZ
{
    public static class SimpleParser
    {
        public static void Parse(IEnumerable<string> args, dynamic options)
        {
            if (!args.Any())
                return;

            List<string> listArgs = new List<string>(args);

            PropertyInfo[] propertyInfos = options.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo info in propertyInfos)
            {
                IList<CustomAttributeData> customAttributes = info.GetCustomAttributesData();
                if (customAttributes.Count > 0)
                {
                    Option option = (Option) info.GetCustomAttributes(typeof (Option)).FirstOrDefault();

                    if (option.DefaultValue != null)
                        info.SetValue(options, option.DefaultValue);

                    if (customAttributes[0].AttributeType == typeof (IndexOption))
                    {
                        IndexOption indexOption = (IndexOption) info.GetCustomAttributes(typeof (IndexOption)).FirstOrDefault();
                        info.SetValue(options, listArgs[indexOption.Index]);
                        continue;
                    }

                    int index = GetIndex(listArgs, option.ShortName, option.LongName);
                    if (index != -1)
                    {
                        if (customAttributes[0].AttributeType == typeof (Option))
                        {
                            if (listArgs.Count <= index + 1)
                                continue;
                            info.SetValue(options, listArgs[index + 1]);
                            continue;
                        }
                        if (customAttributes[0].AttributeType == typeof (BoolOption))
                        {
                            BoolOption boolOption = (BoolOption) info.GetCustomAttributes(typeof (BoolOption)).FirstOrDefault();
                            if (boolOption.Index == null || (boolOption.Index != null && boolOption.Index == index))
                                info.SetValue(options, true);
                            continue;
                        }
                        if (customAttributes[0].AttributeType == typeof (ListOption))
                        {
                            ListOption listOption = (ListOption) info.GetCustomAttributes(typeof (ListOption)).FirstOrDefault();
                            List<string> tmp = new List<string>();
                            string s = listArgs[index + 1];

                            if (s.Contains(listOption.Separator))
                                tmp.AddRange(s.Split(listOption.Separator));
                            else
                                tmp.Add(s);

                            info.SetValue(options, tmp);
                            continue;
                        }
                    }
                }
            }
        }

        private static int GetIndex(List<string> list, string shortName, string longName)
        {
            int retval = GetIndexInternal(list, shortName, "-");

            if (retval == -1)
                return GetIndexInternal(list, longName, "--");

            return retval;
        }

        private static int GetIndexInternal(List<string> list, string s, string symbol)
        {
            if(s != null)
                for (int i = 0; i < list.Count; i++)
                    if (list[i].ToLower() == s.ToLower() || list[i].ToLower() == symbol + s.ToLower())
                        return i;

            return -1;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class Option : Attribute
    {
        public string ShortName;
        public string LongName;
        public string DefaultValue;

        public Option()
        {
        }

        public Option(string shortName)
        {
            ShortName = shortName;
        }

        public Option(string shortName, string longName)
        {
            ShortName = shortName;
            LongName = longName;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class BoolOption : Option
    {
        public int? Index;

        public BoolOption(string shortName, int index) : base(shortName)
        {
            Index = index;
        }

        public BoolOption(string shortName) : base(shortName)
        {
        }

        public BoolOption(string shortName, string longName) : base(shortName, longName)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ListOption : Option
    {
        public char Separator = ',';

        public ListOption()
        {
        }

        public ListOption(string shortName) : base(shortName)
        {
        }

        public ListOption(string shortName, char separator) : base(shortName)
        {
            Separator = separator;
        }

        public ListOption(string shortName, string longName) : base(shortName, longName)
        {
        }

        public ListOption(string shortName, string longName, char separator) : base(shortName, longName)
        {
            Separator = separator;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class IndexOption : Option
    {
        public int Index;

        public IndexOption(int index)
        {
            Index = index;
        }

    }
}