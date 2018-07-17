namespace Pst.Internal.Ltp
{
    internal static class PropertyTypeExtension
    {
        internal static bool IsMultiValue(this PropertyType type) => ((uint)type & 0x1000) == 1;

        internal static bool IsVariableLength(this PropertyType type) =>
            type == PropertyType.String ||
            type == PropertyType.String8 ||
            type == PropertyType.ServerId ||
            type == PropertyType.Restriction ||
            type == PropertyType.RuleAction ||
            type == PropertyType.Binary ||
            type.IsMultiValue();

        internal static int GetLength(this PropertyType type)
        {
            switch (type)
            {
                case PropertyType.Boolean:
                    return 1;

                case PropertyType.Integer16:
                    return 2;

                case PropertyType.Integer32:
                case PropertyType.Floating32:
                case PropertyType.ErrorCode:
                    return 4;

                case PropertyType.Integer64:
                case PropertyType.Floating64:
                case PropertyType.Currency:
                case PropertyType.FloatingTime:
                case PropertyType.Time:
                    return 8;

                case PropertyType.Guid:
                    return 16;

                default:
                    return -1; // not a fixed-size property type
            }
        }
    }
}
