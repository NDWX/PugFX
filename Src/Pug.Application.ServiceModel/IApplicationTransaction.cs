
using System;
using Pug.Application.Data;

namespace Pug.Application.ServiceModel
{
    public interface IApplicationTransaction<DS> : IDisposable
        where DS : class, IApplicationDataSession
    {
        string Identifier { get; }
        
        void Commit();

        void Rollback();
    }
}
