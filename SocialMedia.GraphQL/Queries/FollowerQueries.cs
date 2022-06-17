﻿using HotChocolate.AspNetCore.Authorization;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.GraphQL.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class FollowerQueries
    {
        #region GetFolloweeAsync
        /// <summary>
        /// Gets a <see cref="Follower"/>.
        /// </summary>
        /// <param name="userId">Represents the id of the user.</param>
        /// <param name="service">A service for <see cref="Follower"/> related operations.</param>
        /// <returns>
        /// A <see cref="Follower"/> if found.
        /// </returns>
        [Authorize]
        public async Task<Follower?> GetFolloweeAsync(string userId, [Service] IFollowerService service)
        {
            return await service.GetFolloweeAsync(userId);
        }
        #endregion

        #region GetFollowerAsync
        /// <summary>
        /// Gets a <see cref="Follower"/>.
        /// </summary>
        /// <param name="userId">Represents the id of the user.</param>
        /// <param name="service">A service for <see cref="Follower"/> related operations.</param>
        /// <returns>
        /// A <see cref="Follower"/> if found.
        /// </returns>
        [Authorize]
        public async Task<Follower?> GetFollowerAsync(string userId, [Service] IFollowerService service)
        {
            return await service.GetFollowerAsync(userId);
        }
        #endregion

        #region GetFollowers
        /// <summary>
        /// Gets a list of <see cref="Follower"/>'s.
        /// </summary>
        /// <param name="service">A service for <see cref="Follower"/> related operations.</param>
        /// <returns>
        /// A list of <see cref="Follower"/>'s.
        /// </returns>
        [Authorize]
        public IQueryable<Follower> GetFollowers([Service] IFollowerService service)
        {
            return service.GetFollowers();
        }
        #endregion

        #region GetFollowing
        /// <summary>
        /// Gets a list of <see cref="Follower"/>'s.
        /// </summary>
        /// <param name="service">A service for <see cref="Follower"/> related operations.</param>
        /// <returns>
        /// A list of <see cref="Follower"/>'s.
        /// </returns>
        [Authorize]
        public IQueryable<Follower> GetFollowing([Service] IFollowerService service)
        {
            return service.GetFollowing();
        }
        #endregion
    }
}
