using ControllerUtilities.Response;

namespace AnagraficaAPI.Response
{
    public class TagResponseDto : BaseResponseDto
    {
        public string TagName { get; set; }
        public string Color { get; set; }
    }
}
