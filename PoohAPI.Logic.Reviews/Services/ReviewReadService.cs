﻿using AutoMapper;
using PoohAPI.Infrastructure.ReviewDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Reviews.Services
{
    public class ReviewReadService : IReviewReadService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewReadService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public IEnumerable<Review> GetAllReviews(int maxCount, int offset)
        {
            var reviews = _reviewRepository.GetAllReviews(maxCount, offset);
            return _mapper.Map<IEnumerable<Common.Models.Review>>(reviews);
        }

        public Review GetReviewById(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@id", id);

            var query = "SELECT review_id, review_bedrijf_id, review_student_id, review_sterren" +
            ", review_geschreven, review_anoniem, review_datum, review_status" +
            ", review_status_bevestigd_door " +
            "FROM reg_reviews " +
            "WHERE review_id = @id";            

            var dbReview = this._reviewRepository.GetReview(query, parameters);           

            return this._mapper.Map<Review>(dbReview);            
        }
        
        public IEnumerable<int> GetListReviewIdsForUser(int userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@id", userId);

            string query = "SELECT review_id FROM reg_reviews WHERE review_student_id = @id";

            return this._mapper.Map<IEnumerable<int>>(_reviewRepository.GetListReviewIds(query, parameters));
        }

        public IEnumerable<ReviewPublic> GetListReviewsForCompany(int companyId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@id", companyId);

            string query = @"SELECT review_id, review_bedrijf_id, review_sterren, review_geschreven, 
                IF(u.user_name IS NULL, 'Anoniem',
                        CASE WHEN r.review_anoniem = 1 
                        THEN u.user_name 
                        ELSE 'Anoniem' END 
                    ) AS review_student_name, 
                CASE WHEN r.review_anoniem = 0 
                THEN r.review_datum 
                ELSE NULL END AS review_datum 
            FROM reg_reviews r 
            LEFT JOIN reg_users u ON r.review_student_id = u.user_id 
            WHERE review_bedrijf_id = @id";

            var dbReviews = _reviewRepository.GetListReviews(query, parameters);

            foreach (var item in dbReviews)
            {
                 return _mapper.Map<IEnumerable<ReviewPublic>>(dbReviews);
            }

            return null;
        }
    }
}