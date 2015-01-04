using System;
using System.Collections.Generic;

namespace Movselex.Core.Models
{
    internal interface IDatabaseAccessor : IDisposable
    {
        string DatabaseName { get; }

        IEnumerable<LibraryItem> SelectLibrary();
        void ChangeDatabase(string databaseName);
    }
}