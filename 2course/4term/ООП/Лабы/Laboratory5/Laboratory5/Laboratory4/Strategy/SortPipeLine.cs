using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.Strategy
{
    public class SortPipeline<T>
    {
        private readonly List<ISortCriterion<T>> _criteria = new();

        public SortPipeline<T> AddCriterion(ISortCriterion<T> criterion)
        {
            _criteria.Add(criterion);
            return this;
        }

        public IEnumerable<T> Apply(IEnumerable<T> source)
        {
            if (_criteria.Count == 0)
                return source;

            var first = _criteria[0];
            IOrderedEnumerable<T> ordered = first.ApplyOrder(source);

            for (int i = 1; i < _criteria.Count; i++)
            {
                ordered = _criteria[i].ApplyThenBy(ordered);
            }

            return ordered;
        }
    }
}
