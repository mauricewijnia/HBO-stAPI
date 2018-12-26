﻿using PoohAPI.Logic.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IReviewCommandService
    {
        Review UpdateReview(int reviewId, int companyId, int userId, int stars, string writtenReview, bool anonymous,
        DateTime creationDate, int verifiedReview, int verifiedBy);
        void DeleteReview(int id);
        ReviewPublic PostReview(int companyId, int userId, int stars, string writtenReview, bool anonymous);
    }
}
