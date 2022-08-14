using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyscraper
{
    public class Permutation
    {
        //Author of this code
        //https://stackoverflow.com/questions/25824376/combinations-with-repetitions-c-sharp
        private static Random R = new Random();
        private static List<List<int>> lst = null;
        public static List<int> GetRandomNumber(int NumberofDigit)
        {

            switch (NumberofDigit)
            {
                case 2:
                    lst = lst2Digits;
                    break;
                case 3:
                    lst = lst3Digits;
                    break;
                case 4:
                    lst = lst4Digits;
                    break;
                default:
                    throw new Exception("Number of Digit is " + NumberofDigit.ToString() + " which is not supports");
            }
            int indexResult = R.Next(0, lst.Count);
            return lst[indexResult];



        }
        private static List<List<int>> _lst2Digits = null;
        public static List<List<int>> lst2Digits
        {
            get
            {
                if (_lst2Digits == null)
                {
                    _lst2Digits = GetCombination(new int[] { 1, 2, 3, 4, 5, 6 }, 2);
                }
                return _lst2Digits;

            }
        }

        private static List<List<int>> _lst3Digits = null;
        public static List<List<int>> lst3Digits
        {
            get
            {
                if (_lst3Digits == null)
                {
                    _lst3Digits = GetCombination(new int[] { 1, 2, 3, 4, 5, 6 }, 3);
                }
                return _lst3Digits;

            }
        }

        private static List<List<int>> _lst4Digits = null;
        public static List<List<int>> lst4Digits
        {
            get
            {
                if (_lst4Digits == null)
                {
                    _lst4Digits = GetCombination(new int[] { 1, 2, 3, 4, 5, 6 }, 4);
                }
                return _lst4Digits;

            }
        }
        public static List<List<int>> GetCombination(IEnumerable<int> input, int length)
        {
            List<List<int>> lstReulst = new List<List<int>>();
            foreach (var c in Permutation.CombinationsWithRepetition(input, length))
            {
                List<int> lst = new List<int>();
                foreach (char ch in c)
                {
                    lst.Add(int.Parse(ch.ToString()));
                }
                lstReulst.Add(lst);
            }
            return lstReulst;
        }
        
        public static IEnumerable<int[]> Combinations(int k, int n)
        {
            var result = new int[k];
            var stack = new Stack<int>();
            stack.Push(1);

            while (stack.Count > 0)
            {
                var index = stack.Count - 1;
                var value = stack.Pop();

                while (value <= n)
                {
                    result[index++] = value++;
                    stack.Push(value);
                    if (index == k)
                    {
                        yield return result;
                        break;
                    }
                }
            }
        }
        public static IEnumerable<String> CombinationsWithRepetition(IEnumerable<int> input, int length)
        {
            if (length <= 0)
                yield return "";
            else
            {
                foreach (var i in input)
                    foreach (var c in CombinationsWithRepetition(input, length - 1))
                        yield return i.ToString() + c;
            }



        }


        //https://stackoverflow.com/questions/5132758/words-combinations-without-repetition
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
        {
            int i = 0;
            foreach (var item in items)
            {
                if (count == 1)
                    yield return new T[] { item };
                else
                {
                    foreach (var result in GetPermutations(items.Skip(i + 1), count - 1))
                        yield return new T[] { item }.Concat(result);
                }

                ++i;
            }
        }

    }


}
