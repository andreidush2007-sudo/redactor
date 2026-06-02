using System.Drawing;
using System.Resources;

namespace CircuitEditor
{
    public static class ResourceHelper
    {
        private static ResourceManager _resources;

        static ResourceHelper()
        {
            _resources = new ResourceManager("CircuitEditor.CircuitEditorForm", typeof(CircuitEditorForm).Assembly);
        }

        public static Image GetImage(string name)
        {
            try
            {
                return (Image)_resources.GetObject(name);
            }
            catch
            {
                return null;
            }
        }
    }
}