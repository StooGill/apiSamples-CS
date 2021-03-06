﻿using System.Collections.Generic;

namespace dotMailer.PrintCampaignsAndAddressBooks
{
    internal delegate IEnumerable<TResult> PagedFunc<TResult>(int select, int skip);

    internal class DataReader<TObject>
        where TObject: class
    {
        private const int MaxPageSize = 1000;

        private readonly PagedFunc<TObject> _func;
        public int Select { get; set; }

        public DataReader(PagedFunc<TObject> func)
        {
            _func = func;
            Select = MaxPageSize;
        }

        public IEnumerable<TObject> ReadAll()
        {
            int select = Select;
            int skip = 0;

            bool shouldReadMore;
            do
            {
                List<TObject> items = new List<TObject>(_func(select, skip));                
                foreach (TObject item in items)
                {
                    yield return item;
                }

                skip += select;

                shouldReadMore = (select == items.Count);
            } while (shouldReadMore);
        }
    }
}
