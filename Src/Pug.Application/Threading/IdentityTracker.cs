using System.Collections.Generic;

namespace Pug.Application.Threading
{
    public delegate I NewIdentityCreator<I>();

    internal class IdentityTracker<I>
    {
        List<I> usedIdentifiers = new List<I>();

        NewIdentityCreator<I> newIdentityCreator;

        public IdentityTracker(NewIdentityCreator<I> newIdentityCreator)
        {
            this.newIdentityCreator = newIdentityCreator;
        }

        public I GetNewIdentifier()
        {
            I newIdentifier = newIdentityCreator();

            while (!RegisterIdentifier(newIdentifier))
            {
                newIdentifier = newIdentityCreator();
            }

            return newIdentifier;
        }

        public bool RegisterIdentifier(I identifier)
        {
            if (usedIdentifiers.Contains(identifier))
                return false;

            usedIdentifiers.Add(identifier);

            return true;
        }
    }
}
