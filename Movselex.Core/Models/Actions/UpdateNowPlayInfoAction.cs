namespace Movselex.Core.Models.Actions
{
    internal class UpdateNowPlayInfoAction : MovselexAction
    {

        private readonly string _fileName;

        public UpdateNowPlayInfoAction(string filename)
        {
            _fileName = filename;
        }


        public override void InvokeCore(MovselexClient client)
        {
            var id = client.MovselexLibrary.FindIdByFileName(_fileName);
            client.NowPlayingInfo.SetId(id);
            LibraryItem library = LibraryItem.Empty;
            if (id != -1)
            {
                library = client.MovselexLibrary.GetLibraryItem(id);
            }
            var prevnext = client.MovselexLibrary.GetPreviousAndNextId(library.Gid, library.No);

            client.MovselexLibrary.ResetIsPlaying(library);

            client.NowPlayingInfo.SetLibrary(library);
            client.NowPlayingInfo.SetPreviousAndNextId(prevnext);
            client.MovselexPlaying.Refresh(library);
        }
    }
}