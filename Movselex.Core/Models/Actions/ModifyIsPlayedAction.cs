namespace Movselex.Core.Models.Actions
{
    internal class ModifyIsPlayedAction : MovselexActionBase
    {
        private readonly LibraryItem _libraryItem;

        public ModifyIsPlayedAction(LibraryItem library)
        {
            _libraryItem = library;
        }

        public override void InvokeCore(MovselexClient client)
        {
            client.MovselexLibrary.ModifyIsPlayed(_libraryItem);
        }
    }
}