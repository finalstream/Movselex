namespace Movselex.Core.Models.Actions
{
    internal class FilteringLibraryAction : MovselexActionBase
    {

        private readonly string _filteringText;

        public FilteringLibraryAction(string filteringText)
        {
            _filteringText = filteringText;
        }

        public override void InvokeCore(MovselexClient client)
        {
            client.MovselexLibrary.Load(new LibraryCondition(FilteringMode.SQL, "", _filteringText));
            client.MovselexGroup.Load();
        }
    }
}