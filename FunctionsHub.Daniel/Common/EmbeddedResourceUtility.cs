using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common
{
    public class EmbeddedResourceUtility
    {
        Assembly _assembly;
        public EmbeddedResourceUtility(Assembly assemblyWithResources)
        {
            _assembly = assemblyWithResources;
        }

        public EmbeddedResourceUtility()
        {
            _assembly = Assembly.GetCallingAssembly();
        }

        public string GetContent(string resourceName)
        {
            var resource = _assembly.GetManifestResourceNames().Where(c => c.ToLower() == (_assembly.GetName().Name + "." + resourceName).ToLower()).Single();

            using (var stream = _assembly.GetManifestResourceStream(resource))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
