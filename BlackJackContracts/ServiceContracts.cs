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

        #region Player / Dealer Functions

        [OperationContract]
        Player GetPlayerbyId(int id);

        [OperationContract]
        Player GetDealer();

        #endregion

        #region CallBack Functions
        [OperationContract]
        int RegisterForCallbacks();

        [OperationContract(IsOneWay = true)]
        void UnregisterForCallbacks(int id);

        #endregion

        #region Main Game Logic

        [OperationContract]
        void Hit(int id);

        [OperationContract]
        void Stay(int id);

        [OperationContract]
        void Bet(int id, int betAmount);

        [OperationContract]
        void StartGame(int id);

        [OperationContract]
        void ClearMe(int myCallbackId);

        #endregion
    }
}
