namespace Pug
{
    public partial class Chain<T>
    {
        public class Link
        {
            T content;
            Chain<T>.Link previous;

            public Link(T transaction, Chain<T>.Link parent) : base()
            {
                this.content = transaction;
                this.previous = parent;
            }

            public Chain<T>.Link Previous { get => previous; }

            public T Content { get => content; }
        }
    }
}
