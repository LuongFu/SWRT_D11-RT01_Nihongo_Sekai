using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace JapaneseLearningPlatform.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var memberInfo = enumValue.GetType()
                                      .GetMember(enumValue.ToString())
                                      .FirstOrDefault();
            if (memberInfo != null)
            {
                var displayAttr = memberInfo
                    .GetCustomAttribute<DisplayAttribute>(false);
                if (displayAttr != null)
                    return displayAttr.Name!;
            }
            return enumValue.ToString();
        }
    }
}
