using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.Strategy
{
    public interface ISortCriterion<T>
    {
        IOrderedEnumerable<T> ApplyOrder(IEnumerable<T> source);
        IOrderedEnumerable<T> ApplyThenBy(IOrderedEnumerable<T> source);
    }
}
