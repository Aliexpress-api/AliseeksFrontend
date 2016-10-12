using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE.Models.Search;

namespace AliseeksFE.Injectables.Search
{
    public class PaginationInject
    {
        const int pagesToShow = 5;
        const int itemsPerPage = 48;

        public PaginationInject()
        {

        }

        public int[] GetPages(SearchModel model)
        {
            int maxPage = (int)Math.Ceiling((double)model.Results.SearchCount / itemsPerPage);
            var ret = new List<int>();

            if (model.Criteria.Page > pagesToShow - 1)
            {
                //Center the current page and display some pages before and after
                // 2, 3, [4], 5, 6
                int offset = (int)Math.Floor((double)pagesToShow / 2);
                int current = model.Criteria.Page;

                for(int i = current - offset; i <= maxPage && i <= current + offset; i++)
                {
                    ret.Add(i);
                }
            }
            else
            {
                for(int i = 1; i <= pagesToShow && i <= maxPage; i++)
                {
                    ret.Add(i);
                }
            }

            return ret.ToArray();
        }

        public int[] GetPages(int page, int maxCount, int itemsPerPage)
        {
            int maxPage = (int)Math.Ceiling((double)maxCount / itemsPerPage);
            var ret = new List<int>();

            if (page > pagesToShow - 1)
            {
                //Center the current page and display some pages before and after
                // 2, 3, [4], 5, 6
                int offset = (int)Math.Floor((double)pagesToShow / 2);
                int current = page;

                for (int i = current - offset; i <= maxPage && i <= current + offset; i++)
                {
                    ret.Add(i);
                }
            }
            else
            {
                for (int i = 1; i <= pagesToShow && i <= maxPage; i++)
                {
                    ret.Add(i);
                }
            }

            return ret.ToArray();
        }
    }
}
