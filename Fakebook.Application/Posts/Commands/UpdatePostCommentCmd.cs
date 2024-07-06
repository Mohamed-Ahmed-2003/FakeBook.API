﻿
using Fakebook.Application.Generics;
using FakeBook.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Fakebook.Application.Posts.Commands;
public class UpdatePostCommentCmd : IRequest<Response<PostComment>>
{
    public Guid UserProfileId { get; set; }
    public Guid PostId { get; set; }
    public Guid CommentId { get; set; }
    public string UpdatedText { get; set; }
}