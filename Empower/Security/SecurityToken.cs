namespace Empower.Security
{
    public class SecurityToken
    {
        public Header Header { get; set; }
        public SecurityBody Body { get; set; }

        public const string X_CSRF_TOKEN = "X-CSRF-TOKEN";
    }
}
