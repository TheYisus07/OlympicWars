using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using ContractsOW;

namespace Server
{
    public class ServerPlayerProxy : DuplexClientBase<IPlayerService>
    {
        public ServerPlayerProxy(IPlayerServiceCallback callbackInstance) : base(callbackInstance)
        { }

    }
}
