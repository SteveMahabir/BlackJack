using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;

namespace BlackJackContracts
{
    // This is the Callback Contract that each client will implement
    [ServiceContract]
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateGui(CallbackInfo info);
    }

    [ServiceContract(CallbackContract = typeof(ICallback))]
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
}
