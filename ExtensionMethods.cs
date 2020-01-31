using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedLoopMethods
{
    public static class EnumerableExtensionMethods
    {

        /*
         * This method apply an action to the items in a collection,
         * the method traverses the collection by recursive calls 
         * Params:
         *  @collection The collection
         *  @action The action to be performed with every element in the collection
         * TypeParams:
         *  @T The type of the elements in the collection
         */
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            IEnumerator<T> cNum = collection.GetEnumerator();
            Action ar = null;//forward declaration : this will exist but i declare it later
            ar = () =>
            {
                if (cNum.MoveNext())
                {
                    action(cNum.Current);
                    ar();
                }
            };
            ar();
        }

        public delegate void changeRef<T> (ref T something);

        public static void ForEachR<T> (this T[] col, changeRef<T> a) {
            for (int i = 0; i < col.Length; i++) {
                a(ref col[i]);
            } }

        public static IEnumerable<T> InvertE<T> (this IEnumerable<T> col)
        {   
            IEnumerator<T> colEnum = col.GetEnumerator();
            Action ar = null;
            IList<T> result = new List<T>();
            if (result.Count() > 100000)//I couldn't know the max capacity of the stack so I used a random max value
            {
                result = result.Reverse().ToList();
            }
            else
            {
                ar = () =>//if list is too big it produces stackOverflow
                {
                    if (colEnum.MoveNext())
                    {
                        var temp = colEnum.Current;
                        ar();
                        result.Add(temp);
                    }
                };
            }
            ar();
            return result;
        }

        /*
         * This method apply an action to the items in a collection
         * Params:
         *  @collection The collection
         *  @action1 The action to be performed with every element in the collection
         *  @action2 The action to be performed with every element in the collection
         * TypeParams:
         *  @T The type of the elements in the collection
         */
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action1, Action<T> action2)
        {
            collection.ForEach(item => {
                action1(item);
                action2(item);
            });
        }

        /*
         * Just a printer of a collection using the ForEach method implemented before,
         * no further information needed
         */
        public static void Show<T>(this IEnumerable<T> collection, String std = "\n")
        {
            collection.ForEach(x => Console.Write("{0}{1}", x, std));
        }

        /*
        * This method apply an action to the items in a collection,
        * the action is only apply to the odd elements in the collection
        * Params:
        *  @collection The collection
        *  @action1 The action to be performed with every element in the collection
        * TypeParams:
        *  @T The type of the elements in the collection
        */
        public static void ForEachOdd<T>(this IEnumerable<T> collection, Action<T> action)
        {
            //IEnumerator<T> cNum = collection.GetEnumerator();
            int counter = 0;
            collection.ForEach(x =>
            {

                if (counter % 2 != 0)
                {
                    action(x);

                }
                counter++;
            });
        }

        public static bool MyContains<T>(this IEnumerable<T> collection, T t)
        {
            bool boo = false;
            collection.ForEach(x =>
            {
                if (x.ToString().CompareTo(t.ToString()) == 0)
                {
                    boo = true;
                    
                }
            });
            return boo;
        }

        public static void ForEachNth<T>(this IEnumerable<T> collection, Action<T> action, int numbers)
        {
            int counter = 0;
            int divisor = numbers;
            collection.ForEach(x =>
            {
                if (counter % divisor == 0)
                {
                    action(x);
                }
                counter++;
            }
            );
        }
        /*
         * This method works as a while loop but done with functional programming
         * Params:
         *  @condition The condition of the whileloop
         *  @body The body of the loop to execute while condition is satisfied
         * TypeParams:
         *  @T The type of the elements in the collection
         */
        public static void WhileLoop(Func<bool> condition, Action body)
        {
            if (condition())
            {
                body();
                WhileLoop(condition,body);
            }
        }
        /*
        * This method apply an action to the items in a collection
        * using the WhileLoop implemented before
        * Params:
        *  @collection The collection
        *  @action The action to be performed with every element in the collection
        * TypeParams:
        *  @T The type of the elements in the collection
        */
        public static void ForEachW<T>(this IEnumerable<T> collection, Action<T> action)
        {
            IEnumerator<T> cEnum = collection.GetEnumerator();
            WhileLoop(() => cEnum.MoveNext(), () => action(cEnum.Current));
        }

        /*
         * This method apply an action to the items in a collection
         * if a predicate is satisfied
         * Params:
         *  @collection The collection
         *  @action The action to be performed with every element in the collection
         *  @predicate The predicate to satisfy
         * TypeParams:
         *  @T The type of the elements in the collection
         */
        public static void ForEachPred<T>(this IEnumerable<T> collection, Action<T> action, Predicate<T> predicate)
        {
            collection.ForEach(x =>
            {
                if (predicate(x))
                {
                    action(x);

                }
            }
            );
        }
    }
}
