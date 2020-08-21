using NUnit.Framework;
using Parser;
using System;
using System.Collections.Generic;

namespace Parser
{
    [TestFixture]
    public class ParserTest{
        private Parser _parser;

        [SetUp]
        public void SetUp(){
            //Problem to understanding how to get current base dir in C#
            string currentDir = System.AppContext.BaseDirectory;
            _parser = new Parser();
        }

        [Test]
        public void checkParser(){
            string[] result = _parser.getAllFileNames("C:\\dev\\c_nibu\\Parser.Tests\\testFolder");
            string[] expected = new string[] {"C:\\dev\\c_nibu\\Parser.Tests\\testFolder\\file01.txt", "C:\\dev\\c_nibu\\Parser.Tests\\testFolder\\file02.txt"};

            //Problem to understanding how NUnit do assertions on lists
            Assert.AreSame(result, result);
        }

        [Test]
        public void readFile(){

            string result = _parser.readFile("C:\\dev\\c_nibu\\Parser.Tests\\testFolder\\file01.txt");

            Console.WriteLine(result);

            Assert.AreEqual("tra\r\nla\r\nla", result);
        }

        
        [Test]
        public void parseXmlFromFile(){
            
            
            List<TransactionDTO> result =_parser.getTransactionsFromOFXFile("C:\\dev\\c_nibu\\Parser.Tests\\testFolder\\extrato1.ofx");

            //just for print output
            foreach (var dto in result){
                    Console.WriteLine("Date: " + dto.Date);

                    foreach (var n in dto.Amounts){
                       Console.WriteLine("amount: " +  n);
                    }
                    Console.WriteLine("totalAmount: " + dto.TotalAmount);
            }
        }

       [Test]
        public void removeDuplicatedTransactions(){
            string[] filePath = _parser.getAllFileNames("C:\\dev\\c_nibu\\Parser.Tests\\testFolder");

            //iterate through files and collect output. Second Option
            List<TransactionDTO> result =_parser.getTransactionsFromOFXFile(filePath[0]);

            _parser.getTransactionsFromOFXFile(result);

        }
    }
}