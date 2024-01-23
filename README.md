# Implementation of a DFA
A simple implementation of a DFA, it asks the user for the number of states, the states, the initial state, the final state, the number of keys, and the keys as inputs to build an adjacency dictionary. It also takes test strings to see if they reach the final state. 
- There’s a function that converts an NFA into a DFA, but it’s not included in the console.

## How to Run
- DFA verification: (Uncomment the first block of code and comment the rest) <br><br>
I’ll take a DFA that returns `Yes` when the test string starts with ‘1’;
```
A [1, B]  [0, C]
B [1, B]  [0, B]
C [1, C]  [0, C]
```
1. When prompted, enter the number of states.
```
>> 3
```
2. Enter each state.
```
>> A
>> B
>> C
```
3. Enter the initial state.
```
>> A
```
6. Enter the final state.
```
>> B
```
7. Enter the number of keys.
```
>> 2
```
8. Enter each key.
```
>> 1
>> 0
```
9. Enter the relationship between the states
```
A 1
>> B
A 0
>> C
B 1
```
... etc.<br><br>
10. You can then enter test strings to see if they reach the final state.
```
>> 10
Yes
>> 0111
No
```
- NFA to DFA: 
It converts an NFA to a DFA as follows: <br>
The NFA:
```
A      AB       C
B       A       B
C              AB
```
The corresponding DFA:
```
       0       1
A      AB      C
AB     AB      BC
C      D       AB
BC     A       AB
D      D       D
```