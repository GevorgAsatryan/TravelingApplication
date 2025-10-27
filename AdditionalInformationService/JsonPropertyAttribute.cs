
namespace AdditionalInformationService
{
    internal class JsonPropertyAttribute : Attribute
    {
        private string a;

        public JsonPropertyAttribute(string a)
        {
            this.a = a;
        }
    }
}