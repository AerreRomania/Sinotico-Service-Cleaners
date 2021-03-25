using System;
using System.Runtime.Serialization;

namespace wcfcleaners.Model
{
    [DataContract]
    public class OperatorsJob
    {
        private long _id;
        private string _fullName;
        private string _line;
        private DateTime _startTime;
        private DateTime _endTime;
        private string _shift;

        [DataMember]
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
            }
        }

        [DataMember]
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
            }
        }

        [DataMember]
        public string Line
        {
            get => _line;
            set
            {
                _line = value;
            }
        }

        [DataMember]
        public DateTime StartTime
        {

            get => _startTime;
            set
            {
                _startTime = value;
            }
        }

        [DataMember]
        public DateTime EndTime
        {

            get => _endTime;
            set
            {
                _endTime = value;
            }
        }

        [DataMember]
        public string Shift
        {
            get => _shift;
            set
            {
                _shift = value;
            }
        }
}
}