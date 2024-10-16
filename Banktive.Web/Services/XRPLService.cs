using Banktive.Web.Services.Model;
using Xrpl.Client;
using Xrpl.Client.Model;
using Xrpl.Client.Model.Account;
using Xrpl.Client.Model.Transaction;
using Xrpl.Client.Model.Transaction.TransactionTypes;
using Xrpl.Client.Requests.Account;
using Xrpl.Client.Requests.Transaction;
using Xrpl.Wallet;

namespace Banktive.Web.Services
{
    public class XRPLService
    {
        private IConfiguration _configuration { get; set; }
        private RippleClient _rippleClient { get; set; }
        public XRPLService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private void Connect(string url)
        {
            _rippleClient = new RippleClient(url);
            _rippleClient.Connect();
        }

        private void Disconnect()
        {
            try
            {
                _rippleClient.Disconnect();
            }
            catch
            {

            }
        }

        public async Task<NativeBalance> GetNativeBalance(string url, string address)
        {
            NativeBalance nativeBalance = null;
            try
            {
                Connect(url);
                AccountInfoRequest request = new AccountInfoRequest(address);
                AccountInfo accountInfo = await _rippleClient.AccountInfo(request);
                Currency balance = accountInfo.AccountData.Balance;
                nativeBalance = new NativeBalance
                {
                    CurrencyCode = balance.CurrencyCode,
                    Issuer = balance.Issuer,
                    Value = balance.Value,
                    ValueAsNumber = balance.ValueAsNumber,
                    ValueAsXrp = balance.ValueAsXrp
                };
            }
            catch
            {

            }
            finally
            {
                Disconnect();
            }
            return nativeBalance;
        }

        public async Task<NativePaymentResult> CreatePayment(string url, string addressTo, decimal amount, string addressFrom, string seedFrom)
        {
            NativePaymentResult isCreated = new NativePaymentResult { Successful = false };
            try
            {
                Connect(url);

                AccountInfoRequest request = new AccountInfoRequest(addressFrom);
                AccountInfo accountInfo = await _rippleClient.AccountInfo(request);

                PaymentTransaction paymentTransaction = new PaymentTransaction
                {
                    TransactionType = TransactionType.Payment,
                    Destination = addressTo,
                    Amount = new Currency { ValueAsXrp = amount },
                    Account = addressFrom,
                    Sequence = accountInfo.AccountData.Sequence
                };
                isCreated.AmountDelivered = paymentTransaction.Amount.ValueAsXrp;
                isCreated.AmountSent = paymentTransaction.Amount.ValueAsXrp;
                isCreated.OriginalAmount = paymentTransaction.Amount.ValueAsXrp;
                
                TxSigner txSigner = TxSigner.FromSecret(seedFrom);
                var jsonSigned = txSigner.SignJson(Newtonsoft.Json.Linq.JObject.Parse(paymentTransaction.ToJson()));
                string txBlob = jsonSigned.TxBlob;
                SubmitBlobRequest requestFund = new SubmitBlobRequest();
                requestFund.TransactionBlob = txBlob;

                Submit submit = await _rippleClient.SubmitTransactionBlob(requestFund);
                if (submit.EngineResult == "tesSUCCESS")
                {
                    if(submit.Transaction != null && submit.Transaction.Fee != null)
                    {
                        isCreated.FeeAmount = submit.Transaction.Fee.ValueAsXrp;
                    }
                    isCreated.Successful = true;
                }
            }
            catch
            {
                isCreated.Successful = false;
            }
            finally
            {
                Disconnect();
            }
            
            return isCreated;
        }

        public async Task<XRPLCreateEscrowResult> CreateEscrow(string url, string addressTo, decimal amount, string addressFrom, string seedFrom, DateTime dateToPay)
        {
            XRPLCreateEscrowResult escrowResult = new XRPLCreateEscrowResult { Successful = false };
            try
            {
                Connect(url);

                AccountInfoRequest request = new AccountInfoRequest(addressFrom);
                AccountInfo accountInfo = await _rippleClient.AccountInfo(request);

                long unitDate = (long)(dateToPay - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

                uint release_date_ripple = (uint)(unitDate - 946684800);
                EscrowCreateTransaction escrowCreateTransaction = new EscrowCreateTransaction
                {
                    TransactionType = TransactionType.EscrowCreate,
                    Destination = addressTo,
                    Amount = new Currency { ValueAsXrp = amount },
                    Account = addressFrom,
                    FinishAfter = dateToPay,
                    Sequence = accountInfo.AccountData.Sequence
                };

                escrowResult.AmountDelivered = escrowCreateTransaction.Amount.ValueAsXrp;
                escrowResult.AmountSent = escrowCreateTransaction.Amount.ValueAsXrp;
                escrowResult.OriginalAmount = escrowCreateTransaction.Amount.ValueAsXrp;

                TxSigner txSigner = TxSigner.FromSecret(seedFrom);
                var jsonSigned = txSigner.SignJson(Newtonsoft.Json.Linq.JObject.Parse(escrowCreateTransaction.ToJson()));
                string txBlob = jsonSigned.TxBlob;
                SubmitBlobRequest requestFund = new SubmitBlobRequest();
                requestFund.TransactionBlob = txBlob;

                Submit submit = await _rippleClient.SubmitTransactionBlob(requestFund);
                if (submit.EngineResult == "tesSUCCESS")
                {
                    if (submit.Transaction != null && submit.Transaction.Hash != null)
                    {
                        escrowResult.FeeAmount = submit.Transaction.Fee.ValueAsXrp;
                        escrowResult.Sequence = submit.Transaction.Sequence;
                        escrowResult.Hash = submit.Transaction.Hash;
                    }
                    escrowResult.Successful = true;
                }
            }
            catch
            {
                escrowResult.Successful = false;
            }
            finally
            {
                Disconnect();
            }
            return escrowResult;
        }

        public async Task<XRPLCreateEscrowResult> FinishEscrow(string url, string addressFrom, string seedFrom, string addressCreator, uint offerSequence)
        {
            XRPLCreateEscrowResult escrowResult = new XRPLCreateEscrowResult { Successful = false };
            try
            {
                Connect(url);

                AccountInfoRequest request = new AccountInfoRequest(addressFrom);
                AccountInfo accountInfo = await _rippleClient.AccountInfo(request);

                EscrowFinishTransaction escrowCreateTransaction = new EscrowFinishTransaction
                {
                    TransactionType = TransactionType.EscrowFinish,
                    Account = addressFrom,
                    OfferSequence = offerSequence, Owner = addressCreator, Sequence = accountInfo.AccountData.Sequence
                };

                //escrowResult.AmountDelivered = escrowCreateTransaction..Amount.ValueAsXrp;
                //escrowResult.AmountSent = escrowCreateTransaction.Amount.ValueAsXrp;
                //escrowResult.OriginalAmount = escrowCreateTransaction.Amount.ValueAsXrp;

                TxSigner txSigner = TxSigner.FromSecret(seedFrom);
                var jsonSigned = txSigner.SignJson(Newtonsoft.Json.Linq.JObject.Parse(escrowCreateTransaction.ToJson()));
                string txBlob = jsonSigned.TxBlob;
                SubmitBlobRequest requestFund = new SubmitBlobRequest();
                requestFund.TransactionBlob = txBlob;

                Submit submit = await _rippleClient.SubmitTransactionBlob(requestFund);
                if (submit.EngineResult == "tesSUCCESS")
                {
                    if (submit.Transaction != null && submit.Transaction.Hash != null)
                    {
                        escrowResult.FeeAmount = submit.Transaction.Fee.ValueAsXrp;
                        escrowResult.Sequence = submit.Transaction.Sequence;
                        escrowResult.Hash = submit.Transaction.Hash;
                    }
                    escrowResult.Successful = true;
                }
            }
            catch
            {
                escrowResult.Successful = false;
            }
            finally
            {
                Disconnect();
            }
            return escrowResult;
        }

        public async Task<XRPLCreateEscrowResult> CancelEscrow(string url, string addressFrom, string seedFrom, string addressCreator, uint offerSequence)
        {
            XRPLCreateEscrowResult escrowResult = new XRPLCreateEscrowResult { Successful = false };
            try
            {
                Connect(url);

                AccountInfoRequest request = new AccountInfoRequest(addressFrom);
                AccountInfo accountInfo = await _rippleClient.AccountInfo(request);


                EscrowCancelTransaction paymentTransaction = new EscrowCancelTransaction
                {
                    TransactionType = TransactionType.EscrowCancel,
                    Account = addressFrom,
                    Sequence = accountInfo.AccountData.Sequence,
                    Owner = addressCreator,
                    OfferSequence = offerSequence
                    //Sequence = accountInfo.AccountData.Sequence
                };
                //escrowResult.AmountDelivered = paymentTransaction.Amount.ValueAsXrp;
                //escrowResult.AmountSent = paymentTransaction.Amount.ValueAsXrp;
                //escrowResult.OriginalAmount = paymentTransaction.Amount.ValueAsXrp;

                TxSigner txSigner = TxSigner.FromSecret(seedFrom);
                var jsonSigned = txSigner.SignJson(Newtonsoft.Json.Linq.JObject.Parse(paymentTransaction.ToJson()));
                string txBlob = jsonSigned.TxBlob;
                SubmitBlobRequest requestFund = new SubmitBlobRequest();
                requestFund.TransactionBlob = txBlob;

                Submit submit = await _rippleClient.SubmitTransactionBlob(requestFund);
                if (submit.EngineResult == "tesSUCCESS")
                {
                    if (submit.Transaction != null && submit.Transaction.Hash != null)
                    {
                        escrowResult.FeeAmount = submit.Transaction.Fee.ValueAsXrp;
                        escrowResult.Sequence = submit.Transaction.Sequence;
                        escrowResult.Hash = submit.Transaction.Hash;
                    }
                    escrowResult.Successful = true;
                }
            }
            catch
            {
                escrowResult.Successful = false;
            }
            finally
            {
                Disconnect();
            }
            return escrowResult;
        }
    }
}
