using System.Runtime.Serialization;

namespace wcfcleaners.Model
{
    [DataContract]
    public class Operator
    {
        private int _id;
        private string _fullName;
        private string _pin;
        private string _jobType;
        private string _shift;

        [DataMember]
        public int Id
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
        public string Pin
        {
            get => _pin;
            set
            {
                _pin = value;
            }
        }

        [DataMember]
        public string JobType
        {

            get => _jobType;
            set
            {
                _jobType = value;
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