using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UMTD.Classes
{
    public class uPagination
    {
        public int PageCount;
        public int PageNumber;

        public uPagination(int pageCount, int pageNumber)
        {
            PageCount = pageCount;
            PageNumber = pageNumber;
        }
    }
}