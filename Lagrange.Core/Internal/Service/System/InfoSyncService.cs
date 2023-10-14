using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(InfoSyncEvent))]
[Service("trpc.msg.register_proxy.RegisterProxy.SsoInfoSync")]
internal class InfoSyncService : BaseService<InfoSyncEvent>
{
    protected override bool Build(InfoSyncEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new SsoInfoSync
        {
            SyncType = 143,
            RandomSeq = input.Random,
            Type = 2,
            GroupLastMsgTime = 0,
            C2C = new SsoInfoSyncC2C
            {
                Empty = Array.Empty<byte>(),
                C2CMsgLastTime = 0
            }
        };
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out InfoSyncEvent output, out List<ProtocolEvent>? extraEvents)
    {
        output = InfoSyncEvent.Result(0);
        extraEvents = null;
        return true;
    }
}