﻿namespace Core.Persistance.Paging
{
    public class GetListResponse<T> : BasePageableModel
    {
        public IList<T> Items { get; set; }
    }
}
