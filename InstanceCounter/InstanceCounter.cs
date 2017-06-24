using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Ingi
{
    public class InstanceCounter
    {
        public static Dictionary<Type, int> Traverse(object obj)
        {
            ISet<object> visited = new HashSet<object>();
            Visit(obj, visited);

            Dictionary<Type, int> results = MakeResults(visited);
            return results;
        }

        private static Dictionary<Type, int> MakeResults(IEnumerable v)
        {
            Dictionary<Type, int> results = new Dictionary<Type, int>();
            foreach (var item in v)
                if (results.ContainsKey(item.GetType()))
                    results[item.GetType()]++;
                else
                    results.Add(item.GetType(), 1);
            return results;
        }

        private static void Visit(object obj, ISet<object> visited)
        {
            if (obj == null || visited.Contains(obj))
                return;

            visited.Add(obj);

            /* If obj is a list of lists, traverse each contained list. */
            var items = obj as IEnumerable;
            if (items != null)
            {
                foreach (var item in items)
                    Visit(item, visited);

                return;
            }

            /* Add code, which is to act upon the object, below this line. Example: Console.WriteLine(obj); */

            /* Recursivly visit each object. */
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                var type = property.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    /* If the property's object is an enumerable, traverse the enumerable. */
                    var enumerable = property.GetValue(obj, null) as IEnumerable;
                    foreach (var item in enumerable ?? new object[] { })
                        Visit(item, visited);
                }
                else
                {
                    /* Visit the object. */
                    var value = property.GetValue(obj);
                    Visit(value, visited);
                }
            }
        }
    }
}