namespace SQT.SqlCompilers
{
    public static class RX
    {
        public static string ID = @"[\w\d\.,_\(\)]+";
        public static string ID_capture = @"(?<IdExpr>[\w\d\.,_]+)";
        public static string ID_AS = @"(?:[\w\d\._\(\)]+)(?: AS [\w\d\.,_\(\)]+)?";
    }
}
