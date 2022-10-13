using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AppodealAds.Unity.Common
{
    public interface IAppodealInitializationListener
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        void onInitializationFinished(List<string> errors);
    }
}
