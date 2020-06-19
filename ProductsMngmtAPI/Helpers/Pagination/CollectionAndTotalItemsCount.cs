using System.Collections.Generic;

namespace ProductsMngmtAPI.Helpers.Pagination
{
    public class CollectionAndTotalItemsCount<T>
    {
        public IEnumerable<T> Collection { get; private set; }
        public int TotalCount { get; private set; }

        public CollectionAndTotalItemsCount(IEnumerable<T> collection, int totalCount){
            this.Collection = collection;
            this.TotalCount = totalCount;
        }
    }
}