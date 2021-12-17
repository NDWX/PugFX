namespace Pug
{
    public class Chain<T>
    {
        public class Link
        {
            private T content;
            private Link previous;

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
