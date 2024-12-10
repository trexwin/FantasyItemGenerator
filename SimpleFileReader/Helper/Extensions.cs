namespace SimpleFileReader.Helper
{
    internal static class Extensions
    {
        public static bool StartsWith<T>(this IEnumerable<T> main, IEnumerable<T> start) where T : IEquatable<T>
        {
            var mainenum = main.GetEnumerator();
            var startenum = start.GetEnumerator();
            while (mainenum.MoveNext() && startenum.MoveNext())
            {
                if (!mainenum.Current.Equals(startenum.Current))
                    return false;
            }

            // If startenum has more values that main, return false
            return !startenum.MoveNext();
        }
    }
}
