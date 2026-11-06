namespace grafos_lista_02_atv_06.Core.Interfaces
{
    internal interface IDicionarioIndexado<T>
    {
        public int Add(T item);

        public int IndexOf(T item);

        public bool Contains(T item);

        public int Count { get; }

        public T this[int index] { get; }
    }
}
