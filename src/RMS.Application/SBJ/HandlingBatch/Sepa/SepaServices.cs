using RMS.SBJ.HandlingBatch.Models;
using System;
using System.Linq;
using RMS.SBJ.HandlingBatch.Extensions;

namespace RMS.SBJ.HandlingBatch.Sepa
{
    public class SepaServices
    {
        public string GenerateSepaXml(SepaPaymentBatch paymentBatch)
        {
            var numberOfTransactions = paymentBatch.Payments.Count;
            var controlSum = Math.Round(paymentBatch.Payments.Sum(p => p.Amount), 2);

            var transactions = new System.Collections.ObjectModel.Collection<CreditTransferTransactionInformation10>();

            foreach (var payment in paymentBatch.Payments)
            {
                var transaction = new CreditTransferTransactionInformation10
                {
                    PmtId = new PaymentIdentification1
                    {
                        EndToEndId = payment.Id.ToString()
                    },
                    Amt = new AmountType3Choice
                    {
                        InstdAmt = new ActiveOrHistoricCurrencyAndAmount
                        {
                            Ccy = payment.Currency,
                            Value = payment.Amount
                        }
                    },
                    CdtrAgt = new BranchAndFinancialInstitutionIdentification4
                    {
                        FinInstnId = new FinancialInstitutionIdentification7
                        {
                            BIC = payment.BIC
                        }
                    },
                    Cdtr = new PartyIdentification32
                    {
                        Nm = payment.Name,
                        PstlAdr = new PostalAddress6
                        {
                            AdrTp = payment.AddressType,
                            AdrLine = new System.Collections.ObjectModel.Collection<string> { payment.AddressLine },
                            PstCd = payment.PostcalCode,
                            TwnNm = payment.TownName,
                            Ctry = payment.Country
                        },
                        CtctDtls = new ContactDetails2
                        {
                            Nm = payment.Name,
                            EmailAdr = payment.EmailAddress
                        }
                    },
                    CdtrAcct = new CashAccount16
                    {
                        Id = new AccountIdentification4Choice
                        {
                            IBAN = payment.IBAN
                        }
                    },
                    Purp = new Purpose2Choice
                    {
                        Cd = payment.PurposeCode
                    },
                    RmtInf = new RemittanceInformation5
                    {
                        Ustrd = new System.Collections.ObjectModel.Collection<string>() { payment.UnstructuredInfo }
                    }
                };
                transactions.Add(transaction);
            }

            var sepaDocument = new Document()
            {
                CstmrCdtTrfInitn = new CustomerCreditTransferInitiationV03()
                {
                    GrpHdr = new GroupHeader32()
                    {
                        MsgId = paymentBatch.MessageId,
                        CreDtTm = DateTime.Now,
                        NbOfTxs = numberOfTransactions.ToString(),
                        CtrlSumSpecified = true,
                        CtrlSum = controlSum,
                        InitgPty = new PartyIdentification32()
                        {
                            Nm = paymentBatch.InitiatorName
                        }
                    },
                    PmtInf = new System.Collections.ObjectModel.Collection<PaymentInstructionInformation3>
                    {
                        new PaymentInstructionInformation3
                        {
                            PmtInfId = paymentBatch.MessageId,
                            PmtMtd = paymentBatch.PaymentMethod,
                            NbOfTxs = numberOfTransactions.ToString(),
                            CtrlSumSpecified = true,
                            CtrlSum = controlSum,
                            PmtTpInf = new PaymentTypeInformation19
                            {
                                InstrPrtySpecified = true,
                                InstrPrty = paymentBatch.InstructionPriority,
                                SvcLvl = new ServiceLevel8Choice
                                {
                                    Cd = paymentBatch.ServiceLevelCode
                                }
                            },
                            ReqdExctnDt =  paymentBatch.RequestedExecutionDate,
                            Dbtr = new PartyIdentification32()
                            {
                                Nm = paymentBatch.InitiatorName
                            },
                            DbtrAgt = new BranchAndFinancialInstitutionIdentification4()
                            {
                                FinInstnId = new FinancialInstitutionIdentification7
                                {
                                    BIC = paymentBatch.BIC
                                }
                            },
                            DbtrAcct = new CashAccount16()
                            {
                                Id = new AccountIdentification4Choice()
                                {
                                    IBAN = paymentBatch.IBAN
                                }
                            },
                            ChrgBrSpecified = true,
                            ChrgBr = paymentBatch.ServiceLevel,
                            CdtTrfTxInf = transactions
                        }
                    } // PmtInf
                } // CstmrCdtTrfInitn
            };

            // Make XML from sepaDocument
            var xmlString = sepaDocument.Serialize();
            return xmlString;
        }
    }
}
