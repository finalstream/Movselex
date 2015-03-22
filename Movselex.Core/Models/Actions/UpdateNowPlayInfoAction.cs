namespace Movselex.Core.Models.Actions
{
    internal class UpdateNowPlayInfoAction : MovselexActionBase
    {

        private readonly string _fileName;

        public UpdateNowPlayInfoAction(string filename)
        {
            _fileName = filename;
        }


        public override void InvokeCore(MovselexClient client)
        {
            var id = client.MovselexLibrary.FindId(_fileName);
            client.NowPlayingInfo.SetId(id);
        }
    }
}