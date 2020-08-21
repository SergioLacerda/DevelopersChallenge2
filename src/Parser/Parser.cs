using System;
using System.IO;
using OfxSharp;
using System.Linq;
using System.Collections.Generic;

namespace Parser
{
    public class Parser{
        /*
            I created the methods to get and load files from IO. When i found a way to parse without external libs i will use.
            I started on this aproach, but don't have sucess.

            I do something nearly in NodeJS clawler(github), following the same way:
            1 - Create DTO object from raw string, or try to parse XML for each Transaction
            2 - Collect all transaction between files
            3 - Remove duplicated
        */
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

        public List<TransactionDTO> getTransactionsFromOFXFile(string filePath) {

            //Until now a can't create a parser direct from String, or found a way through XML Parser too.
           var parser = new OFXDocumentParser();
           var ofxDocument = parser.Import(new FileStream(filePath, FileMode.Open));

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

        public List<TransactionDTO> getTransactionsFromOFXFile(List<TransactionDTO> transactionDTOs) {
            /*
            Sorry, i forget the most importante thing: check and remove duplicated transactions
            For this i can do two approachs:
            1 - I can Parse all files at once, collecting "ofxDocument.Transactions" into one list,
                then call GroupBy lambda once.
            2 - Call the method "getTransactionsFromOFXFile" for each file, colleting outputs into one object,
                then here in "getTransactionsFromOFXFile" intersect and remove duplicateds,
            */

            return null;
        }
    }
}
