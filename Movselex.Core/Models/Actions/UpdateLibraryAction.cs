﻿using FinalstreamCommons.Models;

namespace Movselex.Core.Models.Actions
{
    internal class UpdateLibraryAction : MovselexActionBase
    {
        public override void InvokeCore(MovselexClient client)
        {
            client.IsProgressing = true;
            client.LibraryUpdater.Update();
            client.IsProgressing = false;
        }
    }
}