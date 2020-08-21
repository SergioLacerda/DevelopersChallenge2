using System;
using System.IO;
using OfxSharp;
using System.Linq;
using System.Collections.Generic;

namespace Parser
{
    public class Parser{
        //I created the methods to get and load files from IO. When i found a way to parse without external libs i will use.
        //I started on this aproach, but don't have sucess.
        public string[] getAllFileNames(string sourcePath) {
            return Directory.GetFiles(sourcePath);
        }

        public string readFile(string sourcePath) {
           try{
                using (var sr = new StreamReader(sourcePath)){
                    return sr.ReadToEnd();
                }
            }
            catch (IOException e){
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return "";
        }

        public List<TransactionDTO> getTransactionsFromOFXFile(string rawFile) {

            //Until now a can't create a parser direct from String, or found a way through XML Parser too.
           var parser = new OFXDocumentParser();
           var ofxDocument = parser.Import(new FileStream(rawFile, FileMode.Open));

           var rawTransaction = ofxDocument.Transactions.GroupBy(transaction => transaction.Date.ToString("yyyymmdd"),
                                                             transaction => transaction.Amount, 
                                                             (date, amount) => new {date, amount});

            List<TransactionDTO> result = new List<TransactionDTO>();
            foreach (var transaction in rawTransaction){
                TransactionDTO dto = new TransactionDTO();

                List<decimal> ammounts = new List<decimal>();
                foreach (var amount in transaction.amount){
                       ammounts.Add(amount);
                }

                dto.Date = transaction.date;
                dto.Amounts = ammounts;
                dto.TotalAmount = transaction.amount.Sum();
                
                result.Add(dto);
            }

            return result;
        }
    }
}
