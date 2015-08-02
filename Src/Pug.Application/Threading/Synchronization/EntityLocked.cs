using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Pug.Application.Threading.Synchronization
{
    [Serializable]
    public class EntityLocked : Exception
    {
        string identifier;

        public string Identifier
        {
            get { return identifier; }
            private set { identifier = value; }
        }

        public EntityLocked()
            : base()
        {
        }

        public EntityLocked(string identifier)
            : base()
        {
            this.identifier = identifier;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        protected EntityLocked(SerializationInfo info, StreamingContext context)
        {
            this.identifier = info.GetString("Identifier");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Identifier", identifier);
        }
    }
}
