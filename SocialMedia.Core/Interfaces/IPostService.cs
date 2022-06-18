﻿using SocialMedia.Core.Entities;
using SocialMedia.Core.Models.Post;
using SocialMedia.Core.Objects;

namespace SocialMedia.Core.Interfaces
{
    /// <summary>
    /// An interface for post related operations.
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// Used for posting.
        /// </summary>
        /// <param name="request">Represents the required data for posting.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// A <see cref="Result{Post}"/>, containing detailes of operation.
        /// </returns>
        public Task<Result<Post>> PostAsync(CreatePostRequest request);
    }
}
