using Ganss.Xss;

namespace mptc.dgc.isaac.verify.service.Services
{
    public class SanitizationService
    {
        private readonly HtmlSanitizer _sanitizer;

        public SanitizationService()
        {
            _sanitizer = new HtmlSanitizer();
        }

        public string Sanitize(string input)
        {
            return _sanitizer.Sanitize(input);
        }
    }
}