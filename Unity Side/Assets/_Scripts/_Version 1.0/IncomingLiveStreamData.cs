
    public class IncomingLiveStreamData
    {
        public AU[] _actionUnits;
        public double frame;
        public double timestamp;
        public IncomingLiveStreamData(AU[] actionUnits, double frame, double timestamp)
        {
            _actionUnits = actionUnits;
            this.frame = frame;
            this.timestamp = timestamp;
        }
    }