﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enums;
using SocialMedia.Core.Extensions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Models.Post;
using SocialMedia.Core.Objects;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Services
{
    /// <summary>
    /// A service for post related operation.
    /// </summary>
    public class PostService : AbstractService, IPostService
    {
        private readonly IStorageManager _storageManager;

        public PostService(IDbContextFactory<DatabaseContext> dbContextFactory, IHttpContextAccessor contextAccessor, IStorageManager storageManager) : base(dbContextFactory, contextAccessor)
        {
            _storageManager = storageManager;
        }

        #region GetRelevantPostsAsync

        /// <inheritdoc cref="IPostService.GetRelevantPostsAsync(int)"/>
        public async Task<ICollection<Post>> GetRelevantPostsAsync(int amount)
        {
            return amount > 0 ? await _dbContext.Posts
                .Where(options => options.User.Following.Any(options => options.UserId == UserId()))
                .OrderByDescending(options => options.DateCreated)
                .Take(amount)
                .ToListAsync() : new List<Post>();
        }

        #endregion GetRelevantPostsAsync

        #region GetPostByIdAsync

        /// <inheritdoc cref="IPostService.GetPostByIdAsync(string)"/>
        public async Task<Post?> GetPostByIdAsync(string id)
        {
            return Guid.TryParse(id, out _) ? await _dbContext.Posts.FindAsync(id)
                : null;
        }

        #endregion GetPostByIdAsync

        #region GetPostContentAsync

        /// <inheritdoc cref="IPostService.GetPostContentAsync(string)"/>
        public async Task<FileStreamResult?> GetPostContentAsync(string fileName)
        {
            return await _storageManager.DownloadFileAsync(fileName);
        }

        #endregion GetPostContentAsync

        #region PostAsync

        /// <inheritdoc cref="IPostService.PostAsync(CreatePostRequest)"/>
        public async Task<Result<Post>> PostAsync(CreatePostRequest request)
        {
            var validator = new CreatePostValidator();
            var validationResult = validator.Validate(request);

            if (validationResult.IsValid)
            {
                try
                {
                    var fileName = await _storageManager.UploadAsync(request.File);

                    var post = new Post()
                    {
                        Caption = request.Caption ?? string.Empty,
                        Description = request.Description ?? string.Empty,
                        UserId = UserId(),
                        FileName = fileName
                    };

                    _dbContext.Posts.Add(post);
                    await _dbContext.SaveChangesAsync();

                    return Result<Post>.Success(post);
                }
                catch (Exception)
                {
                    return Result<Post>.Failure(ErrorType.Problem, "Something unexpected occurred.");
                }
            }

            return Result<Post>.Failure(ErrorType.BadRequest, "Invalid input.");
        }

        #endregion PostAsync

        #region UpdatePostAsync

        /// <inheritdoc cref="IPostService.UpdatePostAsync(string, UpdatePostRequest)"/>
        public async Task<Result<Post>> UpdatePostAsync(string postId, UpdatePostRequest request)
        {
            var validator = new UpdatePostValidator();
            var validationResult = validator.Validate(request);

            if (validationResult.IsValid)
            {
                if (Guid.TryParse(postId, out _) && postId == request.Id)
                {
                    var post = await _dbContext.Posts.FirstOrDefaultAsync(options => options.Id == postId && options.UserId == UserId());

                    if (post != null)
                    {
                        post.Caption = request.Caption ?? post.Caption;
                        post.Description = request.Description ?? post.Description;
                        post.DateModified = DateTime.UtcNow;

                        await _dbContext.SaveChangesAsync();
                        return Result<Post>.Success(post);
                    }

                    return Result<Post>.Failure(ErrorType.NotFound, "Post not found.");
                }

                return Result<Post>.Failure(ErrorType.BadRequest, "Invalid id.");
            }

            return Result<Post>.Failure(ErrorType.BadRequest, validationResult.ErrorMessage());
        }

        #endregion UpdatePostAsync

        #region DeletePostAsync

        /// <inheritdoc cref="IPostService.DeletePostAsync(string)"/>
        /// <remarks>
        /// May produce the following errors.
        /// <list type="bullet">
        /// <item><see cref="ErrorType.Problem"/></item>
        /// <item><see cref="ErrorType.NotFound"/></item>
        /// <item><see cref="ErrorType.BadRequest"/></item>
        /// </list>
        /// </remarks>
        public async Task<Result<bool>> DeletePostAsync(string id)
        {
            if (Guid.TryParse(id, out _))
            {
                var post = await _dbContext.Posts.FirstOrDefaultAsync(options => options.UserId == UserId() && options.Id == id);

                if (post != null)
                {
                    try
                    {
                        await _storageManager.DeleteAsync(post.FileName);

                        _dbContext.Posts.Remove(post);
                        await _dbContext.SaveChangesAsync();

                        return Result<bool>.Success(true);
                    }
                    catch (Exception)
                    {
                        return Result<bool>.Failure(ErrorType.Problem, "Something unexpected occurred.");
                    }
                }

                return Result<bool>.Failure(ErrorType.NotFound, "Post not found.");
            }

            return Result<bool>.Failure(ErrorType.BadRequest, "Invalid input.");
        }

        #endregion DeletePostAsync
    }
}