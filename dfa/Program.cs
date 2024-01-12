// Online C# Editor for free
// Write, Edit and Run your C# code using C# Online Compiler

using System;
using System.Collections.Generic;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the number of states");
        string statesCount = Console.ReadLine();
        List <string> states = new();
        Console.WriteLine("Enter the states");
        for (int i = 0; i < int.Parse(statesCount); i++) {
            string state = Console.ReadLine();
            states.Add(state);
        }
        Console.WriteLine("Enter the initial state");
        string firstState = Console.ReadLine();
        Console.WriteLine("Enter the final state");
        string finalState = Console.ReadLine();
        Console.WriteLine("Enter the number of keys");
        string keysCount = Console.ReadLine();
        List <string> keys = new();
        Console.WriteLine("Enter the keys");
        for (int i = 0; i < int.Parse(keysCount); i++) {
            string key = Console.ReadLine();
            keys.Add(key);
        }
        
        Dictionary <string, Dictionary<string, string>> adjacency = new();
        foreach (string state in states) {
            if (!adjacency.ContainsKey(state)) adjacency.Add(state, new Dictionary<string, string>());
        }
        
        for (int j = 0; j < int.Parse(statesCount); j++) {
            foreach (string key in keys) {
                Console.WriteLine($"{states[j]} {key}");
                string input = Console.ReadLine();
                foreach (var (state, dic) in adjacency) {
                    if (state == states[j].ToString()) {
                        adjacency[state].Add(key.ToString(), input);
                    }
                }
            }
        }
        
        foreach (var (a, b) in adjacency) {
            Console.Write(a);
            foreach (var c in b) Console.Write($" {c} ");
                Console.WriteLine();
            }
        int counter = 0;
        while (counter < 5) {
            Console.WriteLine("Enter test string");
            string test = Console.ReadLine();
            DFA(adjacency, test, firstState, finalState);
            counter++;
        }
    }
    static void DFA(Dictionary <string, Dictionary<string, string>> adjacency, string input, string firstState, string finalState) {
        string currentState = firstState;
        foreach (char c in input) {
            foreach (var (edge, key) in adjacency[currentState]) {
                if (edge == c.ToString()) {
                    currentState = key;
                    break;
                }
            }
        }
        if (currentState == finalState) Console.WriteLine("Yes");
        else Console.WriteLine("No");
    }
}