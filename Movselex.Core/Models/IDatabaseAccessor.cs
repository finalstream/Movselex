using System;
using System.Collections.Generic;

namespace Movselex.Core.Models
{
    internal interface IDatabaseAccessor : IDisposable
    {
        string DatabaseName { get; }

        IEnumerable<LibraryItem> SelectLibrary(LibraryCondition libCondition);
        void ChangeDatabase(string databaseName);
        IEnumerable<GroupItem> SelectGroup();
        IEnumerable<LibraryItem> ShuffleLibrary(int limitNum);
    }
}