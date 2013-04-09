using System;

namespace CSDN.Client.SDK.Entity
{
    public class PageParameter
    {
        public PageParameter()
        {
            PageIndex = 1;
            PageSize = 15;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; internal set; }
        public int PageNumber 
        {
            get {
                int num = RowCount / PageSize;
                if (RowCount % PageSize > 0) num += 1;
                return num;
            }
        }
    }
}
