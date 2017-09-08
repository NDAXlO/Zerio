using System;
using Abc.Zerio.Framing;
using Abc.Zerio.Serialization;
using Abc.Zerio.Server.Messages;

namespace Abc.Zerio.Server.Serializers
{
    public class PlaceOrderMessageSerializer : BinaryMessageSerializer<PlaceOrderMessage>
    {
        private const byte _version = 1;

        public override void Serialize(PlaceOrderMessage message, UnsafeBinaryWriter binaryWriter)
        {
            binaryWriter.Write(_version);

            binaryWriter.Write(message.Id);
            binaryWriter.Write(message.InstrumentId);
            binaryWriter.Write(message.Price);
            binaryWriter.Write(message.Quantity);
            binaryWriter.Write((byte)message.Side);

            binaryWriter.Write(message.val0);
            binaryWriter.Write(message.val1);
            binaryWriter.Write(message.val2);
            binaryWriter.Write(message.val3);
            binaryWriter.Write(message.val4);
            binaryWriter.Write(message.val5);
            binaryWriter.Write(message.val6);
            binaryWriter.Write(message.val7);
            binaryWriter.Write(message.val8);
            binaryWriter.Write(message.val9);
            binaryWriter.Write(message.val11);
            binaryWriter.Write(message.val12);
            binaryWriter.Write(message.val13);
            binaryWriter.Write(message.val14);
            binaryWriter.Write(message.val15);
        }

        public override void Deserialize(PlaceOrderMessage message, UnsafeBinaryReader binaryReader)
        {
            var version = binaryReader.ReadByte();
            if (version != _version)
                throw new InvalidOperationException($"version mismatch; expected:{_version} received:{version}");

            message.Id = binaryReader.ReadInt64();
            message.InstrumentId = binaryReader.ReadInt32();
            message.Price = binaryReader.ReadDouble();
            message.Quantity = binaryReader.ReadDouble();
            message.Side = (OrderSide)binaryReader.ReadByte();

            message.val0 = binaryReader.ReadInt64();
            message.val1 = binaryReader.ReadInt64();
            message.val2 = binaryReader.ReadInt64();
            message.val3 = binaryReader.ReadInt64();
            message.val4 = binaryReader.ReadInt64();
            message.val5 = binaryReader.ReadInt64();
            message.val6 = binaryReader.ReadInt64();
            message.val7 = binaryReader.ReadInt64();
            message.val8 = binaryReader.ReadInt64();
            message.val9 = binaryReader.ReadInt64();
            message.val11 = binaryReader.ReadInt64();
            message.val12 = binaryReader.ReadInt64();
            message.val13 = binaryReader.ReadInt64();
            message.val14 = binaryReader.ReadInt64();
            message.val15 = binaryReader.ReadInt64();
        }
    }
}
