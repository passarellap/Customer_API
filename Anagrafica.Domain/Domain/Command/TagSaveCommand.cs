using ServiceUtilities.Command;

namespace AnagraficaAPI.Domain.Command
{
    public class TagSaveCommand : BaseUpdateCommand
    {
        public string TagName { get; set; }

        public string Color { get; set; }
    }
}
