using System.Runtime.Serialization;

namespace wcfcleaners.Model
{
    [DataContract]
    public class Machine
    {
        private string _code;
        private bool _isActive;
        private double _efficiency;

        [DataMember]
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
            }
        }

        [DataMember]
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
            }
        }

        [DataMember]
        public double Efficiency
        {
            get => _efficiency;
            set
            {
                _efficiency = value;
            }
        }
    }
}