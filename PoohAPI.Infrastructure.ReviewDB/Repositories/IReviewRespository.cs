﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Infrastructure.ReviewDB.Models;

namespace PoohAPI.Infrastructure.ReviewDB.Repositories
{
    public interface IReviewRepository
    {
        IEnumerable<DBReview> GetAllReviews(int maxCount, int offset);
        DBReview GetReview(string q);
        DBReview GetReview(string query, Dictionary<string, object> parameters);
    }
}
