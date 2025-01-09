using Ganss.Xss;

namespace mptc.dgc.isaac.verify.service.Dtos
{
    public abstract class BaseSanitizerDto
    {
        public void Sanitize()
        {
            var sanitizer = new HtmlSanitizer();
            var properties = GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite && p.CanRead);

            foreach (var property in properties)
            {
                var value = (string?)property.GetValue(this);
                if (!string.IsNullOrEmpty(value))
                {
                    property.SetValue(this, sanitizer.Sanitize(value));
                }
            }
        }
    }
}