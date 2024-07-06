using Asp.Versioning;
using AutoMapper;
using Fakebook.Application.Posts.Commands;
using Fakebook.Application.Posts.Queries;
using FakeBook.API.Contracts.Posts.Requests;
using FakeBook.API.Contracts.Posts.Responses;
using FakeBook.API.Filters;
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
        public async Task<IActionResult> CreatePost ([FromBody] PostCreate postCreate)
        {
            var cmd = new CreatePostCmd
            {
                Text = postCreate.Text,
                UserProfileId = postCreate.UserProfileId
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
        public async Task<IActionResult> UpdatePost([FromBody] PostUpdate updatedPost, string id)
        {

            var command = new UpdatePostCmd()
            {
                NewText = updatedPost.Text,
                PostId = Guid.Parse(id),
            };
            var result = await _mediator.Send(command);

            return !result.Success ? HandleErrorResponse(result.Errors) : NoContent();
        }

        [HttpDelete]
        [Route(ApiRoutes.Post.RouteId)]
        [ValidateGuid("id")]
        public async Task<IActionResult> DeletePost(string id, CancellationToken cancellationToken)
        {
            var command = new DeletePostCmd() { PostId = Guid.Parse(id)};
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
            //var userProfileId = HttpContext.GetUserProfileIdClaimValue();

            var command = new AddPostCommentCmd()
            {
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
            //var userProfileId = HttpContext.GetUserProfileIdClaimValue();
            var postGuid = Guid.Parse(postId);
            var commentGuid = Guid.Parse(commentId);
            var command = new RemovePostCommentCmd
            {
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



    }
}
