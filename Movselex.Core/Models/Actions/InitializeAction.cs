namespace Movselex.Core.Models.Actions
{
    internal class InitializeAction : RefreshAction
    {
        public override void InvokeCore(MovselexClient client)
        {
            // �f�[�^�x�[�X���[�h
            LoadDatabase(client);

            // �t�B���^�����O���[�h
            client.MovselexFiltering.Load(client.AppConfig.Language);
        }
    }
}