﻿using Asp.Versioning;
using AutoMapper;
using Fakebook.Application.CQRS.Posts.Commands;
using Fakebook.Application.CQRS.Posts.Queries;
using FakeBook.API.Contracts.Posts.Requests;
using FakeBook.API.Contracts.Posts.Responses;
using FakeBook.API.Extensions;
using FakeBook.API.Filters;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FakeBook.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class PostsController(IMapper mapper, IMediator mediator) : BaseController
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [Route(ApiRoutes.Post.RouteId)]
        [ValidateGuid("id")]
        public async Task<IActionResult> GetById(string id)
        {
            var queryResult = await _mediator.Send(new GetPostById { PostId = Guid.Parse(id)});

            if (!queryResult.Success)
            {
                return HandleErrorResponse(queryResult.Errors);
            }
            var response = _mapper.Map<AbstractPost>(queryResult.Payload);

            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll ()
        {
            var queryResult = await _mediator.Send(new GetAllPosts());

            if (!queryResult.Success ) {
                return HandleErrorResponse(queryResult.Errors);
            }
            var response = _mapper.Map<List<AbstractPost>>(queryResult.Payload);

            return Ok(response);
        }
        [HttpPost]
        [ValidateModel]
        [FileUploadValidation]
        public async Task<IActionResult> CreatePost ([FromForm] PostCreate postCreate)
        {
            var userProfileId = HttpContext.User.GetUserProfileId();;

            var cmd = new CreatePostCmd
            {
                Text = postCreate.Text,
                UserProfileId = userProfileId,
                MediaFiles = postCreate.MediaFiles,
            };

            var cmdRes = await _mediator.Send(cmd);
            if (!cmdRes.Success)
            {
                return HandleErrorResponse(cmdRes.Errors);
            }

            var res = _mapper.Map<AbstractPost>(cmdRes.Payload);

            return CreatedAtAction(nameof(GetById),new {id = res.PostId},res);
        }

        [HttpPatch]
        [Route(ApiRoutes.Post.RouteId)]
        [ValidateGuid("id")]
        [ValidateModel]
        [FileUploadValidation]
        public async Task<IActionResult> UpdatePost([FromForm] PostUpdate updatedPost, string id)
        {
            var userProfileId = HttpContext.User.GetUserProfileId();

            var command = new UpdatePostCmd()
            {
                NewText = updatedPost.Text,
                PostId = Guid.Parse(id),
                UserProfileId = userProfileId,
                MediaFiles = updatedPost.MediaFiles,
            };
            var result = await _mediator.Send(command);

            return !result.Success ? HandleErrorResponse(result.Errors) : NoContent();
        }

        [HttpDelete]
        [Route(ApiRoutes.Post.RouteId)]
        [ValidateGuid("id")]
        public async Task<IActionResult> DeletePost(string id, CancellationToken cancellationToken)
        {
            var command = new DeletePostCmd() { 
                UserProfileId = HttpContext.User.GetUserProfileId(),
                PostId = Guid.Parse(id)};
            var result = await _mediator.Send(command, cancellationToken);

            return !result.Success ? HandleErrorResponse(result.Errors) : NoContent();
        }

        [HttpGet]
        [Route(ApiRoutes.Post.Comments.All)]
        [ValidateGuid("postId")]
        public async Task<IActionResult> GetCommentsByPostId(string postId)
        {
            var query = new GetPostComments() { PostId = Guid.Parse(postId) };
            var result = await _mediator.Send(query);

            if (!result.Success) HandleErrorResponse(result.Errors);

            var comments = _mapper.Map<List<AbstractPostComment>>(result.Payload);
            return Ok(comments);
        }

        [HttpPost]
        [Route(ApiRoutes.Post.Comments.All)]
        [ValidateGuid("postId")]
        [ValidateModel]
        public async Task<IActionResult> AddPostComment(string postId, [FromBody] PostCommentCreate comment)
        {
            var userProfileId = HttpContext.User.GetUserProfileId();

            var command = new AddPostCommentCmd()
            {
                UserProfileId = userProfileId,
                PostId = Guid.Parse(postId),
                CommentText = comment.Text
            };

            var result = await _mediator.Send(command);

            if (!result.Success) return HandleErrorResponse(result.Errors);

            var newComment = _mapper.Map<AbstractPostComment>(result.Payload);

            return Ok(newComment);
        }

        [HttpDelete]
        [Route(ApiRoutes.Post.Comments.Single)]
        [ValidateGuid("postId", "commentId")]
        public async Task<IActionResult> RemoveCommentFromPost(string postId, string commentId)
        {
            var userProfileId = HttpContext.User.GetUserProfileId();
            var postGuid = Guid.Parse(postId);
            var commentGuid = Guid.Parse(commentId);
            var command = new RemovePostCommentCmd
            {
                UserProfileId = userProfileId,
                CommentId = commentGuid,
                PostId = postGuid
            };

            var result = await _mediator.Send(command);

            if (!result.Success) return HandleErrorResponse(result.Errors);

            return NoContent();
        }

        [HttpPut]
        [Route(ApiRoutes.Post.Comments.Single)]
        [ValidateGuid("postId", "commentId")]
        [ValidateModel]
        public async Task<IActionResult> UpdateCommentText(string postId, string commentId,
            PostCommentUpdate updatedComment )
        {
            var postGuid = Guid.Parse(postId);
            var commentGuid = Guid.Parse(commentId);

            var command = new UpdatePostCommentCmd
            {
                PostId = postGuid,
                CommentId = commentGuid,
                UpdatedText = updatedComment.Text
            };

            var result = await _mediator.Send(command);

            if (!result.Success) return HandleErrorResponse(result.Errors);

            return NoContent();
        }
        [HttpGet]
        [Route(ApiRoutes.Post.Interactions.All)]
        [ValidateGuid("postId")]
        public async Task<IActionResult> GetPostInteractions(string postId, CancellationToken token)
        {
            var postGuid = Guid.Parse(postId);
            var query = new GetPostInteractions { PostId = postGuid };
            var result = await _mediator.Send(query, token);

            if (!result.Success) HandleErrorResponse(result.Errors);

            var mapped = _mapper.Map<List<AbstractPostInteraction>>(result.Payload);
            return Ok(mapped);
        }

               
        [HttpPost]
        [Route(ApiRoutes.Post.Interactions.All)]
        [ValidateGuid("postId")]
        [ValidateModel]
        public async Task<IActionResult> AddPostInteraction(string postId, PostInteractionCreate interaction,
         CancellationToken token)
        {
            var postGuid = Guid.Parse(postId);
            var userProfileId = HttpContext.User.GetUserProfileId();
            var command = new AddInteractionCmd
            {
                PostId = postGuid,
                UserProfileId = userProfileId,
                Type = interaction.Type
            };

            var result = await _mediator.Send(command, token);

            if (!result.Success) return HandleErrorResponse(result.Errors);

            var mapped = _mapper.Map<PostInteraction>(result.Payload);

            return Ok(mapped);
        }
        [HttpDelete]
        [Route(ApiRoutes.Post.Interactions.Single)]
        [ValidateGuid("postId", "interactionId")]
        public async Task<IActionResult> RemovePostInteraction(string postId, string interactionId,
        CancellationToken token)
        {
            var postGuid = Guid.Parse(postId);
            var interactionGuid = Guid.Parse(interactionId);
            var userProfileGuid = HttpContext.User.GetUserProfileId();
            var command = new RemovePostInteractionCmd
            {
                PostId = postGuid,
                InteractionId = interactionGuid,
                UserProfileId = userProfileGuid
            };

            var result = await _mediator.Send(command, token);
            if (!result.Success) return HandleErrorResponse(result.Errors);

            var mapped = _mapper.Map<AbstractPostInteraction>(result.Payload);
            return Ok(mapped);
        }
    }
}