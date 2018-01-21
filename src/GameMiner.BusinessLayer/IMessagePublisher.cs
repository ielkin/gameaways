using System;

namespace GameMiner.BusinessLayer
{
    public interface IMessagePublisher
    {
        void Publish(string message, string queue, DateTime enqueueTime);
    }
}