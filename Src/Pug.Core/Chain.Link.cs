namespace Pug
{
    public class Chain<T>
    {
        public class Link
        {
            private readonly T content;
            private readonly Link previous;

            public Link(T transaction, Link parent)
            {
                content = transaction;
                previous = parent;
            }

            public Link Previous { get => previous; }

            public T Content { get => content; }
        }
    }
}
