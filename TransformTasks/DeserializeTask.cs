using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TransformTasks
{
    //cladd to create objects for dictionaries
    [DataContract]
    class SerializeTask
    {
        [DataMember]
        public string Node { get; set; }
        [DataMember]
        public string TaskKey { get; set; }
        [DataMember]
        public string TaskValue { get; set; }

        public SerializeTask()
        {

        }
    }

        
        
    
   
}
