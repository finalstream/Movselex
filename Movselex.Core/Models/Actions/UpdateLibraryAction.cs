﻿using FinalstreamCommons.Models;

namespace Movselex.Core.Models.Actions
{
    internal class UpdateLibraryAction : MovselexProgressAction
    {
        public override void InvokeProgress(MovselexClient client)
        {
            client.LibraryUpdater.Update();
        }
    }
}