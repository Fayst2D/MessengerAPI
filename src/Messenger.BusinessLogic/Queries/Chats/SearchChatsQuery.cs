﻿using MediatR;
using Messenger.BusinessLogic.Models;
using Messenger.Data.Database;
using Messenger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Queries.Chats;

public class SearchChatsQuery : IRequest<Response<IEnumerable<Chat>>>
{
    public string Title { get; init; } = "";
}

public class SearchChatsHandler : IRequestHandler<SearchChatsQuery, Response<IEnumerable<Chat>>>
{
    private readonly DatabaseContext _context;

    public SearchChatsHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<IEnumerable<Chat>>> Handle(SearchChatsQuery request, CancellationToken cancellationToken)
    {
        var chats = await _context.Chats
            .Where(x => x.ChatType == (int)ChatTypes.Channel)
            .Where(x => EF.Functions.Like(x.Title, $"%{request.Title}%"))
            .Select(chatEntity => new Chat
            {
                Id = chatEntity.Id,
                Title = chatEntity.Title,
                MembersCount = chatEntity.MembersCount,
                ChatType = chatEntity.ChatType,
                Image = chatEntity.Image
            }).Take(100).ToListAsync(cancellationToken);
        
        return Response.Ok<IEnumerable<Chat>>("Ok", chats);
    }
}