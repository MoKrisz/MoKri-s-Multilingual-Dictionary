using Dictionary.Resources.Messages;

namespace Dictionary.BusinessLogic.Services
{
    public static class EnumService
    {
        public static TEnum ConvertFromInt<TEnum>(int enumIntValue) where TEnum : struct, Enum
        {
            if (!TryConvertFromInt<TEnum>(enumIntValue, out var enumValue))
            {
                throw new ArgumentOutOfRangeException(string.Format(ValidationMessages.InvalidValue, enumIntValue, typeof(TEnum)));
            }

            return enumValue!.Value;
        }

        public static bool TryConvertFromInt<TEnum>(int enumIntValue, out TEnum? enumValue) where TEnum : struct, Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), enumIntValue))
            {
                enumValue = null;
                return false;
            }

            enumValue = (TEnum)Enum.ToObject(typeof(TEnum), enumIntValue);
            return true;
        }
    }
}
