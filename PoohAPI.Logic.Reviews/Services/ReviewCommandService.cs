﻿using AutoMapper;
using PoohAPI.Infrastructure.ReviewDB.Models;
using PoohAPI.Infrastructure.ReviewDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using System;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;

namespace PoohAPI.Logic.Reviews.Services
{
    public class ReviewCommandService : IReviewCommandService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IReviewReadService _reviewReadService;

        public ReviewCommandService(IReviewRepository reviewRepository, IMapper mapper, IReviewReadService reviewReadService)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _reviewReadService = reviewReadService;
        }

        public void DeleteReview(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@id", id);

            string query = "DELETE FROM reg_reviews WHERE review_id = @id;";

            _reviewRepository.DeleteReview(query, parameters);
        }

        public Review UpdateReview(int reviewId, int companyId, int userId, int stars, string writtenReview, int anonymous, DateTime creationDate, int verifiedReview, int verifiedBy)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@id", reviewId);
            parameters.Add("@companyId", companyId);
            parameters.Add("@userId", userId);
            parameters.Add("@stars", stars);
            parameters.Add("@writtenReview", writtenReview);
            parameters.Add("@anonymous", anonymous);
            parameters.Add("@creationDate", creationDate);
            parameters.Add("@verifiedReview", verifiedReview);
            parameters.Add("@verifiedBy", verifiedBy);

            string query = "UPDATE reg_reviews SET review_id = @id" +
                ", review_bedrijf_id = @companyId, review_student_id = @userId" +
                ", review_sterren = @stars, review_geschreven = @writtenReview" +
                ", review_anoniem = @anonymous, review_datum = @creationDate" +
                ", review_status = @verifiedReview, review_status_bevestigd_door = @verifiedBy" +
                " WHERE review_id = @id;";

            _reviewRepository.UpdateReview(query, parameters);

            return _reviewReadService.GetReviewById(reviewId);
        }

        public ReviewPublic PostReview(int companyId, int userId, int stars, string writtenReview, int anonymous)
        { 
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@bedrijfId", companyId);
            parameters.Add("@studentId", userId);
            parameters.Add("@sterren", stars);
            parameters.Add("@geschreven", writtenReview);
            parameters.Add("@anoniem", anonymous);
            parameters.Add("@datum", DateTime.Now);
            parameters.Add("@status", 0);
            parameters.Add("@bevestigdDoor", 0);

            string query = "INSERT INTO reg_reviews (review_bedrijf_id, review_student_id" +
                ", review_sterren, review_geschreven, review_anoniem, review_datum" +
                ", review_status, review_status_bevestigd_door) " +
                "VALUES (@bedrijfId, @studentId, @sterren, @geschreven, @anoniem, @datum, @status, @bevestigdDoor);" +
                "SELECT LAST_INSERT_ID()";

            var createdReviewId = _reviewRepository.PostReview(query, parameters);

            return _reviewReadService.GetDetailedReviewById(createdReviewId);
        }
    }
}
