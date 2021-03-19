using System;
using System.ComponentModel;

namespace CySoft.Geometry.Helpers
{
    // Workaround for bug "Using the new init feature gives me this error:"
    // Error CS0518  Predefined type 'System.Runtime.CompilerServices.IsExternalInit' is not defined or imported.
    // See: https://stackoverflow.com/a/62656145/880990

    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public class IsExternalInit { }
}
