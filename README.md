#Вывод до исправления

''' Bash
Test result debit
        false | 7,44 | 16,54 | 0,001 | Account not debited correctly | 

Test Exception debit
        true | System.ArgumentOutOfRangeException | System.ArgumentOutOfRangeException | Debit | Debit amount is less than zero (Parameter 'amount')
Actual value was -100. | 

Debit_ExceedsBalance_Message
        True | True | True | Wrong exception message for exceeding balance | 

Debit_Negative_Message
        True | True | True | Wrong exception message for negative amount | 

Test result credit
        True | 16,54 | 16,54 | 0,001 | Account not debited correctly | 

Test Exception credit
        true | System.ArgumentOutOfRangeException | System.ArgumentOutOfRangeException | Credit | Credit amount is less than zero (Parameter 'amount')
Actual value was -100. | 

Credit_Negative_Message
        True | True | True | Wrong exception message for negative credit |
'''

1 тест false 

#Вывод после исправления

''' Bash
Test result debit
        True | 7,44 | 7,44 | 0,001 | Account not debited correctly | 

Test Exception debit
        true | System.ArgumentOutOfRangeException | System.ArgumentOutOfRangeException | Debit | Debit amount is less than zero (Parameter 'amount')
Actual value was -100. | 

Debit_ExceedsBalance_Message
        True | True | True | Wrong exception message for exceeding balance | 

Debit_Negative_Message
        True | True | True | Wrong exception message for negative amount | 

Test result credit
        True | 16,54 | 16,54 | 0,001 | Account not debited correctly | 

Test Exception credit
        true | System.ArgumentOutOfRangeException | System.ArgumentOutOfRangeException | Credit | Credit amount is less than zero (Parameter 'amount')
Actual value was -100. | 

Credit_Negative_Message
        True | True | True | Wrong exception message for negative credit | 
        '''
все тесты True
