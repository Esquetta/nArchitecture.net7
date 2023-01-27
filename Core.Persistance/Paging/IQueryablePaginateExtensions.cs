﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistance.Paging
{
    public static class IQueryablePaginateExtensions
    {
        public static async Task<IPaginate<T>> ToPaginateAsync<T>(this IQueryable<T> source,
            int index, int size, int from = 0,
            CancellationToken cancellationToken = default)
        {
            if (from > index) throw new ArgumentException($"From: {from} > Index: {index}, must from <= Index");

            int count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            List<T> items = await source.Skip((index - from) * size).Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

            Paginate<T> list = new()
            {
                Count = count,
                Size = size,
                From = from,
                Index = index,
                Items = items,
                Pages = (int)Math.Ceiling(count / (double)size)
            };

            return list;
        }

        public static IPaginate<T> ToPaginate<T>(this IQueryable<T> source, int index, int size, int from = 0)
        {
            if (from > index) throw new ArgumentException($"From: {from} > Index: {index}, must from <= Index");

            int count = source.Count();

            List<T> items = source.Skip((index - from) * size).ToList();
            Paginate<T> list = new()
            {
                Index = index,
                Size = size,
                From = from,
                Count = count,
                Items = items,
                Pages = (int)Math.Ceiling(size / (double)size)
            };
            return list;
        }
    }
}
