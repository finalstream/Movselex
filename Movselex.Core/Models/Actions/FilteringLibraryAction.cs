namespace Movselex.Core.Models.Actions
{
    internal class FilteringLibraryAction : MovselexAction
    {

        private readonly string _filteringText;

        public FilteringLibraryAction(string filteringText)
        {
            _filteringText = filteringText;
        }

        public override void InvokeCore(MovselexClient client)
        {
            client.Filterings.ClearSelection();
            client.MovselexLibrary.Load(new LibraryCondition(
                FilteringMode.SQL, 
                FilteringCondition.Empty, 
                client.AppConfig.MaxLimitNum,
                _filteringText));
            client.MovselexGroup.Load();
        }
    }
}