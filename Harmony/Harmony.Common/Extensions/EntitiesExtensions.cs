using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmony.Common.Extensions
{
    public static class EntitiesExtensions
    {
        public static void UpdateChangedProperties<E, U>(this E existingEntity, U updatedVm)
            where E : class
            where U : class
        {
            var type = typeof(U);
            var properties = type.GetProperties();
            string[] IgnoreList = { "id", "isopen", "nodelete" };

            foreach (var property in properties)
            {
                if (IgnoreList.Contains(property.Name.ToLower()))
                    continue;

                var updatedValue = property.GetValue(updatedVm);
                if (updatedValue != null && property.CanWrite)
                {
                    var existingProperty = existingEntity.GetType().GetProperty(property.Name);
                    if (existingProperty != null && existingProperty.PropertyType == property.PropertyType)
                    {
                        try
                        {

                            existingProperty.SetValue(existingEntity, Convert.ChangeType(updatedValue, property.PropertyType));
                        }
                        catch
                        {
                            existingProperty.SetValue(existingEntity, Convert.ChangeType(updatedValue, existingProperty.PropertyType));
                        }
                    }
                }
            }
        }
    }
}
