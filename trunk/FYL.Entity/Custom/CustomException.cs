using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace FYL.Entity
{
    [Serializable]
    public class CustomException : Exception
    {
        public CustomException() { }

        public CustomException(string msg) : base(msg) { }

        public CustomException(string msg, Exception innerExp) : base(msg, innerExp) { }

        public CustomException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
