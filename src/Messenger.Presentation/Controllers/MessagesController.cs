﻿using AutoMapper;
using MediatR;
using Messenger.BusinessLogic.Commands.Messages.Delete;
using Messenger.BusinessLogic.Commands.Messages.Edit;
using Messenger.BusinessLogic.Commands.Messages.Send;
using Messenger.BusinessLogic.Queries.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Messenger.Presentation.Controllers;

[ApiController]
[Route("api/messages")]
[Authorize]
public class MessagesController : BaseApiController
{
    public MessagesController(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }
        
    /// <summary>
    /// Get messages from chat
    /// </summary>
    /// <param name="chatId">Chat's ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200</returns>
    [HttpGet("{chatId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessages([FromRoute]Guid chatId, CancellationToken cancellationToken)
    {
        var getMessagesQuery = new GetMessagesQuery
        {
            ChatId = chatId
        };

        return await Request(getMessagesQuery, cancellationToken);
    }

    /// <summary>
    /// Send message
    /// </summary>
    /// <param name="request">Message text and chat's ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200, 422, 400</returns>
    [HttpPost("send")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request, CancellationToken cancellationToken)
    {
        var sendMessageCommand = _mapper.Map<SendMessageCommand>(request);

        return await Request(sendMessageCommand, cancellationToken);
    }

    /// <summary>
    /// Delete message
    /// </summary>
    /// <param name="request">Message's and chat's IDs</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200, 404, 422</returns>
    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> DeleteMessage([FromBody] DeleteMessageRequest request, CancellationToken cancellationToken)
    {
        var deleteMessageCommand = _mapper.Map<DeleteMessageCommand>(request);

        return await Request(deleteMessageCommand, cancellationToken);
    }

    /// <summary>
    /// Edit message
    /// </summary>
    /// <param name="request">Message's and chat's IDs</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200, 404, 422</returns>
    [HttpPut("edit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> EditMessage([FromBody] EditMessageRequest request, CancellationToken cancellationToken)
    {
        var editMessageCommand = _mapper.Map<EditMessageCommand>(request);

        return await Request(editMessageCommand, cancellationToken);
    }

    /// <summary>
    /// Search messages by text
    /// </summary>
    /// <param name="chatId">Chat's ID</param>
    /// <param name="messageText">Message's text</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status codes: 200</returns>
    [HttpGet("{chatId:guid}/search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchMessage([FromRoute] Guid chatId, [FromQuery] string messageText,
        CancellationToken cancellationToken)
    {
        var searchMessageQuery = new SearchMessagesQuery
        {
            ChatId = chatId,
            MessageText = messageText
        };

        return await Request(searchMessageQuery, cancellationToken);
    }

}