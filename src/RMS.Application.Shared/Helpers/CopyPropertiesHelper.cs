using System;
using System.Linq;
using System.Reflection;

namespace RMS.Helpers
{
    public static class CopyPropertiesHelper
    {
        public static void CopyObjectProperties(this object source, object destination)
        {
            if (source.Equals(null))
            {
                throw new CopyPropertiesFromSourceObjectException();
            }
            else if (destination.Equals(null))
            {
                throw new CopyPropertiesToDestinationObjectException();
            }

            Type sourceType = source.GetType();
            Type destinationType = destination.GetType();

            var propertySet = from sourceProperty in sourceType.GetProperties()
                              let targetProperty = destinationType.GetProperty(sourceProperty.Name)
                              where sourceProperty.CanRead &&
                              targetProperty != null &&
                              (targetProperty.GetSetMethod(true) != null && !targetProperty.GetSetMethod(true).IsPrivate) &&
                              (targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0 &&
                              targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType)
                              select new { sourceProperty, targetProperty };

            foreach(var property in propertySet)
            {
                if (property.targetProperty.CanWrite)
                { 
                    property.targetProperty.SetValue(destination, property.sourceProperty.GetValue(source, null), null);
                }
            }
        }
    }

    internal class CopyPropertiesFromSourceObjectException : Exception
    {
        // TODO: add overrides
    }

    internal class CopyPropertiesToDestinationObjectException : Exception
    {
        // TODO: add overrides
    }
}
