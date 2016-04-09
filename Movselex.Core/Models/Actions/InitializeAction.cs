namespace Movselex.Core.Models.Actions
{
    internal class InitializeAction : RefreshAction
    {
        public override void InvokeCore(MovselexClient client)
        {
            // データベースロード
            LoadDatabase(client);

            // フィルタリングロード
            client.MovselexFiltering.Load(client.AppConfig.Language);
        }
    }
}