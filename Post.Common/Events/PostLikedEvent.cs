﻿using CQRS_Core.Events;

namespace Post.Common.Events;

public class PostLikedEvent : BaseEvent
{
    public PostLikedEvent() : base(nameof(PostLikedEvent))
    {
    }
}
