namespace Generator
{
    public enum CSharpDataType
    {
        // Integer Types
        Int,
        Short,
        Long,
        Byte,
        SByte,
        UInt,
        UShort,
        ULong,

        // Floating-point Types
        Float,
        Double,
        Decimal,

        // Character and String Types
        Char,
        String,

        // Boolean Type
        Bool,

        // Object and Dynamic Types
        Object,
        Dynamic,

        // DateTime Type
        DateTime,

        // Custom Types (User-defined)
        Enum,
        Class,
        Struct,
        Interface,

        // Nullable Types
        Nullable,

        // Collections
        Array,
        List,
        Dictionary,
        IEnumerable,

        // Special Types
        Void,
        Task,
        Action,
        Func
    }
}
