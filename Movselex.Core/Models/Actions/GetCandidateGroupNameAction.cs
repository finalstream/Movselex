using System;
using System.Collections.Generic;
using System.Linq;
using FinalstreamCommons.WebServices;

namespace Movselex.Core.Models.Actions
{
    /// <summary>
    /// グループ名の候補を取得するアクションを表します。
    /// </summary>
    /// <remarks>グループ名の候補はGoogleCustomSearchの結果を使用する</remarks>
    internal class GetCandidateGroupNameAction : MovselexAction
    {
        private readonly string _groupName;

        public IEnumerable<string> CandidateGroupNames { get; private set; } 

        public GetCandidateGroupNameAction(string groupName)
        {
            _groupName = groupName;
        }

        public override void InvokeCore(MovselexClient client)
        {
            var google = new GoogleCustomSearchService("AIzaSyAvvp88pkckq6x1m-DSrECZZJQSeNijlF8", "014354446303595193098:vzav9hoglna");

            var response = google.Query(_groupName);

            CandidateGroupNames = response.items != null ? response.items.Select(x => x.title) : Enumerable.Empty<string>();
        }
    }
}