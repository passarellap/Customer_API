using ServiceUtilities.Model;

namespace AnagraficaAPI.Domain.Model
{
    public class TagModel : BaseModel
    {
        public string TagName { get; set; }
        public string Color { get; set; }
    }
}