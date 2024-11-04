using ControllerUtilities.Request;

namespace AnagraficaAPI.Request
{
    public class TagSaveRequest : BaseUpdateRequest
    {
        public string TagName { get; set; }

        public string Color { get; set; }
    }
}
