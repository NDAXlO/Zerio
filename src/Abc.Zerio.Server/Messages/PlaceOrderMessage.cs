namespace Abc.Zerio.Server.Messages
{
    public class PlaceOrderMessage
    {
        public long Id;
        public int InstrumentId;
        public double Price;
        public double Quantity;
       
        public OrderSide Side;

        public long val1;
        public long val2;
        public long val3;
        public long val4;
        public long val5;
        public long val6;
        public long val7;
        public long val8;
        public long val9;
        public long val0;
        public long val11;
        public long val12;
        public long val13;
        public long val14;
        public long val15;

        public override string ToString()
        {
            return $"Id: {Id}, InstrumentId: {InstrumentId}, Price: {Price}, Quantity: {Quantity}, Side: {Side}";
        }
    }
}
