using System.Collections.Generic;

namespace Super_Auto_Mobs
{
    public static class ListExtensions
    {
        public static List<T> GetAllExcept<T>(this List<T> list, T element)
        {
            var newList = new List<T>();

            foreach (var currentElement in list)
            {
                if (!currentElement.Equals(element))
                    newList.Add(currentElement);
            }

            return newList;
        }

        public static List<T> GetElementsAfterIndex<T>(this List<T> list, int index)
        {
            var newList = new List<T>();

            for (int i = index + 1; i < list.Count; i++)
            {
                newList.Add(list[i]);
            }

            return newList;
        }
        
        public static List<T> GetElementsBeforeIndex<T>(this List<T> list, int index)
        {
            var newList = new List<T>();

            for (int i = list.Count - 1; i > index; i--)
            {
                newList.Add(list[i]);
            }

            return newList;
        }
    }
}