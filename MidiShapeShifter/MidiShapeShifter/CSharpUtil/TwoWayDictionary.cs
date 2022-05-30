using System;
using System.Collections.Generic;

namespace MidiShapeShifter.CSharpUtil
{
    /// <summary>
    ///     A dictionary where keys and values are both unique.
    /// </summary>
    /// <example><code>
    ///     TwoWayDictionary<string, string> dict = new TwoWayDictionary<string, string>();
    ///     dict.Add("127.0.0.1", "localhost");
    ///     
    ///     string host;
    ///     dict.TryGetRightByLeft("127.0.0.1", out host);
    ///     // host is "localhost"
    ///    
    ///     string ip;
    ///     dict.TryGetLeftByRight("localhost", out ip);
    ///     // ip is "127.0.0.1"
    /// </code></example>
    /// <remarks> 
    ///     This class was taken from the following post: 
    ///     http://stackoverflow.com/questions/2010789/how-to-get-dictionary-tvalue-from-tkey-in-c
    /// </remarks>
    public class TwoWayDictionary<TLeft, TRight>
    {
        private readonly IDictionary<TLeft, TRight> leftToRight = new Dictionary<TLeft, TRight>();
        private readonly IDictionary<TRight, TLeft> rightToLeft = new Dictionary<TRight, TLeft>();

        public ICollection<TLeft> LeftKeys => leftToRight.Keys;

        public ICollection<TRight> RightKeys => rightToLeft.Keys;

        public void Add(TLeft left, TRight right)
        {
            if (leftToRight.ContainsKey(left))
            {
                throw new InvalidOperationException("key left is duplicate");
            }
            if (rightToLeft.ContainsKey(right))
            {
                throw new InvalidOperationException("key right is duplicate");
            }
            leftToRight.Add(left, right);
            rightToLeft.Add(right, left);
        }

        public bool TryGetRightByLeft(TLeft left, out TRight right)
        {
            return leftToRight.TryGetValue(left, out right);
        }

        public bool TryGetLeftByRight(out TLeft left, TRight right)
        {
            return rightToLeft.TryGetValue(right, out left);
        }
    }
}
