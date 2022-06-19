﻿using HotChocolate.AspNetCore.Authorization;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.GraphQL.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class PostQueries
    {
        #region GetPostAsync
        /// <summary>
        /// Used for getting a post.
        /// </summary>
        /// <param name="postId">Represents the id of the post.</param>
        /// <param name="service">A service for <see cref="Post"/> related operations.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation,
        /// a <see cref="Post"/>.
        /// </returns>
        [Authorize]
        public async Task<Post?> GetPostAsync(string postId, [Service] IPostService service)
        {
            return await service.GetPostByIdAsync(postId);
        }
        #endregion
    }
}
