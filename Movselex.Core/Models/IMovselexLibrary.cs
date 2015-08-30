namespace Movselex.Core.Models
{
    public interface IMovselexLibrary
    {
        void Shuffle(int limitNum, LibraryMode libraryMode, bool isSelectAllMovie);
    }
}