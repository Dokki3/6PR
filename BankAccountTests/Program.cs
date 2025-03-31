using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Linq;
using BankAccount;

namespace BankAccountTests {

    public class Assert {

        private Dictionary<string, List< List<string>>> _table;

        public Assert() {
            _table = new Dictionary<string, List< List<string>>>();
        }

        public void AreEqual(double expected, double actual, double error, string description, string nameTest) {
            
            try {
                _table[nameTest].GetType();
            } catch (KeyNotFoundException) {
                _table[nameTest] = new List<List<string>>();
            }
            
            if (Math.Abs(actual - expected) < error) {
                _table[nameTest].Add( new List<string>() {"True", expected.ToString(), actual.ToString(), error.ToString() ,description});
            } else {
                _table[nameTest].Add( new List<string>() {"false", expected.ToString(), actual.ToString(), error.ToString() ,description});
            }
        }

        public void AreEqual(string expected, string actual, string description, string nameTest) {
            try {
                _table[nameTest].GetType();
            } catch (KeyNotFoundException) {
                _table[nameTest] = new List<List<string>>();
            }
            
            if (expected == actual) {
                _table[nameTest].Add(new List<string>() {"True", expected, actual, description});
            } else {
                _table[nameTest].Add(new List<string>() {"False", expected, actual, description});
            }
        }

        public void ThrowsException<T>(string nameTest, Delegate method, params object[] args) {
            try {
                method.DynamicInvoke(args);
            } catch (Exception e) {
                
                try {
                    _table[nameTest].GetType();
                } catch (KeyNotFoundException) {
                    _table[nameTest] = new List<List<string>>();
                }
                
                int index = e.InnerException.ToString().IndexOf(':'); 
                
                if (e.InnerException is T) {
                    _table[nameTest].Add( new List<string>() {"true", e.InnerException.ToString().Substring(0, index), typeof(T).ToString(), method.Method.Name, e.InnerException.Message});
                } else {
                    _table[nameTest].Add( new List<string>() {"false", e.InnerException.ToString().Substring(0, index), typeof(T).ToString(), method.Method.Name, e.InnerException.Message});
                }
            }

        }

        public void Contains(string actual, string expectedSubstring, string description, string nameTest) {
            bool contains = actual.Contains(expectedSubstring);
            AreEqual(contains.ToString(), "True", description, nameTest);
        }

        public void GetTable() {
            foreach (KeyValuePair<string, List<List<string>>> key in _table) {
                Console.WriteLine(key.Key);
                foreach (List<string> i in key.Value) {
                    Console.Write('\t');
                    foreach (string j in i) {
                        Console.Write($"{j} | ");
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
        }

    }
    
    class BankAccountTests {

        Assert assert = new Assert();

        public void Debit_WithValidAmount_UpdatesBalance() {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = 4.55;
            double expected = 7.44;
            BankAccount.BankAccount account = new BankAccount.BankAccount("Mr. Roman Abramovich", beginningBalance);

            // Act
            account.Debit(debitAmount);

            // Assert
            double actual = account.Balance;

            assert.AreEqual(expected, actual, 0.001, "Account not debited correctly", "Test result debit");
        }

        public void Debit_WhenAmountIsLessThanZero_ShouldThrowArgumentOutOfRange() {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = -100.00;
            BankAccount.BankAccount account = new BankAccount.BankAccount("Mr. Roman Abramovich", beginningBalance);

            // Act and assert
            assert.ThrowsException<System.ArgumentOutOfRangeException>("Test Exception debit", new Action<double>(account.Debit), debitAmount);
        }

        public void Debit_WhenAmountIsMoreThanBalance_ShouldThrowWithMessage() {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = 20.0;
            var account = new BankAccount.BankAccount("Mr. Bryan Walton", beginningBalance);

            // Act
            try {
                account.Debit(debitAmount);
            } catch (ArgumentOutOfRangeException e) {
                // Assert
                assert.Contains(e.Message, 
                            BankAccount.BankAccount.DebitAmountExceedsBalanceMessage,
                            "Wrong exception message for exceeding balance",
                            "Debit_ExceedsBalance_Message");
                return;
            }
            assert.AreEqual("Exception", "Not thrown", "Expected exception was not thrown", "Debit_ExceedsBalance");
        }

        public void Debit_WhenAmountIsNegative_ShouldThrowWithMessage() {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = -100.0;
            var account = new BankAccount.BankAccount("Mr. Bryan Walton", beginningBalance);

            // Act
            try {
                account.Debit(debitAmount);
            } catch (ArgumentOutOfRangeException e) {
                // Assert
                assert.Contains(e.Message,
                            BankAccount.BankAccount.DebitAmountLessThanZeroMessage,
                            "Wrong exception message for negative amount",
                            "Debit_Negative_Message");
                return;
            }
            assert.AreEqual("Exception", "Not thrown", "Expected exception was not thrown", "Debit_Negative");
        }

        public void Credit_WithValidAmount_UpdatesBalance() {
            // Arrange
            double beginningBalance = 11.99;
            double creditAmount = 4.55;
            double expected = 16.54;
            BankAccount.BankAccount account = new BankAccount.BankAccount("Mr. Roman Abramovich", beginningBalance);

            // Act
            account.Credit(creditAmount);

            // Assert
            double actual = account.Balance;

            assert.AreEqual(expected, actual, 0.001, "Account not debited correctly", "Test result credit");
        }

        public void Credit_WhenAmountIsLessThanZero_ShouldThrowArgumentOutOfRange() {
            // Arrange
            double beginningBalance = 11.99;
            double creditAmount = -100.00;
            BankAccount.BankAccount account = new BankAccount.BankAccount("Mr. Roman Abramovich", beginningBalance);

            // Act and assert
            assert.ThrowsException<System.ArgumentOutOfRangeException>("Test Exception credit", new Action<double>(account.Credit), creditAmount);
        }

        public void Credit_WhenAmountIsNegative_ShouldThrowWithMessage() {
            // Arrange
            double beginningBalance = 11.99;
            double creditAmount = -100.0;
            var account = new BankAccount.BankAccount("Mr. Bryan Walton", beginningBalance);

            // Act
            try {
                account.Credit(creditAmount);
            } catch (ArgumentOutOfRangeException e) {
                // Assert
                assert.Contains(e.Message,
                            BankAccount.BankAccount.CreditAmountLessThanZeroMessage,
                            "Wrong exception message for negative credit",
                            "Credit_Negative_Message");
                return;
            }
            assert.AreEqual("Exception", "Not thrown", "Expected exception was not thrown", "Credit_Negative");
        }

        static void Main() {
            BankAccountTests tests = new BankAccountTests();

            // Дебитовые тесты
            tests.Debit_WithValidAmount_UpdatesBalance();
            tests.Debit_WhenAmountIsLessThanZero_ShouldThrowArgumentOutOfRange();
            tests.Debit_WhenAmountIsMoreThanBalance_ShouldThrowWithMessage();
            tests.Debit_WhenAmountIsNegative_ShouldThrowWithMessage();

            // Кредитовые тесты
            tests.Credit_WithValidAmount_UpdatesBalance();
            tests.Credit_WhenAmountIsLessThanZero_ShouldThrowArgumentOutOfRange();
            tests.Credit_WhenAmountIsNegative_ShouldThrowWithMessage();

            tests.assert.GetTable();
        }
    }
}

