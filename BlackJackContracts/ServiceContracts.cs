using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;

namespace BlackJackContracts
{
    [ServiceContract]
    public interface IGame
    {
        [OperationContract]
        Card Hit();

        [OperationContract]
        void Stay();

        [OperationContract]
        void Bet();

        [OperationContract]
        void StartGame();

    }
    class ServiceContracts
    {
    }
}
