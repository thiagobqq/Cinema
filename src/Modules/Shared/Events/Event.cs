using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Shared.Events
{
    public static class EventDispatcher
    {
        private static IMediator? _mediator;

        public static void Configure(IMediator mediator)
        {
            _mediator = mediator;
        }

        public static IMediator Mediator => _mediator 
            ?? throw new InvalidOperationException("EventDispatcher not configured. Call EventDispatcher.Configure() at startup.");
    }

    public abstract class Event : INotification
    {
        public async Task Call()
        {
            await EventDispatcher.Mediator.Publish(this);
        }
    }
}