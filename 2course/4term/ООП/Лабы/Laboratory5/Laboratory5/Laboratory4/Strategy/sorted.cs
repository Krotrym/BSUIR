using System;
using System.Collections.Generic;
using System.Text;
using Laboratory4.TypeTransport;
namespace Laboratory4.Strategy
{
    public enum SortDirection { Ascending, Descending }

    public class SortByNameCriterion : ISortCriterion<Result>
    {
        private readonly SortDirection _direction;
        public SortByNameCriterion(SortDirection direction) => _direction = direction;

        public IOrderedEnumerable<Result> ApplyOrder(IEnumerable<Result> source) =>
            _direction == SortDirection.Ascending
                ? source.OrderBy(x => x.Type)
                : source.OrderByDescending(x => x.Type);

        public IOrderedEnumerable<Result> ApplyThenBy(IOrderedEnumerable<Result> source) =>
            _direction == SortDirection.Ascending
                ? source.ThenBy(x => x.Type)
                : source.ThenByDescending(x => x.Type);
    }

    public class SortByPriceCriterion : ISortCriterion<Result>
    {
        private readonly SortDirection _direction;
        public SortByPriceCriterion(SortDirection direction) => _direction = direction;

        public IOrderedEnumerable<Result> ApplyOrder(IEnumerable<Result> source) =>
            _direction == SortDirection.Ascending
                ? source.OrderBy(x => x.Cost)
                : source.OrderByDescending(x => x.Cost);

        public IOrderedEnumerable<Result> ApplyThenBy(IOrderedEnumerable<Result> source) =>
            _direction == SortDirection.Ascending
                ? source.ThenBy(x => x.Cost)
                : source.ThenByDescending(x => x.Cost);
    }

    public class SortBySpeedCriterion : ISortCriterion<Result>
    {
        private readonly SortDirection _direction;
        public SortBySpeedCriterion(SortDirection direction) => _direction = direction;

        public IOrderedEnumerable<Result> ApplyOrder(IEnumerable<Result> source) =>
            _direction == SortDirection.Ascending
                ? source.OrderBy(x => x.Time)
                : source.OrderByDescending(x => x.Time);

        public IOrderedEnumerable<Result> ApplyThenBy(IOrderedEnumerable<Result> source) =>
            _direction == SortDirection.Ascending
                ? source.ThenBy(x => x.Time)
                : source.ThenByDescending(x => x.Time);
    }
}
