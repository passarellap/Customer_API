using System.Collections.Generic;

namespace AnagraficaAPI.Domain.Extension
{
    public static class TagExtension
    {
        public static IEnumerable<string> TagOrderColumnsAvailables()
        {
            return new List<string>()
            {
                "Id"
            };
        }
    }
}
