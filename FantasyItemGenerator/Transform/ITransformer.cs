namespace FantasyItemGenerator.Transform
{
    internal interface ITransformer<T, U>
    {
        public U Transform(T input);
    }
}
