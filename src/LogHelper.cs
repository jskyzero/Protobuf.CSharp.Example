using System;
using System.ComponentModel;
using System.Reflection;

namespace Protobuf.Csharp.Example.Controller {
  internal static class LogHelper {
    public enum LogType {
      [Description("LOG")]
      DEFAULT = 0,
      WARNING = 1,
      ERROR = 2,
    }

    private static void Log(string logString) {
      Console.Out.WriteLine(logString);
    }

    public static void Log(LogType type, string logString) {
      Log(string.Format("{0}: {1}", GetDescription(type), logString));
    }

    private static string GetDescription<T>(this T enumerationValue)
    where T : struct {
      Type type = enumerationValue.GetType();
      if (!type.IsEnum) {
      throw new ArgumentException(
      "EnumerationValue must be of Enum type", "enumerationValue");
      }

      //Tries to find a DescriptionAttribute for a potential friendly name
      //for the enum
      MemberInfo[] memberInfo = type.GetMembers();
      if (memberInfo != null && memberInfo.Length > 0) {
        object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attrs != null && attrs.Length > 0) {
          //Pull out the description value
          return ((DescriptionAttribute) attrs[0]).Description;
        }
      }
      //have no description attribute, return the ToString of the enum
      return enumerationValue.ToString();
    }
  }
}